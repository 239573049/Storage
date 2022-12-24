import { Button, List, message } from "antd";
import { Component, ReactNode } from "react";
import serverApi from "../../apis/serverApi";

class Setting extends Component {

    state = {
        data: [
            {
                title: <Button type="text" block onClick={() => this.onaddWindonwServer()}>自动注册win服务</Button>,
                description: "自动将当前服务注册到window的服务当中，这样就可以开机自启了！"
            },]
    }

    onaddWindonwServer() {
        serverApi.windowServer()
            .then(res => {
                if (res.status === 200) {
                    message.success("注册成功")
                } else {
                    message.error("注册错误")
                }
            })
    }

    render(): ReactNode {
        var { data } = this.state
        return (<div>
            <List
                itemLayout="horizontal"
                dataSource={data}
                renderItem={(item) => (
                    <List.Item>
                        <List.Item.Meta
                            title={item.title}
                            description={item.description}
                        />
                    </List.Item>
                )}
            />
        </div>)
    }
}

export default Setting