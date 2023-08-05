import * as React from "react";
import { StrictMode } from "react";
import { createRoot } from "react-dom/client";

import App from "./App";
import { BrowserRouter, Link } from "react-router-dom";

const rootElement = document.getElementById("root");
const root = createRoot(rootElement);

root.render(
    <StrictMode>
      <BrowserRouter basename={'/'}>
        <App />
      </BrowserRouter>
    </StrictMode>
);
