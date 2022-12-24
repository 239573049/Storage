export interface StorageDokanInstance {
    startMinio: boolean;
    storageDokan: StorageDokan;
    createdTime: string;
}

export enum StorageDokan {
    MinIo,
    Oss
}