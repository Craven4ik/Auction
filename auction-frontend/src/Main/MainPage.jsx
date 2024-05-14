import React, { useState, useEffect } from "react";
import axios from "axios";
import FilterSection from "./FilterSection";
import ProductList from "./ProductList";
import { config } from "../config";
import NavBar from "../Nav/NavBar";
import Footer from "../Footer/Footer";

axios.defaults.withCredentials = true;

const MainPage = () => {
    const [products, setProducts] = useState([])
    const [filters, setFilters] = useState({})

    useEffect(() => {
        const fetchProducts = async () => {
            try {
                const response = await axios.get(`${config.API_BASE_URL}/api/Product`, { params: filters, withCredentials: true });
                setProducts(response.data)
            } catch (error) {
                console.log(error)
            }
        }

        fetchProducts()
    }, [filters])

    const handleFilterChange = (newFilters) => {
        setFilters((prevFilters) => ({ ...prevFilters, ...newFilters }));
    };

    return (
        <div className="wrapper">
            <NavBar />
            <div className="main-area-wrapper">
                <FilterSection onFilterChange={handleFilterChange} />
                <ProductList products={products}/>
            </div>
            <div className="footer">
                <Footer />
            </div>
        </div>
    )
}

export default MainPage;