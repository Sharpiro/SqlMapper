import { ISqlService } from "./I-sql-service";

export class SqlServiceFactory {
    public async createSqlService(sqlOptions: any): Promise<ISqlService> {
        const promise = new Promise<ISqlService>(async (resolve, reject) => {
            let sqlService: ISqlService;
            if (sqlOptions.type === "sql") {
                // const SqlAuthModule = await import('./sql-auth-service');
                let SqlAuthModule = require("./sql-auth-service");
                sqlService = new SqlAuthModule.SqlAuthService(sqlOptions.server, sqlOptions.database,
                    sqlOptions.username, sqlOptions.password, sqlOptions.instance);
            }
            else {
                // const TrustedSqlModule = await import('./trusted-sql-service');
                let TrustedSqlModule = require("./trusted-sql-service");
                sqlOptions.driver = "msnodesqlv8";
                sqlService = new TrustedSqlModule.TrustedSqlService(sqlOptions.server, sqlOptions.database,
                    sqlOptions.instance, sqlOptions.driver);
            }
            resolve(sqlService);
        });
        return promise;
    }
}