import { baseDokandto } from "./baseDokandto";

export interface MinIoDto extends baseDokandto {
    accessKey: string;
    secretKey: string;
    bucketName: string;
    endpoint: string;
    port: number;
    volumeLabel: string;
}