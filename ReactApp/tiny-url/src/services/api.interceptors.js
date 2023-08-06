import axios from "axios";
import {environment} from "../enviroment/enviroment";
import BadRequestError from "../models/errors/badRequest.error";
import UnauthorizedError from "../models/errors/unauthorized.error";
import ConflictError from "../models/errors/conflict.error";

export const instance = axios.create({
    baseURL: environment.apiUrl, headers: {'Content-Type': 'application/json'},
})

instance.interceptors.request.use(config => config)

instance.interceptors.response.use(config => config, error => {
    const status = error.response.status

    if (status === 400) {
        throw new BadRequestError(error.message)
    } else if (status === 401) {
        throw new UnauthorizedError(error.message)
    } else if (status === 409) {
        throw new ConflictError(error.message)
    }
    return Promise.reject(error);
})
