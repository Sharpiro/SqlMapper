'use strict';
import * as vscode from 'vscode';
import { MainController } from "./controllers/main-controller";

let controller: MainController;

// this method is called when your extension is activated
// your extension is activated the very first time the command is executed
export function activate(context: vscode.ExtensionContext) {

    // Use the console to output diagnostic information (console.log) and errors (console.error)
    // This line of code will only be executed once when your extension is activated
    console.log('Congratulations, your extension "sqlmapper" is now active!');
    controller = MainController.create();

    // The command has been defined in the package.json file
    // Now provide the implementation of the command with  registerCommand
    // The commandId parameter must match the command field in package.json
    let order66Disposable = vscode.commands.registerCommand('extension.order66', async () => {
        controller.executeOrder66();
    });

    let getInfoDisposable = vscode.commands.registerCommand('extension.getInfo', async () => {
        controller.getInfo();
    });

    context.subscriptions.push(order66Disposable);
}

// this method is called when your extension is deactivated
export function deactivate() {
}