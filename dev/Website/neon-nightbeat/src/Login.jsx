import React, { useState } from "react";

export const Login = (props) => {
    const [email, setUsername] = useState('');
    const [pass, setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email);
    }

    return (
        <div className="auth-form-container">
            <h2>LOGIN</h2>
            <form className="form" onSubmit={handleSubmit}>
                <input value={email} onChange={(e) => setUsername(e.target.value)}type="username" placeholder="Username" id="username" name="username" />
                <input value={pass} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="Password" id="password" name="password" />
                <button className="login-btn" type="submit">LOG IN</button>
            </form>
            <button className="link-btn" onClick={() => props.onFormSwitch('register')}>Don't have an account? Register here.</button>
        </div>
    )
}