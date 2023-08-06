import {configureStore} from '@reduxjs/toolkit';
import tokenReducer from './slices/token.slice';

export default configureStore({
    reducer: {
        token: tokenReducer,
    },
})