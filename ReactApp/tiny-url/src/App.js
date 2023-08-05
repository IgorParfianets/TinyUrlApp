import './App.css';
import Navbar from "./components/Navbar";
import {Outlet, Route, Routes} from "react-router-dom";

function App() {
  return (
      <>
          <Navbar/>
          <Outlet />
      </>
  );
}

export default App;
