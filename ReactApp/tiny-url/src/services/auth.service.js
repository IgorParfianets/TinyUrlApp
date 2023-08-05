import {instance} from "./api.interceptors";
import {environment} from "../enviroment/enviroment";
import BadRequestError from "../models/errors/badRequest.error";
import TokenDto from "../models/dto/token.dto";
import ConflictError from "../models/errors/conflict.error";
import UnauthorizedError from "../models/errors/unauthorized.error";

export default class AuthService {
    constructor() {
        this._userEndpoint = environment.userEndpoint
        this._tokenEndpoint = environment.tokenEndpoints
    }

    async login(data) {
        try {
            const response = await instance.post(
                this._tokenEndpoint.createToken, JSON.stringify(data),
            )
            // if (response.data.accessToken)
            //     saveTokenData(response.data)

            const token = TokenDto.fromResponse(response.data)
            return token
        } catch (error) {
            if (error instanceof BadRequestError) {
                console.warn(error.message)
            }
        }
    }

    async registration(data) {
        try {
            const response = await instance.post(
                this._userEndpoint, JSON.stringify(data)
            )

            // if (response.data.accessToken)
            //     saveTokenData(response.data)

            const token = TokenDto.fromResponse(response.data)
            return token
        } catch (error) {
            if (error instanceof BadRequestError) {
                console.warn("Incorrect inputted data")
                console.warn(error.message)
            } else if (error instanceof ConflictError) {
                console.warn("User already exists")
                console.warn(error.message)
            }
        }
    }

    async getTokenByRefreshToken(refreshToken) {
        try {
            const response = await instance.post(
                this._tokenEndpoint.refreshToken,
                {refreshToken: refreshToken},
            )
            const token = TokenDto.fromResponse(response.data)
            return token
        } catch (error) {

        }
    }

    async revokeRefreshToken(refreshToken) {
        await instance.post(this._tokenEndpoint.revokeToken, {
            refreshToken: refreshToken,
        });
    }

    async validateToken(accessToken) {
        try {
            let response = await instance.post(
                this._tokenEndpoint.validateToken,
                {},
                {headers: {'Authorization': `Bearer ${accessToken}`,}}
            );
            if (response)
                return true;
        } catch (error) {
            if (error instanceof UnauthorizedError) {
                throw error;
            }
        }
    }
}