import { ConnectionPool } from "mssql";
import { ISqlService } from "./I-sql-service";
const mssql = require('mssql')

export class SqlAuthService implements ISqlService {
    constructor(
        private server: string, private database: string, private username: string,
        private password: string, private instance = "") {
    }

    public async getSql(sql: string): Promise<any[]> {
        const config = {
            user: this.username,
            password: this.password,
            server: this.server,
            database: this.database
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