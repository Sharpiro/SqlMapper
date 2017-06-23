import * as test from "mssql";
const sql = require('mssql')

export class SqlService {
    public async get() {
        try {
            var config = {
                server: 'localhost',
                database: 'ZLUtilities',
                options: {
                    trustedConnection: true
                }
            };
            // var connString = "server=(localdb)\\mssqllocaldb;database=temp;Trusted_Connection=true"
            // var connString = "server=localhost;database=ZLUtilities;Trusted_Connection=true";
            var temp2 = sql.connect(config);
            // var temp = await new test.ConnectionPool(config);
            // temp.connect(err => {
            //     console.log(err);
            // });
            // var pool = await sql.connect(connString);
            // var result = await sql.query("SELECT * FROM dbo.Logs");
            // console.log(result);
        }
        catch (err) {
            console.log(err);
        }
    }
}