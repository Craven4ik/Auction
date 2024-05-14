import React, { useState } from "react";
import axios from "axios";
import NavBar from "../../Nav/NavBar";
import Footer from "../../Footer/Footer";
import { config } from "../../config";
import "./ProductAddPanel.css";

const ProductAddPanel = () => {
    const [name, setName] = useState('')
    const [description, setDescription] = useState('')
    const [betStep, setBetStep] = useState('')
    const [startPrice, setStartPrice] = useState('')
    const [buyOutPrice, setBuyOutPrice] = useState('')
    const [startTime, setStartTime] = useState('')
    const [endTime, setEndTime] = useState('')
    const [errors, setErrors] = useState({})
    const [isMessageVisible, setMessageVisible] = useState(false);

    const handleAddPanel = async () => {
        const newErrors = {}
        if (name === '') {
            newErrors.name = true
        }
        if (description === '') {
            newErrors.description = true
        }
        if (betStep === '' || betStep <= 0) {
            newErrors.betStep = true
        }
        if (startPrice === '' || startPrice <= 0) {
            newErrors.startPrice = true
        }
        if (buyOutPrice !== '' && startPrice !== '' && parseInt(buyOutPrice) <= parseInt(startPrice)) {
            newErrors.buyOutPrice = true
        }
        if (startTime === '' || (startTime && new Date(startTime) < new Date())) {
            newErrors.startTime = true
        }
        if (endTime === '' || (endTime && startTime && endTime < startTime)) {
            newErrors.endTime = true
        }

        setErrors(newErrors)

        if (Object.keys(newErrors).length === 0) {
            const product = {
                name: name,
                description: description,
                betStep: betStep,
                startPrice: startPrice,
                buyOutPrice: buyOutPrice,
                startTime: startTime,
                endTime: endTime
            }

            try {
                await axios.post(`${config.API_BASE_URL}/api/Product`, product, { withCredentials: true });
                setName('')
                setDescription('')
                setBetStep('')
                setStartPrice('')
                setBuyOutPrice('')
                setStartTime('')
                setEndTime('')

                setMessageVisible(true);
                setTimeout(() => {
                    setMessageVisible(false);
                }, 3000);
            } catch (error) {
                console.log(error)
            }
        }
    }

    return (
        <div className="product-add-wrapper">
            <NavBar />
            <div className="product-add-main-area-wrapper">
                <div className="product-add-main-area-name">Панель создания нового товара</div>
                <div className="product-add-attributes">
                    <div className="product-add-attribute">
                        <div className="product-add-label">Название:</div>
                        <input type="text" className="product-add-input" value={name} onChange={(e) => setName(e.target.value)} />
                    </div>
                    {errors.name && <div className="product-add-error">Название не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Описание:</div>
                        <input type="text" className="product-add-input" value={description} onChange={(e) => setDescription(e.target.value)} />
                    </div>
                    {errors.description && <div className="product-add-error">Описание не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Шаг ставки:</div>
                        <input type="number" className="product-add-input" value={betStep} onChange={(e) => setBetStep(e.target.value)} />
                    </div>
                    {errors.betStep && <div className="product-add-error">Шаг ставки не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Начальная цена:</div>
                        <input type="text" className="product-add-input" value={startPrice} onChange={(e) => setStartPrice(e.target.value)} />
                    </div>
                    {errors.startPrice && <div className="product-add-error">Начальная не может быть пустой</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Цена выкупа:</div>
                        <input type="text" className="product-add-input" value={buyOutPrice} onChange={(e) => setBuyOutPrice(e.target.value)} />
                    </div>
                    {errors.buyOutPrice && <div className="product-add-error">Цена выкупа должна быть больше начальной цены</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Дата начала торгов:</div>
                        <input type="date" className="product-add-input" value={startTime} onChange={(e) => setStartTime(e.target.value)} />
                    </div>
                    {errors.startTime && <div className="product-add-error">Дата начала торгов не может быть пустой или быть раньше текущей даты</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Дата окончания торгов:</div>
                        <input type="date" className="product-add-input" value={endTime} onChange={(e) => setEndTime(e.target.value)} />
                    </div>
                    {errors.endTime && <div className="product-add-error">Дата окончания торгов не может быть пустой и должна быть больше даты начала торгов</div>}
                    <button className="product-add-btn" onClick={handleAddPanel}>Добавить товар</button>
                    <div className={`product-add-success ${isMessageVisible ? 'show' : ''}`}>Товар успешно добавлен</div>
                </div>
            </div>
            <div className="footer">
                <Footer />
            </div>
        </div>
    )
}

export default ProductAddPanel