import * as React from "react";
import { useState, useMemo } from 'react';
import { Routes, Route } from "react-router-dom";
import { userContext } from "./context/userContext";

import "./assets/Register.css";

import Home from "./pages/Home";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Collections from "./pages/Collections";
import Missing from "./pages/Missing";

function App() {
    const [user, setUser] = useState(null);
    const providerValue = useMemo(() =>  ({user, setUser}), [user, setUser]);

    return (
        <userContext.Provider value={providerValue}>
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />

                {/* We want to protect this route */}
                <Route path="collections" element={<Collections />} />

                {/* catch all */}
                <Route path="*" element={<Missing />} />
            </Routes>
        </userContext.Provider>
    );
}

export default App;
