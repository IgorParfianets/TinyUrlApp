import {environment} from "../enviroment/enviroment";
import {instance} from "./api.interceptors";
import BadRequestError from "../models/errors/badRequest.error";
import UrlDto from "../models/dto/url.dto";
import ConflictError from "../models/errors/conflict.error";
import UnauthorizedError from "../models/errors/unauthorized.error";

export default class UrlService {
    constructor() {
        this._urlEndpont = environment.urlEndpoint
    }

    async getAllUrls(accessToken) {
        try {
            const response = await instance.get(
                this._urlEndpont,
                {headers: {'Authorization': `Bearer ${accessToken}`,}}
            )
            return response.data.map(url => UrlDto.fromResponse(url))
        } catch (error) {
            if (error instanceof UnauthorizedError) {
                console.error(error.message)
            }
        }
    }

    async createShortUrl(data, accessToken) {
        try {
            let response;
            if (accessToken) {
                response = await instance.post(
                    this._urlEndpont, JSON.stringify(data),
                    {headers: {'Authorization': `Bearer ${accessToken}`,}}
                )
            } else {
                response = await instance.post(
                    this._urlEndpont, JSON.stringify(data)
                )
            }

            return UrlDto.fromResponse(response.data)

        } catch (error) {
            if (error instanceof BadRequestError) {
                console.warn(error.message)
            } else if (error instanceof ConflictError) {
                console.warn(error.message)
            }
        }
    }

    async removeShortUrl(alias, accessToken) {
        try {
            const fullUrlWithParameters = `${this._urlEndpont}/${alias}`
            await instance.delete(
                fullUrlWithParameters,
                {headers: {'Authorization': `Bearer ${accessToken}`,}}
            )
        } catch (error) {
            if (error instanceof BadRequestError) {
                console.warn(error.message)
            }
        }
    }
}