import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from 'react-router-dom';
import axios from "axios";
import NavBar from "../../Nav/NavBar";
import Footer from "../../Footer/Footer";
import { config } from "../../config";
import "./ProductAddPanel.css";

const ProductEditPanel = () => {
    const [errors, setErrors] = useState({})
    const { id } = useParams();
    const [product, setProduct] = useState({
        name: '',
        description: '',
        betStep: '',
        startPrice: '',
        buyOutPrice: '',
        startTime: '',
        endTime: '',
      });

    const navigate = useNavigate()

    useEffect(() => {
        const fetchProduct = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/Product/${id}`, { withCredentials: true });
                setProduct(response.data);
            } catch (error) {
                console.log(error);
            }
        };

        fetchProduct();
    }, [id]);

    const handleEditPanel = async () => {
        const newErrors = {}
        if (product.name === '') {
            newErrors.name = true
        }
        if (product.description === '') {
            newErrors.description = true
        }
        if (product.betStep === '' || product.betStep <= 0) {
            newErrors.betStep = true
        }
        if (product.startPrice === '' || product.startPrice <= 0) {
            newErrors.startPrice = true
        }
        if (product.buyOutPrice !== '' && product.startPrice !== '' && parseInt(product.buyOutPrice) <= parseInt(product.startPrice)) {
            newErrors.buyOutPrice = true
        }
        if (product.startTime === '' || (product.startTime && new Date(product.startTime) < new Date())) {
            newErrors.startTime = true
        }
        if (product.endTime === '' || (product.endTime && product.startTime && product.endTime < product.startTime)) {
            newErrors.endTime = true
        }

        setErrors(newErrors)

        if (Object.keys(newErrors).length === 0) {
            try {
                await axios.put(`${config.API_BASE_URL}/api/Product/${product.id}`, product, { withCredentials: true });
                navigate("/profile")
            } catch (error) {
                console.log(error)
            }
        }
    }

    const handleEscape = () => {
        navigate("/profile")
    }

    return (
        <div className="product-add-wrapper">
            <NavBar />
            <div className="product-add-main-area-wrapper">
                <div className="product-add-main-area-name">Панель изменения товара</div>
                <div className="product-add-attributes">
                    <div className="product-add-attribute">
                        <div className="product-add-label">Название:</div>
                        <input type="text" className="product-add-input" value={product.name} onChange={(e) => setProduct({ ...product, name: e.target.value })} />
                    </div>
                    {errors.name && <div className="product-add-error">Название не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Описание:</div>
                        <input type="text" className="product-add-input" value={product.description} onChange={(e) => setProduct({ ...product, description: e.target.value })} />
                    </div>
                    {errors.description && <div className="product-add-error">Описание не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Шаг ставки:</div>
                        <input type="number" className="product-add-input" value={product.betStep} onChange={(e) => setProduct({ ...product, betStep: e.target.value })} />
                    </div>
                    {errors.betStep && <div className="product-add-error">Шаг ставки не может быть пустым</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Начальная цена:</div>
                        <input type="text" className="product-add-input" value={product.startPrice} onChange={(e) => setProduct({ ...product, startPrice: e.target.value })} />
                    </div>
                    {errors.startPrice && <div className="product-add-error">Начальная не может быть пустой</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Цена выкупа:</div>
                        <input type="text" className="product-add-input" value={product.buyOutPrice} onChange={(e) => setProduct({ ...product, buyOutPrice: e.target.value })} />
                    </div>
                    {errors.buyOutPrice && <div className="product-add-error">Цена выкупа должна быть больше начальной цены</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Дата начала торгов:</div>
                        <input type="date" className="product-add-input" value={product.startTime} onChange={(e) => setProduct({ ...product, startTime: e.target.value })} />
                    </div>
                    {errors.startTime && <div className="product-add-error">Дата начала торгов не может быть пустой или быть раньше текущей даты</div>}
                    <div className="product-add-attribute">
                        <div className="product-add-label">Дата окончания торгов:</div>
                        <input type="date" className="product-add-input" value={product.endTime} onChange={(e) => setProduct({ ...product, endTime: e.target.value })} />
                    </div>
                    {errors.endTime && <div className="product-add-error">Дата окончания торгов не может быть пустой и должна быть больше даты начала торгов</div>}
                    <div className="product-edit-btns">
                        <button className="product-add-btn" onClick={handleEditPanel}>Применить</button>
                        <button className="product-add-btn" onClick={handleEscape}>Отмена</button>
                    </div>
                </div>
            </div>
            <div className="footer">
                <Footer />
            </div>
        </div>
    )
}

export default ProductEditPanel