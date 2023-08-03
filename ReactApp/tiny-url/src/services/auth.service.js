import {instance} from "./api.interceptors";
import Cookies from 'js-cookie'
import {saveTokensStorage} from "../helpers/auth.helper";
import {environment} from "../enviroment/enviroment";
import BadRequestError from "../models/errors/badRequest.error";
import TokenDto from "../models/dto/token.dto";
import ConflictError from "../models/errors/conflict.error";

export default class AuthService {
    constructor() {
        this._userEndpoint = environment.userEndpoint
        this._tokenEndpoint = environment.tokenEndpoints
    }
    async login(data){
        try{
            const response = await instance.post(
                this._tokenEndpoint.createToken, JSON.stringify(data)
            )
            const token = TokenDto.fromResponse(response.data)
            return token
        }catch (error){
            if(error instanceof BadRequestError){
                console.warn(error.message)
            }
        }
    }

    async registration(data){
        try{
            const response = await instance.post(
                this._userEndpoint, JSON.stringify(data)
            )

            if(response.data.accessToken)
                saveTokensStorage(response.data)

            const token = TokenDto.fromResponse(response.data)
            return token
        }catch (error){
            if(error instanceof BadRequestError){
                console.warn("Incorrect inputted data")
                console.warn(error.message)
            }
            else if(error instanceof ConflictError){
                console.warn("User already exists")
                console.warn(error.message)
            }
        }
    }

    async getTokenByRefreshToken(){
        const refreshToken = Cookies.get('refreshToken')

        const response = await instance.post(
            this._tokenEndpoint.refreshToken,
            {refreshToken : refreshToken}
            )

        if(response.data.accessToken)
            saveTokensStorage(response.data)

        const token = TokenDto.fromResponse(response.data)
        return token
    }
}