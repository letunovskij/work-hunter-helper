import React, { useEffect, useState } from 'react';
import { useSelector } from 'react-redux';
import { fetchCurrencyRates, setBaseCurrency } from '../Store/CurrencySlice';
import { store, RootState } from '../Store/Store';

const CurrencyRates: React.FC = () => {
    const { rates, baseCurrency, status, error } = useSelector((state: RootState) => state.currency); 
    const [selectedCurrency, setSelectedCurrency] = useState(baseCurrency);

    useEffect(() => {
        if (status === 'idle') {
            store.dispatch(fetchCurrencyRates(baseCurrency));
        }
    }, [status, baseCurrency, store.dispatch]);

    const handleCurrencyChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const newCurrency = event.target.value;
        setSelectedCurrency(newCurrency);
        store.dispatch(setBaseCurrency(newCurrency));
        store.dispatch(fetchCurrencyRates(newCurrency));
    };

    if (status === 'loading') {
        return <div>Загрузка...</div>;
    }

    if (status === 'failed') {
        return <div>Ошибка: {error}</div>;
    }

    return (
        <div>
            <h1>Курсы валют</h1>
            <div>
                <label htmlFor="baseCurrency">Валюта: </label>
                <select id="baseCurrency" value={selectedCurrency} onChange={handleCurrencyChange}>
                    {Object.keys(rates).map((currency) => (
                        <option key={currency} value={currency}>
                            {currency}
                        </option>
                    ))}
                </select>
            </div>
            <ul>
                {Object.entries(rates).map(([currency, rate]) => (
                    <li key={currency}>
                        {currency}: {rate}
                    </li>
                ))}
            </ul>
        </div>
    );
};

export default CurrencyRates;