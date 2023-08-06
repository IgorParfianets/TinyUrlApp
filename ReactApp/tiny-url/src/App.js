import './App.css';
import Navbar from "./components/navbar.component";
import {Outlet} from "react-router-dom";

function App() {
    return (
        <>
            <Navbar/>
            <Outlet/>
        </>
    );
}

export default App;
