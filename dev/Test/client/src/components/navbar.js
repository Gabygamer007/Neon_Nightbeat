import {Link, useNavigate} from "react-router-dom";
import { useCookies } from "react-cookie";

export const Navbar = () => {
    const [cookies, setCookies] = useCookies(["access_token"]);
    const navigate = useNavigate();

    const logout = () => {
        setCookies("access_token", "");
        window.localStorage.removeItem("UserID");
        navigate("/auth");
    }

    return (
        <div className="navbar">
            {!cookies.access_token ? (
                <Link to="/auth"> Login/Register</Link>
                ) : ( 
                <>
                    <Link to="/">Home</Link>
                    <Link to="/profile"> Profile</Link>
                    <Link to="/leaderboard"> Leaderboard</Link>
                    <button onClick={logout}>Logout</button>
                </>   
            )}
        </div>
    );
    
};