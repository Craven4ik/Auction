import React from "react";
import NavBar from "../Nav/NavBar";
import Footer from "../Footer/Footer";
import "./Profile.css"
import SellingProductsList from "./SellingProductsList";
import PurchasesList from "./PurchasesList";
import UserProfile from "./UserProfile";
import BetsList from "./BetsList";

const ProfilePage = () => {
    return(
        <div className="wrapper-profile">
            <NavBar />
            <div className="profile-main-area-wrapper">
                <div className="left-area-wrapper">
                    <SellingProductsList />
                    <PurchasesList />
                    <BetsList />
                </div>
                <div className="right-area-wrapper">
                    <UserProfile />
                </div>
            </div>
            <div className="footer">
                <Footer />
            </div>
        </div>
    )
}

export default ProfilePage