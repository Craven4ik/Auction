import React, { useState } from 'react';
import './MainPage.css';

const FilterSection = ({ onFilterChange }) => {
    const [status, setStatus] = useState('');
    const [startPrice, setStartPrice] = useState('');
    const [maxBid, setMaxBid] = useState('');
    const [maxBuyout, setMaxBuyout] = useState('');
    const [startDate, setStartDate] = useState('');
    const [endDate, setEndDate] = useState('');
    const [errors, setErrors] = useState({});

    const handleFilterChange = () => {
        const newErrors = {};
        if (endDate && startDate && endDate < startDate) {
            newErrors.endDate = 'Дата "по" должна быть больше даты "с"';
        }
        if (startPrice && startPrice <= 0) {
            newErrors.startPrice = 'Должно быть больше нуля';
        }
        if (maxBid && maxBid <= 0) {
            newErrors.maxBid = 'Должно быть больше нуля';
        }
        if (maxBuyout && maxBuyout <= 0) {
            newErrors.maxBuyout = 'Должно быть больше нуля';
        }

        setErrors(newErrors);

        if (Object.keys(newErrors).length === 0) {
            onFilterChange({ status, startPrice, maxBid, maxBuyout, startDate, endDate });
        }
    };

    return (
        <div className="filter-section">
            <div className="filter-section-title">Параметры поиска</div>
            <div className="filter-section-param">
                <div className="filter-section-param-column">
                    <div className="filter-section-row">
                        <div className="filter-section-label">Статус:</div>
                        <select className='filter-section-input' value={status} onChange={(e) => setStatus(e.target.value)}>
                            <option value=""></option>
                            <option value="0">Продается</option>
                            <option value="1">Продан</option>
                            <option value="2">Отказано</option>
                            <option value="3">Подготовка</option>
                        </select>
                    </div>
                    <div className="filter-section-row">
                        <div className="filter-section-label">Начальная цена:</div>
                        <input className='filter-section-input' type="number" value={startPrice} onChange={(e) => setStartPrice(e.target.value)} />
                    </div>
                    {errors.startPrice && <p className="filter-section-error">{errors.startPrice}</p>}
                </div>
                <div className="filter-section-param-column">
                    <div className="filter-section-row">
                        <div className="filter-section-label">Максимальная ставка:</div>
                        <input className='filter-section-input' type="number" value={maxBid} onChange={(e) => setMaxBid(e.target.value)} />
                    </div>
                    {errors.maxBid && <p className="filter-section-error">{errors.maxBid}</p>}
                    <div className="filter-section-row">
                        <div className="filter-section-label">Максимальная цена выкупа:</div>
                        <input className='filter-section-input' type="number" value={maxBuyout} onChange={(e) => setMaxBuyout(e.target.value)} />
                    </div>
                    {errors.maxBuyout && <p className="filter-section-error">{errors.maxBuyout}</p>}
                </div>
                <div className="filter-section-param-column">
                    <div className="filter-section-row">
                        <div className="filter-section-label">Дата проведения с:</div>
                        <input className='filter-section-input' type="date" value={startDate} onChange={(e) => setStartDate(e.target.value)} />
                    </div>
                    <div className="filter-section-row">
                        <div className="filter-section-label">Дата проведения по:</div>
                        <input className='filter-section-input' type="date" value={endDate} onChange={(e) => setEndDate(e.target.value)} />
                    </div>
                    {errors.endDate && <p className="filter-section-error">{errors.endDate}</p>}
                </div>
            </div>
            <button className="filter-section-button" onClick={handleFilterChange}>Применить фильтры</button>
        </div>
    );
};

export default FilterSection;
