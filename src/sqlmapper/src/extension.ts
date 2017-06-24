'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
// import * as fetcher from "node-fetch";
// import fetch from "node-fetch";
import * as https from 'https';
import * as http from 'http';
import * as fs from 'fs';
import HttpsProxyAgent = require('https-proxy-agent');
import HttpProxyAgent = require('http-proxy-agent');
import { parse as parseUrl } from 'url';
import { SqlService } from "./services/sqlService"

let controller: any = undefined;

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

    // Use the console to output diagnostic information (console.log) and errors (console.error)
    // This line of code will only be executed once when your extension is activated
    console.log('Congratulations, your extension "sqlmapper" is now active!');


    // The command has been defined in the package.json file
    // Now provide the implementation of the command with  registerCommand
    // The commandId parameter must match the command field in package.json
    let disposable = vscode.commands.registerCommand('extension.order66', async () => {
        const driver = 'msnodesqlv8';
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        const database = 'Test';
        const getDatabasesQuery = "SELECT name from sys.databases WHERE owner_sid != 1";
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

        // var sqlService = new SqlService(server, database, instance, driver);
        // var array: string[] = (await sqlService.getSql(getDatabasesQuery)).map(i => i.name);

        // var pick = vscode.window.showQuickPick(array);
        // pick.then(v => {
        //     console.log(`you picked ${v}`);
        // });

        //write file
        fs.exists(csxFilePath, exists => {
            if (exists) {
                fs.unlink(csxFilePath, err => {
                    if (err) throw err;
                    console.log("It's deleted!");
                    fs.writeFile(csxFilePath, csxFileData, { flag: 'wx' }, err => {
                        if (err) throw err;
                        console.log("It's saved!");
                    });
                })
            }
            else {
                fs.writeFile(csxFilePath, csxFileData, { flag: 'wx' }, err => {
                    if (err) throw err;
                    console.log("It's saved!");
                });
            }
        });

        //open document
        var document = await vscode.workspace.openTextDocument(csxFilePath)
        var textEditor = await vscode.window.showTextDocument(document);


        //     const fullUrl = "https://raw.githubusercontent.com/seesharper/Dotnet.Script.NuGetMetadataResolver/master/build/project.json";
        //     const proxyUrl = "http://proxy.wellsfargo.com:8080";

        //     var agent = new HttpsProxyAgent(proxyUrl);

        //     var parsedUrl: any = parseUrl(fullUrl);
        //     var requestOptions: https.RequestOptions = {
        //         host: parsedUrl.host,
        //         path: parsedUrl.path,
        //         agent: agent,
        //         rejectUnauthorized: false
        //     };

        //     console.log("initiating call");
        //     https.get(requestOptions, response => {
        //         console.log("response received");
        //         response.pipe(process.stdout);
        //     });
    });

    context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export function deactivate() {
}