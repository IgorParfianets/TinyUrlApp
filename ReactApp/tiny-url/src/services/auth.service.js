import {instance} from "./api.interceptors";
import Cookies from 'js-cookie'
import {saveTokensStorage} from "../helpers/auth.helper";
import {environment} from "../enviroment/enviroment";

export default class AuthService {
    constructor() {
        this._userEndpoint = environment.userEndpoint
        this._tokenEndpoint = environment.tokenEndpoints
    }
    async login(data){

        const response = await instance.post(
            this._tokenEndpoint.createToken, data
        )
        return response
    }

    async registration(data){
        const response = await instance.post(
            this._userEndpoint, JSON.stringify(data)
        )
        if(response.data.accessToken)
            saveTokensStorage(response.data)

        return response.data
    }

    async getNewTokens(){
        const refreshToken = Cookies.get('refreshToken')

        console.log(refreshToken)
        const response = await instance.post(
            this._tokenEndpoint.refreshToken,
            {refreshToken},
            {headers: {'Content-Type': 'application/json'}
            })
        console.log(response.data)

        if(response.data.accessToken)
            saveTokensStorage(response.data)

        return response
    }


}