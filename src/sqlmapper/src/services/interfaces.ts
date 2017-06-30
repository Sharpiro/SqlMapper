import { RequestOptions, IncomingMessage, ClientRequest } from "http";

export interface INodeHttp {
    get(options: RequestOptions, callback?: (res: IncomingMessage) => void): ClientRequest;
}