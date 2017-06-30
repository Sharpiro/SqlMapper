import * as assert from 'assert';

// You can import and use all API from the 'vscode' module
// as well as import your extension to test it
import * as vscode from 'vscode';
import * as myExtension from '../src/extension';
import { TrustedSqlService } from "../src/services/sql/trusted-sql-service"

// Defines a Mocha test suite to group tests of similar kind together
suite("SqlService Tests", () => {

    // Defines a Mocha unit test
    test("SqlServiceTest1", () => {
        const driver = 'msnodesqlv8';
        const server = '(localdb)';
        const instance = "mssqllocaldb";
        const database = 'Test';
        var sqlService = new TrustedSqlService(server, database, instance, driver);
        sqlService.getSql(null);
    });

});