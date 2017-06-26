"use strict";
exports.__esModule = true;
var https = require("https");
var HttpsProxyAgent = require("https-proxy-agent");
var url_1 = require("url");
var HttpService = (function () {
    function HttpService() {
        this.proxyUrl = "http://proxy.wellsfargo.com:8080";
        this.proxyAgent = new HttpsProxyAgent(this.proxyUrl);
    }
    HttpService.prototype.get = function (url) {
        var parsedUrl = url_1.parse(url);
        var requestOptions = {
            host: parsedUrl.host,
            path: parsedUrl.path,
            agent: this.proxyAgent,
            rejectUnauthorized: false
        };
        var promise = new Promise(function (resolve, reject) {
            https.get(requestOptions, function (response) {
                response.setEncoding("utf8");
                response.on("data", function (chunk) {
                    resolve(chunk);
                });
            });
        });
        return promise;
    };
    return HttpService;
}());
exports.HttpService = HttpService;
