// import { SqlService } from "../services/sqlService";
import { FileService } from "../services/fileService";
import { HttpService } from "../services/http-Service";

export class MainController {
    private constructor(private sqlService, private fileService: FileService, private httpService: HttpService) { }

    public async executeOrder66() {
        // const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
        // var databaseNames: string[] = (await this.sqlService.getSql(getDatabasesQuery)).map(i => i.name);

        const databaseNames = ["Temp", "Other"];
        var connectionString = "server=(localdb)\\mssqllocaldb;database=Temp;Trusted_Connection=true"

        const httpsUrl = "https://raw.githubusercontent.com/seesharper/Dotnet.Script.NuGetMetadataResolver/master/build/project.json";
        const localUrl = `http://localhost:2000/api/test/get?connectionString=${connectionString}&databaseName=${databaseNames[0]}`;
        const httpUurl = "http://www.google.com";
        // this.httpService.get(httpsUrl).catch(r => console.log(r)).then(r => console.log(`1:\n\n ${r}`));
        this.httpService.get(localUrl).catch(r => console.log(r)).then(r => console.log(`2:\n\n ${r}`));
        // this.httpService.get(httpUurl).catch(r => console.log(r)).then(r => console.log(`3:\n\n ${r}`));



        //         const csxFilePath = "C:\\temp\\csxtest\\main.csx";
        //         const csxFileData =
        //             `#! "netcoreapp1.1"
        // #r "nuget:NetStandard.Library,1.6.1"
        // #r "nuget:Microsoft.EntityFrameworkCore.SqlServer,1.1.2"
        // #r "C:\\temp\\gen.dll"

        // using GeneratedNamespace;

        // var context = new GeneratedContext();
        // var logsDbSet = context.Logs;
        // var temp = logsDbSet.Where(l => l.Id == 1);`;

        //         if (await this.fileService.exists(csxFilePath)) {
        //             await this.fileService.delete(csxFilePath);
        //         }
        //         await this.fileService.create(csxFilePath, csxFileData);
    }

    public static create(): MainController {
        const driver = 'msnodesqlv8';
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        const database = 'Test';
        var proxyUrl = process.env.proxy;
        var httpService = new HttpService(proxyUrl);
        // var sqlService = new SqlService(server, database, instance, driver);
        var sqlService = null;
        var fileService = new FileService();
        return new MainController(sqlService, fileService, httpService);
    }
}