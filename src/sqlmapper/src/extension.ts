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
        const message = "All the jedi are now dead...";
        vscode.window.showInformationMessage(message);

        const fullUrl = "https://raw.githubusercontent.com/seesharper/Dotnet.Script.NuGetMetadataResolver/master/build/project.json";
        const proxyUrl = vscode.workspace.getConfiguration("http").get("proxy").toString();
        const proxyUrlConst = "http://proxy.wellsfargo.com:8080";
        var compare = proxyUrl === proxyUrlConst;
        // var proxyPort = 8080;
        const testUrl = "http://domain.com:3000/path/to/something?query=string#fragment";
        var parsedTestUrl = parseUrl(testUrl);
        var parsedProxyUrl = parseUrl(proxyUrlConst);
        var proxyOptions = {
            host: parsedProxyUrl.hostname,
            port: Number(parsedProxyUrl.port),
            auth: parsedProxyUrl.auth,
            rejectUnauthorized: false
        }

        var agent = new HttpProxyAgent(proxyOptions);
        var parsedUrl = parseUrl(fullUrl);
        var requestOptions: https.RequestOptions = {
            host: parsedUrl.host,
            path: parsedUrl.path,
            agent: agent,
            rejectUnauthorized: false
        };

        console.log("initiating call");
        http.request({ host: "www.google.com", agent: agent }, response => {
            console.log("response received");
            console.log(response);
        })
        // https.request(requestOptions, response => {
        //     console.log("response received");
        //     console.log(response);
        // });
    });

    context.subscriptions.push(disposable);
}

// this method is called when your extension is deactivated
export function deactivate() {
}