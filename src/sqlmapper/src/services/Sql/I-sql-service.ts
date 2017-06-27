export interface ISqlService {
    getSql(sql: string): Promise<any[]>;
}