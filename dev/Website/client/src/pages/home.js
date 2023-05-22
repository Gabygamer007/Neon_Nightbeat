import { Link } from "react-router-dom";

export const Home = () => {   
    return (
        <div className='main-container'>
            <>
            <div className='navbar'>
                <Link className='link login' to="/login"> LOGIN</Link>
                <Link className='link register' to="/register"> REGISTER</Link>
            </div>
            </>  
            <div className='title'>WELCOME TO NEON NIGHTBEAT WEBSITE</div>
        </div>
    );
};