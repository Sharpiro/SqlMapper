'use strict';
import * as vscode from 'vscode';
import { MainController } from "./controllers/main-controller";
import { HostDownloader } from "./services/host-downloader";

let controller: MainController;

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export async function activate(context: vscode.ExtensionContext) {

    // Use the console to output diagnostic information (console.log) and errors (console.error)
    // This line of code will only be executed once when your extension is activated
    console.log('"sqlmapper" is activating...');
    controller = await MainController.create();
    var hostDownloader = new HostDownloader();

    const downloadTask = hostDownloader.download();
    // The command has been defined in the package.json file
    // Now provide the implementation of the command with registerCommand
    // The commandId parameter must match the command field in package.json
    let order66Disposable = vscode.commands.registerCommand('extension.order66', async () => {
        controller.executeOrder66();
    });

    let getInfoDisposable = vscode.commands.registerCommand('extension.getInfo', async () => {
        controller.getInfo();
    });

    let buildProfileDisposable = vscode.commands.registerCommand('extension.buildProfile', async () => {
        controller.buildProfile();
    });

    context.subscriptions.push(order66Disposable);
    context.subscriptions.push(getInfoDisposable);
    context.subscriptions.push(buildProfileDisposable);

    await downloadTask;

    console.log('"sqlmapper" is active!');
}

// this method is called when your extension is deactivated
export function deactivate() {

}