import {useState} from "react";
import {useDispatch, useSelector} from "react-redux";
import TokenDto from "../../models/dto/token.dto";
import {removeToken} from "../../storage/slices/token.slice";
import {setToken as setTokenToStore} from "../../storage/slices/token.slice";

export default function useToken() {
    const [token, setToken] = useState(
        useSelector(
            (state) =>
                new TokenDto(
                    state.token.accessToken,
                    state.token.userId,
                    state.token.tokenExpiration,
                    state.token.refreshToken
                )
        )
    );

    let tk = useSelector(
        (state) =>
            new TokenDto(
                state.token.accessToken,
                state.token.userId,
                state.token.tokenExpiration,
                state.token.refreshToken
            )
    );

    let isEqual = JSON.stringify(tk) === JSON.stringify(token);
    if (!isEqual) {
        setToken(tk);
    }

    const dispatch = useDispatch();
    const saveToken = (accessToken) => {
        let isEqual =
            JSON.stringify(accessToken) === JSON.stringify(new TokenDto());
        if (isEqual) {
            dispatch(removeToken(accessToken));
        } else {
            dispatch(setTokenToStore(accessToken));
        }

        setToken((token) => ({...token, ...accessToken}));
    };

    return {
        setToken: saveToken,
        token: token,
    };
}
