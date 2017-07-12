import { HttpService } from "./http-Service";
import { FileService } from "./fileService";
import * as fs from 'fs';

var yauzl = require("yauzl");

export class HostDownloader {
    constructor(private httpService: HttpService, private fileService: FileService) { }

    private tempData = "";

    public async download(): Promise<void> {

        var x: any = await this.httpService.get("https://sharpirostorage.blob.core.windows.net/test/SqlMapper.Host-0.1.0.zip");
        var y = x.length;
        var z = <Buffer>x;
        const zipFilePath = "C:\\Users\\U403598\\Desktop\\temp\\zipping\\testBufferedFile.zip"
        await this.fileService.create(zipFilePath, z);
        await this.openZip(zipFilePath);
    }

    private async openZip(zipFilePath: string) {
        yauzl.open(zipFilePath, { lazyEntries: false }, function (err, zipfile) {
            if (err) throw err;
            zipfile.readEntry();
            zipfile.on("entry", function (entry) {
                if (/\/$/.test(entry.fileName)) {
                    // Directory file names end with '/'.
                    // Note that entires for directories themselves are optional.
                    // An entry's fileName implicitly requires its parent directories to exist.
                } else {
                    // file entry
                    zipfile.openReadStream(entry, function (err, readStream) {
                        if (err) throw err;
                        readStream.on("end", function () {
                            zipfile.readEntry();
                        });
                        console.log(entry);
                        // readStream.pipe(fs.createWriteStream(`C:\\Users\\U403598\\Desktop\\temp\\zipping\\${entry.fileName}`));
                    });
                }
            });
        });
    }
}

export class Task {
    private constructor() { }
    public static delay(seconds: number): Promise<void> {
        return new Promise<void>((resolve, reject) => {
            setTimeout(function () {
                resolve();
            }, seconds * 1000);
        });
    }
}