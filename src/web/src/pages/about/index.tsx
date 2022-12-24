import { Component, ReactNode } from "react";
import style from './index.less'

class About extends Component {
    render(): ReactNode {
        return (<div>
            <div className={style.title}>关于</div>
            <div className={style.content}>本软件是完全免费并且开源的，开源协议基于MIT欢迎更多爱好者贡献！</div>
            <div className={style.content}>由<a target="_blank" href="https://github.com/239573049">token</a>起始的开源项目；</div>
        </div>)
    }
}

export default About