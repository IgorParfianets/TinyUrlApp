import './App.css';
import Navbar from "./components/Navbar";
import {Route, Routes} from "react-router-dom";
import Registration from "./pages/Registration";
import Login from "./pages/Login";
import Home from "./pages/Home";

function App() {
  return (
      <>
        <Navbar/>
        <Routes>
          <Route path="/Registration" element={<Registration/>}/>
          <Route path="/Login" element={<Login/>}/>
          <Route path="/" element={<Home/>}/>
          <Route path="*" element={<Home/>}/>
        </Routes>
      </>
  );
}

export default App;
