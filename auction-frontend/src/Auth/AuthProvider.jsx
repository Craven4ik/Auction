import React, { createContext, useState } from 'react';
import axios from 'axios';
import { config } from "../config.jsx";

export const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    const checkAuthentication = async () => {
        try {
            const response = await axios.get(`${config.API_BASE_URL}/api/User/checkToken`, {withCredentials: true});
            console.log(response.data);
            setIsAuthenticated(true);
        } catch (error) {
            console.log(error);
            setIsAuthenticated(false);
        }
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, checkAuthentication }}>
            {children}
        </AuthContext.Provider>
    );
};