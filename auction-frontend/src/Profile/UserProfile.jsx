import React, { useEffect, useState } from "react";
import axios from "axios";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faUserCircle } from "@fortawesome/free-solid-svg-icons";
import "./Profile.css"
import { Link, useNavigate } from "react-router-dom";
import { config } from "../config";

const UserProfile = () => {
    const navigate = useNavigate()
    const [userInfo, setUserInfo] = useState(null)
    const [amount, setAmount] = useState('')
    const [amountError, setAmountError] = useState(false)

    const amountHandler = (e) => {
        setAmount(e.target.value)

        if (e.target.value > 0 || amount === '') {
            setAmountError(false)
        }
        else {
            setAmountError(true)
        }
    }

    const amountButtonHandler = async () => {
        const amountValue = parseFloat(amount);

        if (isNaN(amountValue) || amountValue <= 0) {
            setAmountError(true);
            return;
        }

        const amountInfo = {
            amount: amountValue
        }

        try {
            const response = await axios.put(`${config.API_BASE_URL}/api/User/updateBalance`, amountInfo, { withCredentials: true });
            console.log(response.data)

            setUserInfo(prevUserInfo => ({
                ...prevUserInfo,
                balance: prevUserInfo.balance + amountValue
            }));
            setAmount('')
        } catch (error) {
            console.log(error)
        }
    }

    useEffect(() => {
        const fetchUserInfo = async () => {
            if (userInfo !== null) return
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/User/getUserInfo`, { withCredentials: true });
                setUserInfo(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchUserInfo()
    }, [userInfo])

    const logoutButtonHandler = async () => {
        try {
            const response = await axios.get(`${config.API_BASE_URL}/api/User/logout`, { withCredentials: true });
            console.log(response.data)
        } catch (error) {
            console.log(error)
        }

        navigate('/login')
    }

    return (
        <>
            {userInfo === null
                ? <></>
                : <div className="user-profile">
                    <div className="user-profile-image">
                        <FontAwesomeIcon icon={faUserCircle} size="10x" />
                    </div>
                    <div className="user-profile-name">Имя: {userInfo.name}</div>
                    <div className="user-profile-name">Почта: {userInfo.email}</div>
                    <div className="user-profile-registration-date">Дата регистриции: {new Date(userInfo.registrationDate).toLocaleDateString()}</div>
                    <div className="user-profile-balance">Баланс: {userInfo.balance} р.</div>
                    <div className="user-profile-balance">Свободный баланс: {userInfo.freeBalance} р.</div>
                    <Link className="link-to-product-add" to="/addProduct">Добавить товар</Link>
                    <div className="user-profile-buttons">
                        <input onChange={amountHandler} value={amount} type="number" placeholder="Сумма пополнения" className="user-profile-input" />
                        {amountError && <p className="filter-section-error">Сумма должна быть больше нуля</p>}
                        <button disabled={amountError || amount === ''} onClick={amountButtonHandler} className="user-profile-button">Пополнить баланс</button>
                        <button className="user-profile-button" onClick={logoutButtonHandler}>Выход</button>
                    </div>
                </div>}
        </>
    )
}

export default UserProfile