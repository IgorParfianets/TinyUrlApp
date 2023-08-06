import {createSlice} from '@reduxjs/toolkit'
import {NIL} from 'uuid'

export const tokenSlice = createSlice({
    name: 'token',
    initialState: {
        accessToken: null,
        userId: NIL,
        tokenExpiration: null,
        refreshToken: NIL,
    },
    reducers: {
        setToken: (state, action) => {
            const {accessToken, userId, tokenExpiration, refreshToken} = action.payload;

            state.accessToken = accessToken
            state.userId = userId
            state.tokenExpiration = tokenExpiration
            state.refreshToken = refreshToken
        },
        removeToken: (state) => {
            state.accessToken = null
            state.userId = NIL
            state.tokenExpiration = null
            state.refreshToken = NIL
        },
    },
})

export const {setToken, removeToken} = tokenSlice.actions

export default tokenSlice.reducer
