import React, { useState, useEffect } from "react";
import axios from "axios";
import { config } from "../config";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faEdit } from "@fortawesome/free-solid-svg-icons";
import { faTrash } from "@fortawesome/free-solid-svg-icons";
import { Link } from 'react-router-dom';

const SellingProductsList = () => {
    const headerNames = ["Лот", "Начало торгов", "Окончание торгов", "Максимальная ставка", "Статус", "Управление"]
    const [productsForSale, setProductsForSale] = useState([])

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/Product/getUserProducts`, { withCredentials: true });
                setProductsForSale(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchProducts()
    }, [])

    const handleDeleteBtn = async (productId) => {
        try {
            await axios.delete(`${config.API_BASE_URL}/api/Product/${productId}`, { withCredentials: true });
            setProductsForSale(productsForSale.filter(product => product.id !== productId))
        } catch (error) {
            console.log(error)
        }
    }

    return (
        <div className="product-list-wrapper">
            {productsForSale.length === 0
                ? <div className="product-list-header">У вас еще нет товаров для продажи</div>
                : <div className="user-products">
                    <div className="product-list-header">Товары на продажу</div>
                    <div className="user-product-list">
                        <div className="product-table-names">
                            {headerNames.map((name, index) => (
                                <div className="product-table-name" key={index}>{name}</div>
                            ))}
                        </div>
                        {productsForSale.map((product) => (
                            <div key={product.id} className="product-table-items">
                                <div className="product-table-item">{product.name}</div>
                                <div className="product-table-item">{new Date(product.startTime).toLocaleDateString()}</div>
                                <div className="product-table-item">{new Date(product.endTime).toLocaleDateString()}</div>
                                <div className="product-table-item">{product.maxBet || 'Нет ставок'}</div>
                                <div className="product-table-item">{product.state === 0 ? 'Продается' : product.state === 1 ? 'Продан' : product.state === 2 ? 'Отказано' : 'Подготовка'}</div>
                                {product.state === 3
                                    ?
                                    <div className="product-table-buttons">
                                        {/* <button className="product-table-button"> */}
                                        <Link to={`/editPanel/${product.id}`} className="product-table-button">
                                            <FontAwesomeIcon icon={faEdit} />
                                        </Link>
                                        {/* </button> */}
                                        <button className="product-table-button">
                                            <FontAwesomeIcon icon={faTrash} onClick={() => handleDeleteBtn(product.id)} />
                                        </button>
                                    </div>
                                    : product.state === 2
                                    ?
                                    <div className="product-table-buttons">
                                        <button className="product-table-button">
                                            <FontAwesomeIcon icon={faTrash} onClick={() => handleDeleteBtn(product.id)} />
                                        </button>
                                    </div>
                                    :
                                    <div className="unable-to-change">Этот товар нельзя изменить или удалить</div>
                                }
                            </div>
                        ))}
                    </div>
                </div>
            }
        </div>
    )
}

export default SellingProductsList