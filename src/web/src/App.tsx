import React from 'react';
import './App.css';

import { Routes, Route, BrowserRouter } from "react-router-dom"

import About from './pages/about';
import Setting from './pages/setting';
import Head from './components/head';
import Home from './pages/home';
import axios from 'axios'

axios.defaults.baseURL = "http://127.0.0.1:30099";
axios.defaults.timeout = 30000;

class App extends React.Component {

  render(): React.ReactNode {
    return (
      <BrowserRouter>
        <Head />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/*" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/setting" element={<Setting />} />

        </Routes>
      </BrowserRouter>
    )
  }
}

export default App;
