import axios from 'axios'
import { StorageDokan } from '../module/storage'

class ServerApi {
    getlist() {
        return axios.get("/api/server/list")
    }

    get(dokan: StorageDokan) {
        return axios.get("/api/server?dokan=" + dokan)
    }

    start(dokan: StorageDokan) {
        return axios.post('/api/server/start?dokan=' + dokan)
    }

    stop(dokan: StorageDokan) {
        return axios.post('/api/server/stop?dokan=' + dokan)
    }

    windowServer() {
        return axios.post('/api/server/window-server')
    }
}

export default new ServerApi()