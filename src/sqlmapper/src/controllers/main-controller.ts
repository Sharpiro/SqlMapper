import * as vscode from 'vscode';
// import { SqlService } from "../services/Sql/sqlService";
import { MemorySqlService as SqlService } from "../services/Sql/memory-sql-service";
import { FileService } from "../services/fileService";
import { HttpService } from "../services/http-Service";
import { ISqlService } from "../services/Sql/I-sql-service";

export class MainController {
    private constructor(private sqlService: ISqlService, private fileService: FileService, private httpService: HttpService) { }

    public async executeOrder66() {
        //get database list
        const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
        var databaseNames: string[] = (await this.sqlService.getSql(getDatabasesQuery)).map(i => i.name);
        // const databaseNames = ["Temp", "Other"];
        var connectionString = "server=(localdb)\\mssqllocaldb;database=Temp;Trusted_Connection=true"

        //user selection
        var pick = await vscode.window.showQuickPick(databaseNames);

        //make request to build assembly and return result data
        const localUrl = `http://localhost:2000/api/test/get?connectionString=${connectionString}&databaseName=${databaseNames[0]}`;
        var data = await this.httpService.get(localUrl);
        var jObject = JSON.parse(data);

        //create script file
        const csxFilePath = "C:\\Users\\U403598\\AppData\\Local\\SqlMapper\\main.csx";
        const csxFileData = jObject.script;
        if (await this.fileService.exists(csxFilePath)) {
            await this.fileService.delete(csxFilePath);
        }
        await this.fileService.create(csxFilePath, csxFileData);

        //open document
        var document = await vscode.workspace.openTextDocument(csxFilePath)
        var textEditor = await vscode.window.showTextDocument(document);
    }

    public static create(): MainController {
        const driver = 'msnodesqlv8';
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        const database = 'Test';
        var proxyUrl = process.env.proxy;
        var httpService = new HttpService(proxyUrl);
        // var sqlService = new SqlService(server, database, instance, driver);
        var sqlService = new SqlService(server, database, instance, driver);
        var fileService = new FileService();
        return new MainController(sqlService, fileService, httpService);
    }
}