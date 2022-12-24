import axios from 'axios'
import { MinIoDto } from '../module/minio';

class MinioConfigApi {
    put(option: MinIoDto) {
        return axios.put('/api/minio-config', option)
    }
    get() {
        return axios.get('/api/minio-config')
    }
}

export default new MinioConfigApi()