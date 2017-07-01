import { ISqlService } from "./I-sql-service";

export class MemorySqlService implements ISqlService {
    connectionString: string;
    
    constructor(
        private server: string, public database: string,
        private instance = "", private driver?: string) {
    }
    
    public async getSql(sql: string): Promise<any[]> {
        return new Promise<any[]>((resolve, reject) => {
            var list = [{ name: "Temp" }];
            resolve(list);
        });
    }
}