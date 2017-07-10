import * as https from 'https';
import * as http from 'http';
import HttpsProxyAgent = require('https-proxy-agent');
import HttpProxyAgent = require('http-proxy-agent');
import { parse as parseUrl, Url } from 'url';
import { INodeHttp } from "./interfaces"

export class HttpService {
    private httpsProxyAgent: HttpsProxyAgent;
    private httpProxyAgent: HttpProxyAgent;
    private _proxyUrl: string;
    private httpTimeout = 10;

    public get proxyUrl(): string {
        return this._proxyUrl;
    }

    public set proxyUrl(value: string) {
        this._proxyUrl = value;
        if (!value) return;
        this.httpsProxyAgent = new HttpsProxyAgent(value);
        this.httpProxyAgent = new HttpProxyAgent(value);
    }

    constructor(proxyUrl?: string) {
        this.proxyUrl = proxyUrl;
    }

    public get(url: string): Promise<string> {
        const parsedUrl: Url = parseUrl(url);
        const isHttps = parsedUrl.protocol === "https:";
        const isLocal = parsedUrl.hostname.includes("localhost");
        const proxy = !this.proxyUrl ? null : isLocal ? null : isHttps ? this.httpsProxyAgent : this.httpProxyAgent;
        const requestOptions: https.RequestOptions = {
            host: parsedUrl.hostname,
            path: parsedUrl.path,
            port: parsedUrl.port ? Number(parsedUrl.port) : null,
            agent: proxy,
            rejectUnauthorized: false
        };

        const nodeHttp = isHttps ? https : http;
        let promise = this.initiateCall(nodeHttp, requestOptions);
        return promise;
    }

    private initiateCall(nodeHttp: INodeHttp, requestOptions: http.RequestOptions) {
        let localPromise = new Promise<string | Buffer>((resolve, reject) => {
            let isTimedOut = false;
            setTimeout(() => {
                isTimedOut = true;
                reject(new Error("Connection timed out"));
                return;
            }, this.httpTimeout * 1000);
            nodeHttp.get(requestOptions, response => handleData(response, resolve, reject, isTimedOut));
        });

        const handleData = (response: http.IncomingMessage, resolve: (value: string | Buffer) => void, reject: () => void, isTimedOut: boolean) => {
            if (isTimedOut) return;
            if (response.statusCode !== 200 && response.statusCode !== 201) reject();
            var allBuffer: Buffer = new Buffer(0);
            let allData = "";
            // response.setEncoding("utf8");
            response.on("data", chunk => {
                if (typeof chunk === "string")
                    allData += chunk;
                else
                    allBuffer = Buffer.concat([allBuffer, chunk]);
            });

            response.on("end", () => {
                allData !== "" ? resolve(allData) : resolve(allBuffer);
            })
        }

        return localPromise;

    }

    // private GetData(buffer: Buffer) {
    //     var temp = new Uint8Array(buffer)..buffer;

    //     temp.slice()

    //     var arrayBuffer = new ArrayBuffer(buffer.length);
    //     var byteArray = new Uint8Array(arrayBuffer);

    //     for (var i = 0; i < buffer.length; i++) {
    //         byteArray[i] = buffer[i];
    //     }
    // }
}