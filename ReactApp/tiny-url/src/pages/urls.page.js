import UrlService from "../services/url.service";
import {useEffect, useState} from "react";
import useToken from "../utils/hooks/useToken";
import UrlDto from "../models/dto/url.dto";
import {UrlElement} from "../components/url-element.component";
import UnauthorizedError from "../models/errors/unauthorized.error";
import AuthService from "../services/auth.service";

const urlService = new UrlService()
const authService = new AuthService()

export function Urls() {
    const {token, setToken} = useToken()
    const [urls, setUrls] = useState([])

    useEffect(() => {
        async function fetchData() {
            let allUrls = await urlService.getAllUrls(token.accessToken);
            const result = allUrls.map(url => UrlDto.fromResponse(url));
            setUrls(result);
        }
        fetchData();
    })

    const handleRemoveUrl = async (alias) => {
        try {
            await urlService.removeShortUrl(alias, token.accessToken)
            const newUrls = urls.filter(url => url.alias !== alias)
            setUrls(newUrls)
        } catch (error) {
            if (error instanceof UnauthorizedError) {
                if (token.refreshToken != null) {
                    const newToken = await authService.getTokenByRefreshToken(token.refreshToken)

                    if (Object.keys(newToken).length > 0) {
                        setToken(newToken)
                        await handleRemoveUrl(alias)
                    } else {
                        throw error
                    }
                }
            }
        }
    }

    return (
        <div>
            <ul>
                {urls.map(url =>
                    <UrlElement
                        key={url.alias}
                        originalUrl={url.originalUrl}
                        shortUrl={url.shortUrl}
                        alias={url.alias}
                        urlCreated={url.urlCreated}
                        handleRemoveUrl={handleRemoveUrl}
                    />)}
            </ul>
        </div>
    )
}