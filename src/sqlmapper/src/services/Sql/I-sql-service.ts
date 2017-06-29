export interface ISqlService {
    readonly connectionString: string;
    database: string;
    getSql(sql: string): Promise<any[]>;
}