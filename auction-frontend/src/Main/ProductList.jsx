import React from "react";
import { Link } from 'react-router-dom';
import './MainPage.css';

const ProductList = ({ products }) => {
  const headerNames = ["Лот", "Начальная цена", "Цена выкупа", "Шаг ставки", "Начало торгов", "Окончание торгов", "Максимальная ставка", "Статус"]

    return (
        <div className="product-table">
          <div className="product-table-names">
            {headerNames.map((name, index) => (
              <div className="product-table-name" key={index}>{name}</div>
            ))}
          </div>
        {products.length === 0
        ? <div className="loading-block">Товары не найдены...</div>
        : products.map((product) => (
          <Link to={`/product/${product.id}`} key={product.id} className="product-link">
            <div key={product.id} className="product-table-items">
              <div className="product-table-item">{product.name}</div>
              <div className="product-table-item">{product.startPrice}</div>
              <div className="product-table-item">{product.buyOutPrice}</div>
              <div className="product-table-item">{product.betStep}</div>
              <div className="product-table-item">{new Date(product.startTime).toLocaleDateString()}</div>
              <div className="product-table-item">{new Date(product.endTime).toLocaleDateString()}</div>
              <div className="product-table-item">{product.maxBet || 'Нет ставок'}</div>
              <div className="product-table-item">{product.state === 0 ? 'Продается' : product.state === 1 ? 'Продан' : product.state === 2 ? 'Отказано' : 'Подготовка' }</div>
          </div>
          </Link>
        ))}
      </div>
    );
  };
  
  export default ProductList;