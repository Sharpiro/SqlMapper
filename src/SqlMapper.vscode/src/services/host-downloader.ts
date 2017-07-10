import { HttpService } from "./http-Service";

export class HostDownloader {
    constructor(private httpService: HttpService) { }

    public download(): Promise<void> {
        
        return new Promise<void>((resolve, reject) => {
            setTimeout(function () {
                resolve();
            }, 5000);
        });
    }
}