import React, { useState, useEffect } from "react";
import axios from "axios";
import { config } from "../config";
import { Link } from 'react-router-dom';

const PurchasesList = () => {
    const headerNames = ["Лот", "Начало торгов", "Окончание торгов", "Максимальная ставка", "Статус"]
    const [purchases, setPurchases] = useState([])

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/Product/getUserPurchases`, { withCredentials: true });
                setPurchases(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchProducts()
    }, [])

    return (
        <div className="product-list-wrapper">
            {purchases.length === 0
                ? <div className="product-list-header">У вас еще нет покупок</div>
                : <div className="user-products">
                    <div className="product-list-header">Список покупок</div>
                    <div className="user-product-list">
                        <div className="product-table-names">
                            {headerNames.map((name, index) => (
                            <div className="product-table-name" key={index}>{name}</div>
                            ))}
                        </div>
                        {purchases.map((product) => (
                            <Link to={`/product/${product.id}`} key={product.id} className="product-link">
                                <div key={product.id} className="product-table-items">
                                    <div className="product-table-item">{product.name}</div>
                                    <div className="product-table-item">{new Date(product.startTime).toLocaleDateString()}</div>
                                    <div className="product-table-item">{new Date(product.endTime).toLocaleDateString()}</div>
                                    <div className="product-table-item">{product.maxBet || 'Нет ставок'}</div>
                                    <div className="product-table-item">{product.state === 0 ? 'Продается' : product.state === 1 ? 'Продан' : product.state === 2 ? 'Отказано' : 'Подготовка'}</div>
                                </div>
                            </Link>
                        ))}
                    </div>
                </div>
            }
        </div>
    )
}

export default PurchasesList