import React, { useEffect, useContext } from "react";
import { BrowserRouter as Router, Route, Routes, Navigate } from "react-router-dom";
import LoginForm from "../Auth/LoginForm.jsx";
import RegisterForm from "../Auth/RegisterForm";
import MainPage from "../Main/MainPage.jsx";
import { AuthContext } from "../Auth/AuthProvider.jsx";
import ProductDetails from "../Product/ProductDetails/ProductDetails.jsx";
import HomePage from "../Home/HomePage.jsx";
import ProfilePage from "../Profile/ProfilePage.jsx";
import ProductAddPanel from "../Product/ProductAddPanel/ProductAddPanel.jsx";
import ProductEditPanel from "../Product/ProductAddPanel/ProductEditPanel.jsx";

const AllRoutes = () => {
    const { isAuthenticated, checkAuthentication } = useContext(AuthContext);

    useEffect(() => {
        checkAuthentication();
    }, []);

    return (
        <Router>
            <Routes>
                <Route path="/login" element={<LoginForm />} />
                <Route path="/register" element={<RegisterForm />} />
                <Route path="/main" element={isAuthenticated ? <MainPage /> : <Navigate to="/login" />} />
                <Route path="/home" element={isAuthenticated ? <HomePage /> : <Navigate to="/login" />} />
                <Route path="/profile" element={isAuthenticated ? <ProfilePage /> : <Navigate to="/login" />} />
                <Route path="/addProduct" element={isAuthenticated ? <ProductAddPanel /> : <Navigate to="/login" />} />
                <Route path="/editPanel/:id" element={isAuthenticated ? <ProductEditPanel /> : <Navigate to="/login" />} />
                <Route path="/product/:id" element={isAuthenticated ? <ProductDetails /> : <Navigate to="/login" />} />
                <Route path="*" element={<Navigate to="/login" />} />
            </Routes>
        </Router>
    );
}

export default AllRoutes