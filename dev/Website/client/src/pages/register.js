import { useState } from "react";
import axios from 'axios';
import { Link } from 'react-router-dom';

export const Register = () => {
    return (
        <div className="main-container">
            <div className="navbar">
                <Link className='link home' to="/"> HOME</Link>
                <Link className='link login' to="/login"> LOGIN</Link>
            </div>
            <RegisterForm />
        </div>
    );
};


const RegisterForm = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const onSubmit = async (event) => {
        event.preventDefault();
        try {
            await axios.post("http://localhost:3001/register/register_post", {username, password});
            alert("Registration completed! Now Login.");
        } catch (err) {
            console.error(err)
        }
    };

    return (
        <Form 
            username={username} 
            setUsername={setUsername}
            password={password} 
            setPassword={setPassword}
            label="REGISTER"
            onSubmit={onSubmit}
        />
    );
};

const Form = ({username, setUsername, password, setPassword, label, onSubmit}) => {
    return (
        <div className="form-container register">
            <form onSubmit={onSubmit}>
                <div className="form-group">
                    <h2 className="form-group-item register">{label}</h2>
                    <div className="inputbox">
                        <input className="form-group-item input register" type="text" id="username" value={username} onChange={(event) => setUsername(event.target.value)} required/>
                        <label className="label">Username</label>
                    </div>
                    <div className="inputbox">
                        <input className="form-group-item input register" type="password" id="password" value={password} onChange={(event) => setPassword(event.target.value)} required/>
                        <label className="label">Password</label>
                    </div>
                    <button className="form-group-item btn register" onSubmit={onSubmit} type="submit">{label}</button>
                </div>
            </form>
        </div>
    );
};