import * as fs from 'fs';

export class FileService {
    public exists(filePath: string): Promise<boolean> {
        var promise = new Promise<boolean>((resolve, reject) => {
            fs.exists(filePath, doesExist => {
                resolve(doesExist);
            })
        });
        return promise;
    }

    public delete(filePath: string): Promise<void> {
        var promise = new Promise<void>((resolve, reject) => {
            fs.unlink(filePath, err => {
                if (err) reject(err);
                else resolve();
            })
        });
        return promise;
    }

    public create(filePath: string, fileData: string): Promise<void> {
        var promise = new Promise<void>((resolve, reject) => {
            fs.writeFile(filePath, fileData, err => {
                if (err) reject(err);
                else resolve();
            });
        });
        return promise;
    }
}