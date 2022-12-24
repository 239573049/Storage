import { Component, ReactNode } from "react";
import { Card, Input, Button, InputNumber, Switch, Collapse, message, Tag } from "antd";
import { MinIoDto } from "../../module/minio";
import MinioConfigApi from "../../apis/minioConfigApi";
import style from './index.less'
import serverApi from "../../apis/serverApi";
import { StorageDokan } from "../../module/storage";

const { Panel } = Collapse;
interface IProps { }
interface IState {
    data: MinIoDto;
    serverState: boolean,
    serverButtonLoad: boolean
}

class MinIo extends Component<IProps, IState> {
    state: Readonly<IState> = {
        data: {
            accessKey: "",
            secretKey: "",
            bucketName: "",
            endpoint: "",
            port: 8001,
            volumeLabel: "",
            startDefault: true,
            mountPoint: "",
        },
        serverState: false,
        serverButtonLoad: false
    };
    onFinish() {
        var { data } = this.state;
        console.log("Finish:", data);
        MinioConfigApi.put(data)
    }
    onChange(key: string, value: any) {
        var { data } = this.state;
        data[key] = value;
        this.setState({
            data,
        });
    }

    getState() {
        serverApi.get(StorageDokan.MinIo)
            .then(res => {
                this.setState({
                    serverState: res.data
                })
            })
    }

    onGet() {
        MinioConfigApi.get()
            .then((res) => {
                var { data } = this.state;
                var value = res.data as MinIoDto;
                data = value;
                console.log(data);

                this.setState({
                    data: data,
                });
            });
    }

    constructor(props: any) {
        super(props);
        this.onGet();
        this.getState();
    }

    start() {
        this.setState({
            serverButtonLoad: true
        })
        serverApi.start(StorageDokan.MinIo)
            .then(res => {
                if (res.status === 200) {
                    message.success("启动成功")
                    this.setState({
                        serverState: true
                    })
                } else {
                    message.error("启动错误")
                }

                this.setState({
                    serverButtonLoad: false
                })
            })
    }

    stop() {
        this.setState({
            serverButtonLoad: true
        })
        serverApi.stop(StorageDokan.MinIo)
            .then(res => {
                if (res.status === 200) {
                    message.success("关闭成功")
                    this.setState({
                        serverState: false
                    })
                } else {
                    message.error("关闭错误")
                }

                this.setState({
                    serverButtonLoad: false
                })
            })
    }

    render(): ReactNode {
        var { data, serverState, serverButtonLoad } = this.state;

        return (
            <Card title="MinIo" style={{ width: "100%", height: "60%" }}>
                <Collapse>
                    <Panel header="Minio配置" key="1">
                        <div className={style.minio}>

                            <span>accessKey:</span>
                            <Input
                                value={data.accessKey}
                                className={style.input}
                                placeholder="accessKey"
                                onChange={(e) => this.onChange("accessKey", e.target.value)}
                            />
                        </div>
                        <div className={style.minio}>

                            <span> bucketName:</span>
                            <Input
                                value={data.bucketName}
                                className={style.input}
                                onChange={(e) => this.onChange("bucketName", e.target.value)}
                                placeholder="bucketName"
                            />
                        </div>
                        <div className={style.minio}>

                            <span> secretKey:</span>
                            <Input
                                value={data.secretKey}
                                className={style.input}
                                onChange={(e) => this.onChange("secretKey", e.target.value)}
                                placeholder="secretKey"
                            />
                        </div>
                        <div className={style.minio}>

                            <span> endpoint:</span>
                            <Input
                                value={data.endpoint}
                                className={style.input}
                                onChange={(e) => this.onChange("endpoint", e.target.value)}
                                placeholder="endpoint"
                            />
                        </div>
                        <div className={style.minio}>

                            <span> port:</span>
                            <InputNumber
                                min={0}
                                max={65535}
                                value={data.port}
                                defaultValue={8001}
                                className={style.input}
                                onChange={(e) => this.onChange("port", e)}
                            />
                        </div>
                        <div className={style.minio}>

                            <span> volumeLabel:</span>
                            <Input
                                value={data.volumeLabel}
                                className={style.input}
                                onChange={(e) => this.onChange("volumeLabel", e.target.value)}
                                placeholder="volumeLabel"
                            />
                        </div>
                        <div className={style.minio}>

                            <span> mountPoint:</span>
                            <Input
                                value={data.mountPoint}
                                className={style.input}
                                onChange={(e) => this.onChange("mountPoint", e.target.value)}
                                placeholder="mountPoint"
                            />
                        </div>
                        <div className={style.minio}>

                            <span> startDefault:</span>
                            <Switch
                                checked={data.startDefault}
                                onChange={(e) => this.onChange("startDefault", e)}
                            />
                        </div>
                        <div>
                            <Button type="primary" block onClick={() => this.onFinish()}>
                                更新
                            </Button>
                        </div>
                    </Panel>
                    <Panel header="服务状态" key="2">
                        {serverState ? <Tag color="success">服务运行中</Tag> :
                            <Tag color="warning">服务未运行</Tag>}
                    </Panel>
                    <Panel header="操作" key="3">
                        {serverState ? <Button type="dashed" loading={serverButtonLoad} onClick={() => this.stop()}>关闭服务</Button> :
                            <Button type="primary" loading={serverButtonLoad} onClick={() => this.start()}>启动服务</Button>}
                    </Panel>
                </Collapse>

            </Card>
        );
    }
}

export default MinIo;
