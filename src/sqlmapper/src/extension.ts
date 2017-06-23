'use strict';
// The module 'vscode' contains the VS Code extensibility API
// Import the module and reference it with the alias vscode in your code below
import * as vscode from 'vscode';
// import * as fetcher from "node-fetch";
// import fetch from "node-fetch";
import * as https from 'https';
import * as http from 'http';
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
    let disposable = vscode.commands.registerCommand('extension.order66', () => {
        var sqlService = new SqlService();
        sqlService.get();

        //     var pick = vscode.window.showQuickPick(["1", "2", "3"]);
        //     pick.then(v => {
        //         console.log(v);
        //     });

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