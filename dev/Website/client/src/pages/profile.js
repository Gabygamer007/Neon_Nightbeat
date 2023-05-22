import {Link, useNavigate} from "react-router-dom";
import { useCookies } from "react-cookie";

export const Profile = () => {
    // eslint-disable-next-line
    const [cookies, setCookies] = useCookies(["access_token"]);
    const navigate = useNavigate();

    const logout = () => {
        setCookies("access_token", "");
        window.localStorage.removeItem("UserID");
        navigate("/login");
    }
    
    return (
        <div className="main-container">
            <>
                <div className="navbar">
                    <Link className="link profile" to="/profile"> Profile</Link>
                    <Link className="link leaderboard" to="/leaderboard"> Leaderboard</Link>
                    <button className="btn logout" onClick={logout}>SIGN OUT</button>
                </div>
                
            </>  
            <div className="title">Profile</div>
        </div>
    );
};