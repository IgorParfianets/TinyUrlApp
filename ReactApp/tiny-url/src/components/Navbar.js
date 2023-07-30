import {Link} from "react-router-dom";

export default function Navbar(){
    return (
        <div>
            <Link to="/">Home</Link>
            <Link to="/Login">Login</Link>
            <Link to="/Registration">Registration</Link>
        </div>
    );
}