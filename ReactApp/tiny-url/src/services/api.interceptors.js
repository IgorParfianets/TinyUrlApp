import axios from "axios";
import {getAccessToken} from "../helpers/auth.helper";
import {environment} from "../enviroment/enviroment";

export const instance = axios.create({
    baseURL: environment.apiUrl,
    headers: {'Content-Type': 'application/json'}
})

instance.interceptors.request.use(config => {
    //todo here token keeping in cookie, maybe need keeping in redux or localStorage
    const accessToken = getAccessToken()

    if(config && config.headers && accessToken){
        config.headers.Authorization = `Bearer ${accessToken}`
    }
    return config
})

instance.interceptors.response.use(config => config, error => {
    //todo making general errorHandler (to create some classes for every error)
    console.log('error', error)
})
