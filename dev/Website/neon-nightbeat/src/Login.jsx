import React, { useState } from "react";

export const Login = (props) => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(username);
    }

    return (
        <div>
            <h1>NEON NIGHTBEAT</h1>
            <div className="auth-form-container">
                <h2>LOGIN</h2>
                <form className="form" onSubmit={handleSubmit}>
                    <input value={username} onChange={(e) => setUsername(e.target.value)}type="text" placeholder="Username" id="username" name="username" />
                    <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="Password" id="password" name="password" />
                    <button className="login-btn" type="submit">LOG IN</button>
                </form>
                <button className="link-btn" onClick={() => props.onFormSwitch('register')}>Don't have an account? Register here.</button>
            </div> 
        </div>
        
    )
}