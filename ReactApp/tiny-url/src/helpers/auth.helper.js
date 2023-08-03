import Cookies from 'js-cookie'

export const getAccessToken = () => {
    return (Cookies.get('accessToken') || null)
}

export const getUserFromStorage = () => {
    return JSON.parse(localStorage.getItem('user') || {})
}

export const saveTokensStorage = (data) => {
    Cookies.set('accessToken', data.accessToken)
    Cookies.set('refreshToken', data.refreshToken)
}

export const removeFromStorage = () => {
    Cookies.remove('accessToken')
    Cookies.remove('refreshToken')
    localStorage.removeItem('user')
}

export const saveToStorage = (data) => {
    saveTokensStorage(data)
    localStorage.setItem('user', JSON.stringify(data.user))
}