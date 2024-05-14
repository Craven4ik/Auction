import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Link, useLocation, useNavigate } from 'react-router-dom';
import './NavBar.css';
import { config } from "../config";

const NavBar = () => {
    const [balance, setBalance] = useState(0)

    const navigate = useNavigate()
    const location = useLocation()

    useEffect(() => {
        const fetchBalance = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/User/getBalance`, { withCredentials: true });
                setBalance(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchBalance()
    }, [])

    const logoClickHandler = () => {
        navigate('/home')
    }

    const balanceClickHandler = () => {
        navigate('/profile')
    }

    const balanceClassName = location.pathname === '/profile' ? 'navbar-right hidden-part' : 'navbar-right';

    return (
        <div className="navbar">
            <div className="navbar-left" onClick={logoClickHandler}>
                <div className="navbar-logo"></div>
                <div className="navbar-name">Маркетплейс</div>
            </div>
            <div className="navbar-center">
                <Link className="navbar-link" to="/home">Главная</Link>
            </div>
            <div className="navbar-center">
                <Link className="navbar-link" to="/main">Торги</Link>
            </div>
            <div className="navbar-center">
                <Link className="navbar-link" to="/profile">Профиль</Link>
            </div>
            <div className={balanceClassName} onClick={balanceClickHandler}>Баланс: {balance} р.</div>
        </div>
    );
};

export default NavBar;
