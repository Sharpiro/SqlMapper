using SqlMapper.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.SqlClient;
using System.IO;

namespace SqlMapper.Core
{
    public class SchemaService
    {
        private readonly string _connectionString;

        public SchemaService(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IEnumerable<DatabaseDto> CreateDatabaseSchemas()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var databases = GetDatabases(conn);
                return databases;
            }
        }

        public IEnumerable<string> GetDatabasesNames()
        {
            var dbNames = new List<string>();
            const string sqlCommand = "SELECT name from sys.databases WHERE owner_sid != 1";
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                using (var command = new SqlCommand(sqlCommand, conn))
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (dataReader.FieldCount != 1) throw new IndexOutOfRangeException($"Expected 1 fields, but was actually {dataReader.FieldCount}");
                        if (!(dataReader[0] is string dbName)) throw new InvalidDataException("Unable to convert db name to string");
                        dbNames.Add(dbName);
                    }
                }
            }

            return dbNames.ToImmutableList();
        }

        private IEnumerable<DatabaseDto> GetDatabases(SqlConnection conn)
        {
            var databases = new List<DatabaseDto>();
            const string sqlCommand = "SELECT name from sys.databases WHERE owner_sid != 1";
            using (var command = new SqlCommand(sqlCommand, conn))
            using (var dataReader = command.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    if (dataReader.FieldCount != 1) throw new IndexOutOfRangeException($"Expected 3 fields, but was actually {dataReader.FieldCount}");
                    if (!(dataReader[0] is string dbName)) throw new InvalidDataException("Unable to convert db name to string");
                    var db = GetTables(conn, DatabaseFactory.DatabaseDto(dbName));
                    databases.Add(db);
                }
            }
            return databases.ToImmutableList();
        }

        private DatabaseDto GetTables(SqlConnection conn, DatabaseDto database)
        {
            var sqlCommand = $"SELECT TABLE_NAME FROM {database.Name}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            using (var command = new SqlCommand(sqlCommand, conn))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        if (dataReader.FieldCount != 1) throw new IndexOutOfRangeException($"Expected 3 fields, but was actually {dataReader.FieldCount}");
                        if (!(dataReader[0] is string tableName)) throw new InvalidDataException("Unable to convert table name to string");
                        database = GetColumns(conn, database, DatabaseFactory.TableDto(tableName));
                    }
                }
                return database;
            }
        }

        private DatabaseDto GetColumns(SqlConnection conn, DatabaseDto database, TableDto table)
        {
            var sqlCommand = $"SELECT COLUMN_NAME, DATA_TYPE, ORDINAL_POSITION FROM {database.Name}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
            using (var command = new SqlCommand(sqlCommand, conn))
            {
                command.Parameters.AddWithValue("@TableName", table.Name);
                using (var dataReader = command.ExecuteReader())
                {
                    var columns = new List<ColumnDto>();
                    while (dataReader.Read())
                    {
                        if (dataReader.FieldCount != 3) throw new IndexOutOfRangeException($"Expected 3 fields, but was actually {dataReader.FieldCount}");
                        if (!(dataReader[0] is string columnName)) throw new InvalidDataException("Unable to convert column name to string");
                        if (!(dataReader[1] is string dataType)) throw new InvalidDataException("Unable to convert column data type to string");
                        if (!(dataReader[2] is int ordinalPosition)) throw new InvalidDataException("Unable to convert column ordinal position to string");
                        var column = DatabaseFactory.ColumnDto(columnName).WithDataType(dataType).WithOrdinalPosition(ordinalPosition);
                        columns.Add(column);
                    }
                    table = table.WithColumns(columns);
                }
                return database.AddTables(table);
            }
        }
    }
}