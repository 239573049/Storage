import { Component, ReactNode } from 'react'
import style from './index.less'
import { Link } from 'react-router-dom';
import {
    GithubOutlined
} from '@ant-design/icons';

class Head extends Component {
    render(): ReactNode {
        return (<div className={style.menu}>
            <a href='https://github.com/239573049/Storage'><div><GithubOutlined /></div></a>
            <Link to={'/setting'}><div>系统设置</div></Link>
            <Link to={'/about'}><div>关于</div></Link>
            <Link to={'/'}><div >首页</div></Link>
        </div>)
    }
}

export default Head