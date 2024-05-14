import React, { useState, useEffect, useContext } from "react";
import { Link, useNavigate } from 'react-router-dom';
import './Auth.css'
import axios from 'axios';
import { config } from "../config.jsx";
import { AuthContext } from './AuthProvider';

const LoginForm = () => {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [emailDirty, setEmailDirty] = useState(false)
    const [passwordDirty, setPasswordDirty] = useState(false)
    const [emailError, setEmailError] = useState('Email не может быть пустым')
    const [passwordError, setPasswordError] = useState('Пароль не может быть пустым')
    const [error, setError] = useState(false)
    const [requestError, setRequestError] = useState('Пользователь с таким email не существует')

    const { checkAuthentication } = useContext(AuthContext);

    const navigate = useNavigate();

    const blurHandler = (e) => {
        // eslint-disable-next-line default-case
        switch (e.target.name) {
            case 'email':
                setEmailDirty(true)
                break
            case 'password':
                setPasswordDirty(true)
                break
        }
    }

    const emailHandler = (e) => {
        setEmail(e.target.value)
        const re = /^(([^<>()[\].,;:\s@"]+(\.[^<>()[\].,;:\s@"]+)*)|(".+"))@(([^<>()[\].,;:\s@"]+\.)+[^<>()[\].,;:\s@"]{2,})$/iu;
        if (!re.test(String(e.target.value).toLowerCase())) setEmailError('Некорректный email')
        else setEmailError('')
    }

    const passwordHanlder = (e) => {
        setPassword(e.target.value)
        if (e.target.value.length < 4 || e.target.value.length > 8) {
            setPasswordError('Пароль должен быть длиннее 3 и короче 9 символов')
            if (!e.target.value) setPasswordError('Пароль не может быть пустым')
        }
        else setPasswordError('')
    }

    const [formValid, setFormValid] = useState(false)

    useEffect(() => {
        if (emailError || passwordError) setFormValid(false)
        else setFormValid(true)
    }, [emailError, passwordError])

    const handleSubmit = async (e) => {
        e.preventDefault()
    
        const user = {
            Email: email,
            Password: password
        }
    
        try {
            const response = await axios.post(`${config.API_BASE_URL}/api/User/login`, user, { withCredentials: true })
            await checkAuthentication();
            setError(false)
            console.log(response.data)
            navigate('/main')
        } catch (error) {
            setError(true)
            if (error.response.data === 'Failed to login')
                setRequestError('Неверный пароль')
            else if (error.response.data === 'User not found')
                setRequestError('Пользователь с таким email не существует')
            console.log(error)
        }
    };

    return (
        <form className='form' onSubmit={handleSubmit}>
            <h1>Вход</h1>
            {error && <div style={{color:'red'}}>{requestError}</div>}
            {(emailDirty && emailError) && <div style={{color:'red'}}>{emailError}</div>}
            <input className='input' onChange={e => emailHandler(e)} value={email} onBlur={e => blurHandler(e)} name="email" type="text" placeholder='Enter your email...'/>

            {(passwordDirty && passwordError) && <div style={{color:'red'}}>{passwordError}</div>}
            <input className='input' onChange={e => passwordHanlder(e)} value={password} onBlur={e => blurHandler(e)} name="password" type="password" placeholder='Enter your password...'/>
            
            <button disabled={!formValid} className={formValid ? 'submitButton' : 'submitButtonDisabled'} type="submit">Войти</button>
            <Link className="auth-link" to="/register">Зарегистрироваться</Link>
            <Link to="/main">Главная</Link>
        </form>
  );
};

export default LoginForm;