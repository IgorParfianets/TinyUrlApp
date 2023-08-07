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
        const response = await instance.post(
            this._tokenEndpoint.createToken, JSON.stringify(data),
        )
        return TokenDto.fromResponse(response.data)
    }

    async registration(data) {
        const response = await instance.post(
            this._userEndpoint, JSON.stringify(data)
        )
        return TokenDto.fromResponse(response.data)
    }

    async getTokenByRefreshToken(refreshToken) {
        try{
            const response = await instance.post(
                this._tokenEndpoint.refreshToken,
                {refreshToken: refreshToken},
            )
            return TokenDto.fromResponse(response.data)
        }catch(error){
            if(error.response.status === 401){
                throw new UnauthorizedError(error.response.message)
            } else if(error.response.status === 404){
                throw new BadRequestError(error.response.message)
            }
        }

    }

    async revokeRefreshToken(refreshToken) {
        await instance.post(this._tokenEndpoint.revokeToken, {
            refreshToken: refreshToken,
        });
    }

    async validateToken(accessToken) {
        try{
            return await instance.post(
                this._tokenEndpoint.validateToken,
                {},
                {headers: {'Authorization': `Bearer ${accessToken}`,}}
            );
        }catch(error){
            if(error.response.status === 401){
                throw new UnauthorizedError(error.response.message)
            }
        }

    }
}