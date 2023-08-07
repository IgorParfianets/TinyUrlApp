import {useEffect} from "react";
import {useNavigate} from "react-router-dom";
import AuthService from "../services/auth.service";
import useToken from "../utils/hooks/useToken";
import UnauthorizedError from "../models/errors/unauthorized.error";
import BadRequestError from "../models/errors/badRequest.error";

const authService = new AuthService()

export function AuthGuard({component}){
    const { token, setToken } = useToken();
    const navigate = useNavigate();

    useEffect(() => {
        const validateToken = async () => {
            let isTokenValid;
            try {
                console.log("Валидация токена")
                isTokenValid = await authService.validateToken(token.accessToken);
            } catch (error) {
                if (error instanceof UnauthorizedError) {
                    try{
                        console.log("Словили UnauthorizedError")
                        let newToken = await authService.getTokenByRefreshToken(
                            token.refreshToken
                        );
                        if (newToken) {
                            console.log("Получили новый токен")
                            isTokenValid = await authService.validateToken(
                                newToken.accessToken
                            );
                            console.log(`Старый ${token}`)
                            console.log("Проверили валидность нового токена")
                            setToken(newToken);
                            console.log(`Новый ${newToken}`)
                        }
                    }catch (error){
                        if(error instanceof BadRequestError){
                            console.log("Словили BadRequest")
                            navigate("/login");
                            console.error(error.message)
                        }
                    }
                }
            }
            if (!isTokenValid) {
                console.log("Словили !isTokenValid")
                navigate("/login");
            }
        };

        if (token.accessToken) {
            validateToken(token);
        } else {
            console.log("Редирект")
            navigate("/login");
        }
    }, [token]);

    return component;
}