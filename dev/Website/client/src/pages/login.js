import { useState } from "react";
import axios from 'axios';
import {useCookies} from "react-cookie";
import {Link, useNavigate} from "react-router-dom";

export const Login = () => {
    return (
        <div className="main-container">
            <div className="navbar">
                <Link className='link home' to="/"> HOME</Link>
                <Link className='link register' to="/register"> REGISTER</Link>
            </div>
            <LoginForm />
        </div>
    );
};

const LoginForm = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    // eslint-disable-next-line
    const [_, setCookies] = useCookies(["access_token"]);

    const navigate = useNavigate();

    const onSubmit = async (event) => {
        event.preventDefault();
        try {
            const response = await axios.post("http://localhost:3001/login/login_post", {username, password});
            setCookies("access_token", response.data.token);
            window.localStorage.setItem("userID", response.data.userID);
            if (response.data.userID != null) {
                navigate("/profile");
            } else {
                alert("Username or password is incorrect!");
            }
        } catch (err) {
            console.error(err);
        }
    };

    return (
        <Form 
            username={username} 
            setUsername={setUsername} 
            password={password} 
            setPassword={setPassword} 
            label="LOGIN"
            onSubmit={onSubmit}
        />
    );
};

const Form = ({username, setUsername, password, setPassword, label, onSubmit}) => {
    return (
        <div className="form-container login">
            <form onSubmit={onSubmit}>
                <div className="form-group">
                    <h2 className="form-group-item login">{label}</h2>
                    <div className="inputbox">
                        <input className="form-group-item input login" type="text" id="username" value={username} onChange={(event) => setUsername(event.target.value)} required/>
                        <label className="label">Username</label>
                    </div>
                    <div className="inputbox">
                        <input className="form-group-item input login" type="password" id="password" value={password} onChange={(event) => setPassword(event.target.value)} required/>
                        <label className="label">Password</label>
                    </div>
                    <button className="form-group-item btn login" onSubmit={onSubmit} type="submit">{label}</button>
                </div>
            </form>
        </div>
    );
};