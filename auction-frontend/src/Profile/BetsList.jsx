import React, { useState, useEffect } from "react";
import axios from "axios";
import { config } from "../config";
import { Link } from 'react-router-dom';

const BetsList = () => {
    const headerNames = ["Лот", "Ставка", "Начало торгов", "Окончание торгов"]
    const [bets, setBets] = useState([])

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/Product/getUserBets`, { withCredentials: true });
                setBets(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchProducts()
    }, [])

    return (
        <div className="product-list-wrapper">
            {bets.length === 0
                ? <div className="product-list-header">У вас еще нет ставок</div>
                : <div className="user-products">
                    <div className="product-list-header">Список ставок</div>
                    <div className="user-product-list">
                        <div className="product-table-names">
                            {headerNames.map((name, index) => (
                            <div className="product-table-name" key={index}>{name}</div>
                            ))}
                        </div>
                        {bets.map((product) => (
                            <Link to={`/product/${product.productId}`} key={product.productId} className="product-link">
                                <div key={product.productId} className="product-table-items">
                                    <div className="product-table-item">{product.name}</div>
                                    <div className="product-table-item">{product.offer}</div>
                                    <div className="product-table-item">{new Date(product.startTime).toLocaleDateString()}</div>
                                    <div className="product-table-item">{new Date(product.endTime).toLocaleDateString()}</div>
                                    {/* <div className="product-table-item">{product.maxBet || 'Нет ставок'}</div>
                                    <div className="product-table-item">{product.state === 0 ? 'Продается' : product.state === 1 ? 'Продан' : product.state === 2 ? 'Отказано' : 'Подготовка'}</div> */}
                                </div>
                            </Link>
                        ))}
                    </div>
                </div>
            }
        </div>
    )
}

export default BetsList