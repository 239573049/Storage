import { Component, ReactNode } from "react";
import { Card, Form, Input, Button, InputNumber, Switch } from 'antd'
import { MinIoDto } from "../../module/minio";

interface IProps {

}
interface IState {
    data: MinIoDto
}

class MinIo extends Component<IProps, IState> {
    state: Readonly<IState> = {
        data: {
            accessKey: '',
            secretKey: '',
            bucketName: '',
            endpoint: '',
            port: 8001,
            volumeLabel: '',
            startDefault: true,
            mountPoint: ''
        }
    }
    onFinish(values: any) {
        console.log('Finish:', values);
    };
    onChange(key: string, value: any) {
    }
    render(): ReactNode {
        var { data } = this.state
        return (
            <Card title="MinIo配置" style={{ width: '100%', height: '60%' }}>
                <Form
                    name="basic"
                    labelCol={{ span: 8 }}
                    wrapperCol={{ span: 16 }}
                    initialValues={{ remember: true }}
                    autoComplete="off"
                    onFinish={(values) => this.onFinish(values)}>
                    <Form.Item
                        name="accessKey"
                        label="accessKey"
                        rules={[{ required: true, message: 'Please input your username!' }]}
                    >
                        <Input value={data.accessKey} placeholder="accessKey" onChange={(e) => this.onChange('accessKey', e.target.value)} />
                    </Form.Item>
                    <Form.Item
                        name="secretKey"
                        label="secretKey"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input
                            value={data.secretKey}
                            onChange={(e) => this.onChange('secretKey', e.target.value)}
                            placeholder="secretKey"
                        />
                    </Form.Item>
                    <Form.Item
                        name="bucketName"
                        label="bucketName"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input
                            value={data.bucketName}
                            onChange={(e) => this.onChange('bucketName', e.target.value)}
                            placeholder="bucketName"
                        />
                    </Form.Item>
                    <Form.Item
                        name="bucketName"
                        label="bucketName"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input
                            onChange={(e) => this.onChange('bucketName', e.target.value)}
                            value={data.endpoint}
                            placeholder="endpoint"
                        />
                    </Form.Item>
                    <Form.Item
                        name="port"
                        label="port"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <InputNumber min={0} max={65535} value={data.port} defaultValue={8001}
                            onChange={(e) => this.onChange('port', e)} />
                    </Form.Item>

                    <Form.Item
                        name="volumeLabel"
                        label="volumeLabel"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input
                            value={data.volumeLabel}
                            onChange={(e) => this.onChange('volumeLabel', e)}
                            placeholder="volumeLabel"
                        />
                    </Form.Item>
                    <Form.Item
                        name="mountPoint"
                        label="mountPoint"
                        rules={[{ required: true, message: 'Please input your password!' }]}
                    >
                        <Input
                            value={data.mountPoint}
                            onChange={(e) => this.onChange('mountPoint', e)}
                            placeholder="mountPoint"
                        />
                    </Form.Item>
                    <Form.Item
                        name="startDefault"
                        label="自启动"
                        rules={[{ required: true, message: 'Please input your startDefault!' }]}
                    >
                        <Switch checked={data.startDefault} onChange={(e) => this.onChange('startDefault', e)} />
                    </Form.Item>
                    <Button
                        type="primary"
                        block
                    >
                        更新
                    </Button>
                </Form>
            </Card>)
    }
}

export default MinIo