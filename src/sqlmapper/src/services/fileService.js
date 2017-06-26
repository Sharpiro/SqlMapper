"use strict";
exports.__esModule = true;
var fs = require("fs");
var FileService = (function () {
    function FileService() {
    }
    FileService.prototype.exists = function (filePath) {
        var promise = new Promise(function (resolve, reject) {
            fs.exists(filePath, function (doesExist) {
                resolve(doesExist);
            });
        });
        return promise;
    };
    FileService.prototype["delete"] = function (filePath) {
        var promise = new Promise(function (resolve, reject) {
            fs.unlink(filePath, function (err) {
                if (err)
                    reject(err);
                else
                    resolve();
            });
        });
        return promise;
    };
    FileService.prototype.create = function (filePath, fileData) {
        var promise = new Promise(function (resolve, reject) {
            fs.writeFile(filePath, fileData, function (err) {
                if (err)
                    reject(err);
                else
                    resolve();
            });
        });
        return promise;
    };
    return FileService;
}());
exports.FileService = FileService;
