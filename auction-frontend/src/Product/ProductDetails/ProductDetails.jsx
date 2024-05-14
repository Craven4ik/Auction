import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import axios from 'axios';
import './ProductDetails.css';
import { config } from "../../config";
import Footer from "../../Footer/Footer";
import NavBar from "../../Nav/NavBar";

const ProductDetails = () => {
  const [product, setProduct] = useState(null)
  const [owner, setOwner] = useState('')
  const [buyer, setBuyer] = useState('')
  const [isOwner, setIsOwner] = useState(false)
  const [bet, setBet] = useState('')
  const [betError, setBetError] = useState(false)
  const [betErrorMessage, setBetErrorMessage] = useState('')
  const [balance, setBalance] = useState('')
  const [userId, setUserId] = useState('')
  const { id } = useParams();
  const [isMessageVisible, setMessageVisible] = useState(false);
  const [message, setMessage] = useState('')

  const navigate = useNavigate()

  useEffect(() => {
    const fetchProduct = async () => {
      if (owner !== '') return
      try {
        const response = await axios.get(`${config.API_BASE_URL}/api/Product/${id}`, { withCredentials: true });
        setProduct(response.data)
        if (!product) return

        const fetchUserNames = async () => {
          try {
            const response = await axios.get(`${config.API_BASE_URL}/api/User/getUsername/${product.ownerId}`, { withCredentials: true });
            setOwner(response.data)
          } catch (error) {
            console.log(error)
          }
          try {
            const response = await axios.get(`${config.API_BASE_URL}/api/User/getUsername/${product.buyerId}`, { withCredentials: true });
            setBuyer(response.data)
          } catch (error) {
            console.log(error)
          }
          try {
            const response = await axios.get(`${config.API_BASE_URL}/api/User/checkOwner/${product.ownerId}`, { withCredentials: true });
            setIsOwner(response.data)
          } catch (error) {
            console.log(error)
          }
        }

        if (owner === '')
          fetchUserNames()
      } catch (error) {
        console.log(error)
      }
    }

    fetchProduct()
  }, [id, product, owner])

  useEffect(() => {
    const fetchBalance = async () => {
      try {
        const response = await axios.get(`${config.API_BASE_URL}/api/User/getFreeBalance`, { withCredentials: true });
        setBalance(response.data)
      } catch (error) {
        console.log(error)
      }
    }

    fetchBalance()
  }, [balance])

  useEffect(() => {
    const fetchUserId = async () => {
      try {
        const response = await axios.get(`${config.API_BASE_URL}/api/User/getUserId`, { withCredentials: true });
        setUserId(response.data)
      } catch (error) {
        console.log(error)
      }
    }

    fetchUserId()
  }, [balance])

  if (!product) {
    return <div>Загрузка...</div>
  }

  const handleOffer = (e) => {
    setBet(e.target.value)

    if (product.maxBetOwnerId && product.maxBetOwnerId === userId) {
      setBetError(true)
      setBetErrorMessage('Ваша ставка наивысшая')
    }
    else if (e.target.value <= 0) {
      setBetError(true)
      setBetErrorMessage('Размер ставки должен быть положительным')
    }
    else if (product.maxBet && product.betStep && e.target.value <= product.maxBet + product.betStep) {
      setBetError(true)
      setBetErrorMessage('Размер ставки слишком маленький')
    }
    else if (product.startPrice && !product.betId && e.target.value < product.startPrice) {
      setBetError(true)
      setBetErrorMessage('Размер ставки меньше начальной цены')
    }
    else if (balance < e.target.value) {
      setBetError(true)
      setBetErrorMessage('У вас недостаточно средств')
    }
    else {
      setBetError(false)
    }
  }

  const handleSubmit = async () => {
    const offer = {
      offer: bet
    }

    try {
      const response = await axios.post(`${config.API_BASE_URL}/api/Product/doBet/${id}`, offer, { withCredentials: true });
      console.log(response.data)
      setMessage('Ваша ставка успешно добавлена')
      setMessageVisible(true);
      setTimeout(() => {
        setMessageVisible(false);
        navigate('/main')
      }, 3000);
    } catch (error) {
      console.log(error)
    }
  }

  const handleBuyOut = async () => {
    try {
      const response = await axios.post(`${config.API_BASE_URL}/api/Product/buyOutProduct/${id}`, { withCredentials: true });
      console.log(response.data)
      setMessage('Выкуп товара прошел успешно')
      setMessageVisible(true);
      setTimeout(() => {
        setMessageVisible(false);
        navigate('/profile')
      }, 3000);
    } catch (error) {
      console.log(error)
    }
  }

  return (
    <div className="wrapper">
      <NavBar />
      <div className="product-details">
        <h2>Информация о товаре:</h2>
        <div className="product-details-row">
          <div className="product-details-column">
            <p><strong>Лот:</strong> {product.name}</p>
            <p><strong>Владелец:</strong> {owner || '-'}</p>
            <p><strong>Покупатель:</strong> {buyer || '-'}</p>
            <p><strong>Начальная цена:</strong> {product.startPrice}</p>
            <p><strong>Шаг ставки:</strong> {product.betStep}</p>
          </div>
          <div className="product-details-column">
            <p><strong>Цена выкупа:</strong> {product.buyOutPrice}</p>
            <p><strong>Начало торгов:</strong> {new Date(product.startTime).toLocaleDateString()}</p>
            <p><strong>Окончание торгов:</strong> {new Date(product.endTime).toLocaleDateString()}</p>
            <p><strong>Максимальная ставка:</strong> {product.maxBet || 'Нет ставок'}</p>
            <p><strong>Статус:</strong> {product.state === 0 ? 'Продается' : product.state === 1 ? 'Продан' : product.state === 2 ? 'Отказано' : 'Подготовка'}</p>
          </div>
        </div>
        <p><strong>Описание:</strong> {product.description}</p>
        <div className="buttons-section">
          {!isOwner && product.state === 0
            ? <div className="bet-section">
              <div className="bet-info">
                <div className="bet-label">Размер ставки:</div>
                <input type="number" className="bet-input" value={bet} onChange={(e) => handleOffer(e)} />
              </div>
              {betError && <div className="bet-error-label">{betErrorMessage}</div>}
              {/* {balance < product.buyOutPrice && <div className="bet-error-label">Для выкупа недостаточно средств</div>} */}
              {!product.buyOutPrice
                ? <div className="bet-error-label">Этот товар нельзя выкупить</div>
                : balance < product.buyOutPrice
                  ? <div className="bet-error-label">Для выкупа недостаточно средств</div>
                  : <></>}
              <div className="bet-section-btns">
                <button className="bet-section-btn" disabled={betError || !bet} onClick={handleSubmit}>Сделать ставку</button>
                <button className="bet-section-btn" disabled={balance < product.buyOutPrice} onClick={handleBuyOut} >Выкупить</button>
              </div>
              <div className={`product-add-success ${isMessageVisible ? 'show' : ''}`}>{message}</div>
            </div>
            : isOwner
              ? <div className="failed-to-do-bet">Нельзя делать ставки на свои товары</div>
              : <div className="failed-to-do-bet">На этот товар ставки не доступны</div>
          }
        </div>
      </div>
      <div className="footer">
        <Footer />
      </div>
    </div>
  );
};

export default ProductDetails;
