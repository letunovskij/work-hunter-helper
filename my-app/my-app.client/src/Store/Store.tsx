import { configureStore } from '@reduxjs/toolkit'; // Импортируем функцию configureStore из библиотеки @reduxjs/toolkit
import currencyReducer from './CurrencySlice';

// configureStore — это функция, которая создает хранилище
export const store = configureStore({
    reducer: {
        currency: currencyReducer,
    },
});

// RootState — это тип, который представляет состояние хранилища
export type RootState = ReturnType<typeof store.getState>;
// AppDispatch — это тип, который представляет функцию dispatch нашего стора
export type AppDispatch = typeof store.dispatch;