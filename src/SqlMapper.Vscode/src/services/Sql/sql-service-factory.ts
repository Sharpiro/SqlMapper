import { ISqlService } from "./I-sql-service";

export class SqlServiceFactory {
    public createSqlService(sqlOptions: any): ISqlService {
        let sqlService: ISqlService;
        if (sqlOptions.type === "sql") {
            let SqlAuthModule = require("./sql-auth-service");
            sqlService = new SqlAuthModule.SqlAuthService(sqlOptions.server, sqlOptions.database,
                sqlOptions.username, sqlOptions.password, sqlOptions.instance);
        }
        else {
            let TrustedSqlModule = require("./trusted-sql-service");
            sqlOptions.driver = "msnodesqlv8";
            sqlService = new TrustedSqlModule.TrustedSqlService(sqlOptions.server, sqlOptions.database,
                sqlOptions.instance, sqlOptions.driver);
        }

        return sqlService;
    }
}