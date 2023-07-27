import * as React from "react";
import { Routes, Route } from "react-router-dom";

import "./assets/Register.css";

import Layout from "./pages/Layout";
import Home from "./pages/Home";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Collections from "./pages/Collections";
import Missing from "./pages/Missing";
import RequireAuth from "./pages/RequireAuth";

function App() {
    return (
        <Routes>
            <Route path="/" element={<Layout />}>
                <Route path="/" element={<Home />} />
                <Route path="/login" element={<Login />} />
                <Route path="register" element={<Register />} />

                {/* We want to protect this route */}
                <Route element={<RequireAuth />}>
                    <Route path="collections" element={<Collections />} />
                </Route>

                {/* catch all */}
                <Route path="*" element={<Missing />} />
            </Route>
        </Routes>
    );
}

export default App;
