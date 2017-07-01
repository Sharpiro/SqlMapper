import * as vscode from "vscode";
import { TrustedSqlService } from "../services/Sql/trusted-sql-service";
// import { MemorySqlService } from "../services/Sql/memory-sql-service";
// import { SqlAuthService } from "../services/Sql/sql-auth-service";
import { FileService } from "../services/fileService";
import { HttpService } from "../services/http-Service";
import { ISqlService } from "../services/Sql/I-sql-service";

export class MainController {
    private constructor(private sqlService: ISqlService, private fileService: FileService, private httpService: HttpService) { }

    public async getInfo(): Promise<void> {
        var commands = (await vscode.commands.getCommands()).filter(c => c.toLowerCase().includes("folder"));
        var path = vscode.workspace.rootPath;
        const x = 5;
        // console.log("restarting omnisharp...");
        // await vscode.commands.executeCommand("o.pickProjectAndStart", 0);
    }

    public async executeOrder66(): Promise<void> {
        try {
            await vscode.workspace.saveAll();
            await vscode.commands.executeCommand("workbench.action.closeAllEditors");

            // get database list
            const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
            var databaseNames: string[] = (await this.sqlService.getSql(getDatabasesQuery)).map(i => i.name);

            // user selection
            var databasePick = await vscode.window.showQuickPick(databaseNames);

            // make request to build assembly and return result data
            this.sqlService.database = databasePick;
            const connectionString = this.sqlService.connectionString;
            const workspaceDir = vscode.workspace.rootPath;
            const localUrl = `http://localhost:2000/api/test/get?connectionString=${connectionString}&databaseName=${databasePick}&workspaceDir=${workspaceDir}`;
            var data = await this.httpService.get(localUrl);
            var jObject = JSON.parse(data);

            // create script file
            // const csxFileData = jObject.script;
            // if (await this.fileService.exists(csxFilePath)) {
            //     await this.fileService.delete(csxFilePath);
            // }
            // await this.fileService.create(csxFilePath, csxFileData);

            // open document
            // await vscode.commands.executeCommand('vscode.openFolder', vscode.Uri.parse("C:\\Users\\U403598\\AppData\\Local\\SqlMapper"));
            // await vscode.commands.executeCommand(command, "C:\\Users\\U403598\\AppData\\Local\\SqlMapper");
            const csxFilePath = jObject.scriptPath;
            var document = await vscode.workspace.openTextDocument(csxFilePath)
            var textEditor = await vscode.window.showTextDocument(document);
            await vscode.commands.executeCommand("o.pickProjectAndStart");
        } catch (err) {
            console.error(err);
            throw err;
        }
    }

    public static create(): MainController {
        const driver = "msnodesqlv8";
        // const server = "localhost";
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        // const instance = null;
        const database = "Temp";
        var proxyUrl = process.env.proxy;
        var httpService = new HttpService(proxyUrl);
        var sqlService = new TrustedSqlService(server, database, instance, driver);
        // var sqlService = new MemorySqlService(server, database, instance, driver);
        // var sqlService = new SqlAuthService(server, database, "test", "test", instance);
        var fileService = new FileService();
        return new MainController(sqlService, fileService, httpService);
    }
}