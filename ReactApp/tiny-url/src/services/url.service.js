import {environment} from "../enviroment/enviroment";
import {instance} from "./api.interceptors";
import TokenDto from "../models/dto/token.dto";
import BadRequestError from "../models/errors/badRequest.error";
import UrlDto from "../models/dto/url.dto";
import ConflictError from "../models/errors/conflict.error";

export default class UrlService{
    constructor() {
        this._urlEndpont = environment.urlEndpoint
        this._tokenEndpoint = environment.tokenEndpoints
    }

    async createShortUrl(data){
        try{
            const response = await instance.post(
                this._urlEndpont, JSON.stringify(data)
            )
            const url = UrlDto.fromResponse(response.data)
            return url

        }catch (error){
            if(error instanceof BadRequestError){
                console.warn(error.message)
            } else if (error instanceof ConflictError){
                console.warn(error.message)
            }
        }
    }
    async removeShortUrl(alias){
        try{
            const response = await instance.delete(
                this._urlEndpont, alias
            )
            if(response.status === 204){
                console.log('nice')
            }
        }catch (error){
            if(error instanceof BadRequestError){
                console.warn(error.message)
            }
        }
    }
}