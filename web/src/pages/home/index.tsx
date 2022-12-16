import { Component, ReactNode } from "react";
import { Tabs } from 'antd';
import Oss from "../../components/oss";
import MinIo from "../../components/minio";

class Home extends Component {

    render(): ReactNode {
        console.log(this.props);
        return (<div>
            <Tabs
                defaultActiveKey="1"
                items={[
                    {
                        label: `MinIo`,
                        key: 'minio',
                        children: <MinIo />,
                    }, {
                        label: `Oss`,
                        key: 'oss',
                        children: <Oss />
                    }
                ]}
            />
        </div>)
    }
}

export default Home