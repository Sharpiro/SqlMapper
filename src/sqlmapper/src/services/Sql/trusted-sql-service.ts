import { ConnectionPool } from "mssql";
import { ISqlService } from "./I-sql-service";
const mssql = require('mssql/msnodesqlv8')

export class TrustedSqlService implements ISqlService {
    constructor(
        private server: string, private database: string,
        private instance = "", private driver?: string) {
    }

    public async getSql(sql: string): Promise<any[]> {
        var config = {
            driver: this.driver,
            connectionString: `Driver={SQL Server Native Client 11.0};Server={${this.server}\\${this.instance}};Database={${this.database}};Trusted_Connection={yes};`,
        };

        let connectionPool: ConnectionPool;
        try {
            connectionPool = await mssql.connect(config)
            let result = await connectionPool.request().query(sql);
            if (!result || !result.recordsets || result.recordsets.length === 0)
                return [];
            var records = <any[]>result.recordsets[0]
            return records;
        } catch (err) {
            console.log(err);
            throw new Error("An error occurred making SQL call");
        } finally {
            connectionPool.close();
            mssql.close();
        }
    }
}