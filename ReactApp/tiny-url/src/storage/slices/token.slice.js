import { createSlice } from '@reduxjs/toolkit'
import { NIL } from 'uuid'
import TokenDto from "../../models/dto/token.dto";

export const tokenSlice = createSlice({
    name: 'token',
    initialState: {
        accessToken: null,
        userId: NIL,
        tokenExpiration: null,
        refreshToken: NIL,
    },
    reducers: {
        setToken: (state, value) => {
            if (value.payload instanceof TokenDto) {
                let token = value.payload
                state.accessToken = token.accessToken
                state.userId = token.userId
                state.tokenExpiration = token.tokenExpiration
                state.refreshToken = token.refreshToken
            }
        },
        removeToken: (state, value) => {
            state.accessToken = null
            state.userId = NIL
            state.tokenExpiration = null
            state.refreshToken = NIL
        },
    },
})

export const { setToken, removeToken } = tokenSlice.actions

export default tokenSlice.reducer
