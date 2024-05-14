import React, { useState, useEffect } from "react";
import { Link, useNavigate } from 'react-router-dom';
import './Auth.css'
import axios from 'axios';
import { config } from "../config.jsx";

axios.defaults.withCredentials = true;

const RegisterForm = () => {
    const [userName, setUserName] = useState('')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [userNameDirty, setUserNameDirty] = useState(false)
    const [emailDirty, setEmailDirty] = useState(false)
    const [passwordDirty, setPasswordDirty] = useState(false)
    const [userNameError, setUserNameError] = useState('Имя не может быть пустым')
    const [emailError, setEmailError] = useState('Email не может быть пустым')
    const [passwordError, setPasswordError] = useState('Пароль не может быть пустым')

    const navigate = useNavigate()

    const blurHandler = (e) => {
        // eslint-disable-next-line default-case
        switch (e.target.name) {
            case 'username':
                setUserNameDirty(true)
                break
            case 'email':
                setEmailDirty(true)
                break
            case 'password':
                setPasswordDirty(true)
                break
        }
    }

    const userNameHanlder = (e) => {
        setUserName(e.target.value)
        if (e.target.value.length < 4 || e.target.value.length > 8) {
            setUserNameError('Имя должено быть длиннее 3 и короче 9 символов')
            if (!e.target.value) setUserNameError('Имя не может быть пустым')
        }
        else setUserNameError('')
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
        if (emailError || userNameError || passwordError) setFormValid(false)
        else setFormValid(true)
    }, [emailError, userNameError, passwordError])

    const handleSubmit = async (e) => {
        e.preventDefault();

        const user = {
            UserName: userName,
            Email: email,
            Password: password
        }

        try {
            const respose = await axios.post(`${config.API_BASE_URL}/api/User/register`, user)
            console.log(respose.data)
            navigate('/login')
        } catch (error) {
            console.log(error)
        }
    };

    return (
        <form className='form' onSubmit={handleSubmit}>
            <h1>Регистрация</h1>
            {(userNameDirty && userNameError) && <div style={{color:'red'}}>{userNameError}</div>}
            <input className='input' onChange={e => userNameHanlder(e)} value={userName} onBlur={e => blurHandler(e)} name="username" type="text" placeholder='Enter your username...'/>

            {(emailDirty && emailError) && <div style={{color:'red'}}>{emailError}</div>}
            <input className='input' onChange={e => emailHandler(e)} value={email} onBlur={e => blurHandler(e)} name="email" type="text" placeholder='Enter your email...'/>

            {(passwordDirty && passwordError) && <div style={{color:'red'}}>{passwordError}</div>}
            <input className='input' onChange={e => passwordHanlder(e)} value={password} onBlur={e => blurHandler(e)} name="password" type="password" placeholder='Enter your password...'/>
            
            <button disabled={!formValid} className={formValid ? 'submitButton' : 'submitButtonDisabled'} type="submit">Зарегистрироваться</button>
            <Link className="auth-link" to="/login">Авторизоваться</Link>
        </form>
  );
};

export default RegisterForm;