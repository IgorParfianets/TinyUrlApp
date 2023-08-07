import {Link} from "react-router-dom";
import useToken from "../utils/hooks/useToken";
import AuthService from "../services/auth.service";
import TokenDto from "../models/dto/token.dto";

const authService = new AuthService()

// const authorizedSettings = ["My URLs", "Logout"];
// const nonAuthorizedSettings = ["Login", "Register"];
export default function Navbar() {
    const {token, setToken} = useToken()

    const handleLogout = async () => {
        await authService.revokeRefreshToken(token.refreshToken)
        setToken(new TokenDto())
    }

    return (
        <nav className="bg-sky-200 min-h-[3rem]">
            <div className="container mx-auto px-4">
                <ul className="flex items-center ">
                    <li className="inline p-3 hover:bg-amber-300"><Link to="/">Home</Link></li>
                    {token.accessToken !== null ?
                        <>
                            <li className="inline p-3 hover:bg-amber-300"><Link to="/urls">My URLs</Link></li>
                            <li className="inline p-3 hover:bg-amber-300">
                                <button onClick={handleLogout}>Logout</button>
                            </li>
                        </>
                        :
                        <>
                            <li className="inline p-3 hover:bg-amber-300"><Link to="/login">Login</Link></li>
                            <li className="inline p-3 hover:bg-amber-300"><Link to="/registration">Registration</Link>
                            </li>
                        </>
                    }
                </ul>
            </div>
        </nav>
    )
}