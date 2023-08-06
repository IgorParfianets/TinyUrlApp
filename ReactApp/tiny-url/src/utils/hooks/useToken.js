import {useMemo} from "react";
import {useDispatch, useSelector} from "react-redux";
import TokenDto from "../../models/dto/token.dto";
import {removeToken} from "../../storage/slices/token.slice";
import {setToken} from "../../storage/slices/token.slice";

export default function useToken() {
    const dispatch = useDispatch();
    const tokenData = useSelector(state => state.token);

    const token = useMemo(
        () => ({
            accessToken: tokenData.accessToken,
            userId: tokenData.userId,
            tokenExpiration: tokenData.tokenExpiration,
            refreshToken: tokenData.refreshToken
        }),
        [tokenData]
    );

    const saveToken = (accessToken) => {
        let isEqual = JSON.stringify(accessToken) === JSON.stringify(new TokenDto());

        if (isEqual) {
            dispatch(removeToken());
        } else {
            dispatch(setToken({
                accessToken: accessToken.accessToken,
                userId: accessToken.userId,
                tokenExpiration: accessToken.tokenExpiration,
                refreshToken: accessToken.refreshToken
            }));
        }
    };

    return {
        setToken: saveToken,
        token: token,
    };
}
