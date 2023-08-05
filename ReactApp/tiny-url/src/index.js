import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import { createBrowserRouter, RouterProvider} from "react-router-dom";
import ErrorPage from "./pages/error.page";
import Home from "./pages/home.page";
import Login from "./pages/login.page";
import Registration from "./pages/registration.page";
import {AuthGuard} from "./guard/auth.guard";
import {Urls} from "./pages/urls.page";
import {Provider} from "react-redux";
import store from "./storage/store";

const router = createBrowserRouter([
    {
        path: "/",
        element: <App />,
        errorElement: <ErrorPage />,
        children: [
            {
                errorElement: <div>Something goes wrong.</div>,
                children: [
                    {
                        index: true,
                        element: <Home />
                    },
                    {
                        path: "/login",
                        element: <Login />,
                    },
                    {
                        path: "/registration",
                        element: <Registration />,
                    },
                    {
                        path: "/urls",
                        element: <AuthGuard component={Urls}/>
                    },
                ],
            },
        ],
    },
]);

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  //<React.StrictMode>
    <Provider store={store}>
        <RouterProvider router={router} />
    </Provider>
  //</React.StrictMode>
)
