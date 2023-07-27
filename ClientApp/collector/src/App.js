import * as React from "react";
import { Routes, Route } from "react-router-dom";

import './assets/Register.css';

import Home from "./pages/Home";
import About from "./pages/About";
import Register from './pages/Register'

function App() {
    return (
        <div className="App">
            <Register />
        </div>
    );
}

export default App;
