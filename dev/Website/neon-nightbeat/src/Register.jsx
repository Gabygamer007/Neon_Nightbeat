import React, { useState } from "react";

export const Register = (props) => {
    const [email, setUsername] = useState('');
    const [pass, setPassword] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        console.log(email);
    }

    return (
        <div className="auth-form-container">
            <h2>REGISTER</h2>
        <form className="form" onSubmit={handleSubmit}>
            <input value={email} onChange={(e) => setUsername(e.target.value)}type="username" placeholder="Username" id="username" name="username" />
            <input value={pass} onChange={(e) => setPassword(e.target.value)} type="password" placeholder="Password" id="password" name="password" />
            <button className="register-btn" type="submit">REGISTER</button>
        </form>
        <button className="link-btn" onClick={() => props.onFormSwitch('login')}>Already have an account? Login here.</button>
    </div>
    )
}