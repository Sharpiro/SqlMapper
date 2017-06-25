import { SqlService } from "../services/sqlService";
import { FileService } from "../services/fileService";

export class MainController {
    private constructor(private sqlService, private fileService: FileService) { }

    public async executeOrder66() {
        const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
        var array: string[] = (await this.sqlService.getSql(getDatabasesQuery)).map(i => i.name);


        const csxFilePath = "C:\\temp\\csxtest\\main.csx";
        const csxFileData =
            `#! "netcoreapp1.1"
#r "nuget:NetStandard.Library,1.6.1"
#r "nuget:Microsoft.EntityFrameworkCore.SqlServer,1.1.2"
#r "C:\\temp\\gen.dll"

using GeneratedNamespace;

var context = new GeneratedContext();
var logsDbSet = context.Logs;
var temp = logsDbSet.Where(l => l.Id == 1);`;

        if (await this.fileService.exists(csxFilePath)) {
            await this.fileService.delete(csxFilePath);
        }
        await this.fileService.create(csxFilePath, csxFileData);
    }

    public static create(): MainController {
        const driver = 'msnodesqlv8';
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        const database = 'Test';
        var sqlService = new SqlService(server, database, instance, driver);
        var fileService = new FileService();
        return new MainController(sqlService, fileService);
    }
}