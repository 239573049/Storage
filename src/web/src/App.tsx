import React from 'react';
import './App.css';

import { Routes, Route, BrowserRouter } from "react-router-dom"

import About from './pages/about';
import Setting from './pages/setting';
import Head from './components/head';
import Home from './pages/home';

class App extends React.Component {

  render(): React.ReactNode {
    return (
      <BrowserRouter>
        <Head />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/about" element={<About />} />
          <Route path="/setting" element={<Setting />} />
        </Routes>
      </BrowserRouter>
    )
  }
}

export default App;
