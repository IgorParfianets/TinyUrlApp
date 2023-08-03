import { NIL } from "uuid";

export default class TokenDto {
    accessToken = "";
    userId = NIL;
    tokenExpiration = null;
    refreshToken = NIL;

    constructor(accessToken, userId, tokenExpiration, refreshToken) {
        this.accessToken = accessToken;
        this.userId = userId;
        this.tokenExpiration = tokenExpiration;
        this.refreshToken = refreshToken;
    }

    static fromResponse(response) {
        return new TokenDto(
            response.accessToken,
            response.userId,
            response.tokenExpiration,
            response.refreshToken
        );
    }
}
