import {useEffect} from "react";
import {useNavigate} from "react-router-dom";
import AuthService from "../services/auth.service";
import useToken from "../utils/hooks/useToken";
import UnauthorizedError from "../models/errors/unauthorized.error";

const authService = new AuthService()

export function AuthGuard({component}){
    const { token, setToken } = useToken();
    const navigate = useNavigate();

    useEffect(() => {
        const validateToken = async () => {
            let isTokenValid;
            try {
                isTokenValid = await authService.validateToken(token.accessToken);
            } catch (error) {
                if (error instanceof UnauthorizedError) {
                    let newToken = await authService.getTokenByRefreshToken(
                        token.refreshToken
                    );
                    if (newToken) {
                        isTokenValid = await authService.validateToken(
                            newToken.accessToken
                        );
                        setToken(newToken);
                    }
                }
            }

            if (!isTokenValid) {
                navigate("/login");
            }
        };

        if (token.accessToken) {
            validateToken(token);
        } else {
            navigate("/login");
        }
    }, [token]);

    return component;
}