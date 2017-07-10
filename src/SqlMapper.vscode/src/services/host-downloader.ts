import { HttpService } from "./http-Service";
import { FileService } from "./fileService";

export class HostDownloader {
    constructor(private httpService: HttpService, private fileService: FileService) { }

    public async download(): Promise<void> {

        var x: any = await this.httpService.get("https://sharpirostorage.blob.core.windows.net/test/SqlMapper.Host-0.1.0.zip");
        var y = x.length;
        var z = <Buffer>x;
        await this.fileService.createBuffer("c:\\users\\u403598\\desktop\\temp\\testBufferedFile.zip", z);
        // return new Promise<void>((resolve, reject) => {
        //     setTimeout(function () {
        //         resolve();
        //     }, 5000);
        // });
    }
}