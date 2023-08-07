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
        const response = await instance.get(
            this._urlEndpont,
            {headers: {'Authorization': `Bearer ${accessToken}`,}}
        )
        return response.data.map(url => UrlDto.fromResponse(url))
    }

    async createShortUrl(data, accessToken) {
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
    }

    async removeShortUrl(alias, accessToken) {
        const fullUrlWithParameters = `${this._urlEndpont}/${alias}`
        const response = await instance.delete(
            fullUrlWithParameters,
            {headers: {'Authorization': `Bearer ${accessToken}`,}}
        )
        return response.status === 204
    }
}