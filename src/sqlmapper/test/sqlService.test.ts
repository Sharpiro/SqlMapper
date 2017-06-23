import * as assert from 'assert';

// You can import and use all API from the 'vscode' module
// as well as import your extension to test it
import * as vscode from 'vscode';
import * as myExtension from '../src/extension';
import { SqlService } from "../src/services/sqlService"

// Defines a Mocha test suite to group tests of similar kind together
suite("SqlService Tests", () => {

    // Defines a Mocha unit test
    test("SqlServiceTest1", () => {
        var service = new SqlService();
        service.get();
        
    });

});