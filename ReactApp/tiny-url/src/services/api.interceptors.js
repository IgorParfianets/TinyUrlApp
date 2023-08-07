import axios from "axios";
import {environment} from "../enviroment/enviroment";
import BadRequestError from "../models/errors/badRequest.error";
import UnauthorizedError from "../models/errors/unauthorized.error";
import ConflictError from "../models/errors/conflict.error";

export const instance = axios.create({
    baseURL: environment.apiUrl, headers: {'Content-Type': 'application/json'},
})

instance.interceptors.request.use(config => config, error => error)

instance.interceptors.response.use(config => config, error => {
    console.log("Ответ ошибка")
    const statusCode = error.response.status
    const errorMessage = error.response.message
    if (statusCode === 400) {
        //throw new BadRequestError(errorMessage)
        console.log("Ответ 400")
    } else if (statusCode === 401) {
        return Promise.reject(new UnauthorizedError(errorMessage));
        console.log("Ответ 401")
    } else if (statusCode === 409) {
        console.log("Ответ 409")
//throw new ConflictError(errorMessage)
    }else{
        return Promise.reject(error)
    }

})
