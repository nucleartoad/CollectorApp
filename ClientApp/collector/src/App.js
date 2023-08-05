import * as React from "react";
import { useState, useMemo } from 'react';
import { BrowserRouter, Routes, Route } from "react-router-dom";

import "./assets/Register.css";

import Home from "./pages/Home";
import Register from "./pages/Register";
import Login from "./pages/Login";
import Logout from "./pages/Logout";
import Collection from "./pages/Collection";
import Collections from "./pages/Collections";
import Item from "./pages/Item";
import AddItem from "./pages/AddItem";
import CreateCollection from "./pages/CreateCollection";
import Missing from "./pages/Missing";
import ProtectedRoute from './util/ProtectedRoute';

function App() {
    return (
        <Routes>
            <Route path="login" element={<Login />} />
            <Route path="register" element={<Register />} />
            <Route path="logout" element={<Logout />} />

            <Route path="/" element={
                <ProtectedRoute>
                    <Home />
                </ProtectedRoute>
            } />

            <Route path="collections" element={
                <ProtectedRoute>
                    <Collections />
                </ProtectedRoute>
            } />

            <Route path="collections/:collectionId" element={
                <ProtectedRoute>
                    <Collection />
                </ProtectedRoute>
            } />

            <Route path="collections/create-collection" element={
                <ProtectedRoute>
                    <CreateCollection />
                </ProtectedRoute>
            } />

            <Route path="collections/:collectionId/add-item" element={
                <ProtectedRoute>
                    <AddItem />
                </ProtectedRoute>
            } />

            <Route path="collections/:collectionId/:itemId" element={
                <ProtectedRoute>
                    <Item />
                </ProtectedRoute>
            } />

            <Route path="*" element={<Missing />} />
        </Routes>
    );
}

export default App;
