import {environment} from "../enviroment/enviroment";
import {instance} from "./api.interceptors";
import BadRequestError from "../models/errors/badRequest.error";
import UrlDto from "../models/dto/url.dto";
import ConflictError from "../models/errors/conflict.error";
import UnauthorizedError from "../models/errors/unauthorized.error";

export default class UrlService{
    constructor() {
        this._urlEndpont = environment.urlEndpoint
    }

    async getAllUrls(){
        try{
            const response = await instance.get(
                this._urlEndpont
            )
            const urls = response.data.map(url => UrlDto.fromResponse(url))
            return urls
        }catch (error){
            if(error instanceof UnauthorizedError){
                console.error(error.message)
            }
        }
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
            const fullUrlWithParameters = `${this._urlEndpont}/${alias}`
            const response = await instance.delete(
                fullUrlWithParameters
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