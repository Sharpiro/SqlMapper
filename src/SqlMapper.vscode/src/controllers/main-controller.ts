import * as vscode from "vscode";
import { FileService } from "../services/fileService";
import { HttpService } from "../services/http-Service";
import { ISqlService } from "../services/Sql/I-sql-service";
import { SqlServiceFactory } from "../services/Sql/sql-service-factory";

export class MainController {
    private profilePath = `${process.env.LOCALAPPDATA}\\sqlmapper\\profile.json`;
    private profile: any;

    private constructor(private sqlServiceFactory: SqlServiceFactory, private fileService: FileService, private httpService: HttpService) { }

    public async getInfo(): Promise<void> {
        var commands = (await vscode.commands.getCommands()).filter(c => c.toLowerCase().includes("folder"));
        var path = vscode.workspace.rootPath;
        const x = 5;
        // console.log("restarting omnisharp...");
        // await vscode.commands.executeCommand("o.pickProjectAndStart", 0);
    }

    public async buildProfile(): Promise<void> {
        const type = await vscode.window.showQuickPick(["sql", "trusted"]);
        const server = await vscode.window.showInputBox({ prompt: "Server Name" });
        const instance = await vscode.window.showInputBox({ prompt: "Instance Name(optional)" });
        const database = await vscode.window.showInputBox({ prompt: "Database" });
        let username: string;
        let password: string;
        if (type === "sql") {
            username = await vscode.window.showInputBox({ prompt: "Username" });
            password = await vscode.window.showInputBox({ prompt: "Password" });
        }

        this.profile = {
            type: type,
            server: server,
            instance: instance,
            database: database,
            username: username,
            password: password
        }

        await this.fileService.create(this.profilePath, JSON.stringify(this.profile));
    }

    public async executeOrder66(): Promise<void> {
        try {
            if (!this.profile) {
                if (!await this.fileService.exists(this.profilePath))
                    throw new Error("Must first create sql profile");

                const sqlOptionsJson = await this.fileService.open(this.profilePath);
                this.profile = JSON.parse(sqlOptionsJson);
            }

            await vscode.workspace.saveAll();
            await vscode.commands.executeCommand("workbench.action.closeAllEditors");


            const sqlService = await this.sqlServiceFactory.createSqlService(this.profile);

            // get database list
            const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
            var databaseNames: string[] = (await sqlService.getSql(getDatabasesQuery)).map(i => i.name);

            // user selection
            var databasePick = await vscode.window.showQuickPick(databaseNames);

            // make request to build assembly and return result data
            sqlService.database = databasePick;
            const connectionString = sqlService.connectionString;
            const workspaceDir = vscode.workspace.rootPath;
            const localUrl = `http://localhost:2000/api/test/get?connectionString=${connectionString}&databaseName=${databasePick}&workspaceDir=${workspaceDir}&libType=0`;
            var data = <string>(await this.httpService.get(localUrl));
            var jObject = JSON.parse(data);

            // create script file
            // const csxFileData = jObject.script;
            // if (await this.fileService.exists(csxFilePath)) {
            //     await this.fileService.delete(csxFilePath);
            // }
            // await this.fileService.create(csxFilePath, csxFileData);

            // open document
            // await vscode.commands.executeCommand('vscode.openFolder', vscode.Uri.parse("C:\\Users\\U403598\\AppData\\Local\\SqlMapper"));
            // await vscode.commands.executeCommand(command, "C:\\Users\\me\\AppData\\Local\\SqlMapper");
            const csxFilePath = jObject.scriptPath;
            var document = await vscode.workspace.openTextDocument(csxFilePath)
            var textEditor = await vscode.window.showTextDocument(document);
            await vscode.commands.executeCommand("o.pickProjectAndStart");
        } catch (err) {
            console.error(err);
            vscode.window.showErrorMessage(err.message);
        }
    }

    public static async create(): Promise<MainController> {
        // const driver = "msnodesqlv8";
        // const server = "localhost";
        // const server = '(localdb)';
        // const instance = "mssqllocaldb";
        // const instance = null;
        // const database = "Temp";
        // var sqlService = new TrustedSqlService(server, database, instance, driver);
        // var sqlService = new MemorySqlService(server, database, instance, driver);
        // var sqlService = new SqlAuthService(server, database, "test", "test", instance);
        var sqlServiceFactory = new SqlServiceFactory();
        var fileService = new FileService();
        var proxyUrl = process.env.proxy;
        var httpService = new HttpService(proxyUrl);
        return new MainController(sqlServiceFactory, fileService, httpService);
    }
}