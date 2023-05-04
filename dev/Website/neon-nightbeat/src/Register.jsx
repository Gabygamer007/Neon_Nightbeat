import React, { useState } from "react";

export const Register = (props) => {
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
                <h2>REGISTER</h2>
                <form className="form" onSubmit={handleSubmit}>
                    <input value={username} onChange={(e) => setUsername(e.target.value)}type="text" placeholder="Username" id="username" name="username" />
                    <input value={password} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="Password" id="password" name="password" />
                    <button className="register-btn" type="submit">REGISTER</button>
                </form>
                <button className="link-btn" onClick={() => props.onFormSwitch('login')}>Already have an account? Login here.</button>
            </div>
        </div>
        
    )
}