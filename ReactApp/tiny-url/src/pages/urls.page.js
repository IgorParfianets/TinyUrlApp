import {useEffect, useState} from "react";
import UrlService from "../services/url.service";
import useToken from "../utils/hooks/useToken";
import UrlDto from "../models/dto/url.dto";
import {UrlElement} from "../components/url-element.component";
import UnauthorizedError from "../models/errors/unauthorized.error";
import AuthService from "../services/auth.service";
import BadRequestError from "../models/errors/badRequest.error";
import {useNavigate} from "react-router-dom";
import TokenDto from "../models/dto/token.dto";

const urlService = new UrlService()
const authService = new AuthService()

export function Urls() {
    const {token, setToken} = useToken()
    const [urls, setUrls] = useState([])
    const navigate = useNavigate()

    useEffect(() => {
        console.log("В useEffect зашли")
        async function fetchData() {
            console.log(`Наш новый токен ${token}`)
            let allUrls = await urlService.getAllUrls(token.accessToken);

            const result = allUrls.map(url => UrlDto.fromResponse(url));
            console.log(`получили урлы ${result}`)
            setUrls(result);
        }
        fetchData();
    },[token.accessToken])

    const handleRemoveUrl = async (alias) => {
        try {
            await urlService.removeShortUrl(alias, token.accessToken)
            const newUrls = urls.filter(url => url.alias !== alias)
            setUrls(newUrls)
        } catch (error) {
            if (error instanceof UnauthorizedError) {
                if (token.refreshToken) {
                    try {
                        const newToken = await authService.getTokenByRefreshToken(token.refreshToken)
                        if (newToken) {
                            setToken(newToken)
                            await urlService.removeShortUrl(alias, newToken.accessToken)
                        } else {
                            setToken(new TokenDto())
                            navigate('/login')
                        }
                    } catch (error) {
                        if (error instanceof BadRequestError) {
                            setToken(new TokenDto())
                            navigate('/login')
                        }
                    }
                } else {
                    setToken(new TokenDto())
                    navigate('/login')
                }
            }
        }
    }

    return (
        <div className="flex items-center justify-center bg-gray-100 p-8 bg-white rounded-lg">
            <ul>
                {urls.length > 0 ?
                    (urls.map(url =>
                            <UrlElement
                                key={url.alias}
                                originalUrl={url.originalUrl}
                                shortUrl={url.shortUrl}
                                alias={url.alias}
                                urlCreated={url.urlCreated}
                                handleRemoveUrl={handleRemoveUrl}
                            />)
                    ) : (
                        <div className="flex items-center justify-center h-screen">
                            <h1 className="text-4xl font-bold">You don't have any urls yet</h1>
                        </div>
                    )}

            </ul>
        </div>
    )
}

// const validateToken = async () => {
//     let isTokenValid;
//     try {
//         isTokenValid = await authService.validateToken(token.accessToken);
//     } catch (error) {
//         if (error instanceof UnauthorizedError) {
//             try{
//                 let newToken = await authService.getTokenByRefreshToken(token.refreshToken);
//                 if (newToken) {
//                     isTokenValid = await authService.validateToken(newToken.accessToken);
//                     setToken(newToken);
//                 }
//             }catch (error){
//                 if(error instanceof BadRequestError){
//                     navigate('/login')
//                 }
//                 console.error(error.message)
//             }
//         }
//     }
//     if (!isTokenValid) {
//         navigate("/login");
//     }else{
//         const isDeleted = await urlService.removeShortUrl(alias, token.accessToken)
//         if(isDeleted){
//             const newUrls = urls.filter(url => url.alias !== alias)
//             setUrls(newUrls)
//         }
//     }
// }
// validateToken()