import {Link} from "react-router-dom";
import useToken from "../utils/hooks/useToken";
import AuthService from "../services/auth.service";
import {useEffect, useState} from "react";

const authService = new AuthService()

export default function Navbar(){
    const {token, setToken} = useToken()
    const [isAuthorized, setIsAuthorized] = useState(false)

    useEffect(() => {
        const validateToken = async () => {
            let isTokenValid =  await authService.validateToken(token.accessToken);
            if(isTokenValid)
                setIsAuthorized(true)
        };
        validateToken()
    }, [token]);

    const handleLogout = () => {
        authService.revokeRefreshToken()
        setToken(null)
        setIsAuthorized(false)
    }

    if(isAuthorized){
        return (
            <div>
                <Link to="/">Home</Link>
                <button onClick={handleLogout}>Logout</button>
            </div>
        )
    }

    return (
        <div>
            <Link to="/">Home</Link>
            <Link to="/login">LoginPage</Link>
            <Link to="/registration">RegistrationPage</Link>
        </div>
    );
}