import {Link, useNavigate} from "react-router-dom";
import { useCookies } from "react-cookie";
import { useState, useEffect } from "react";
import axios from "axios";

export const Profile = () => {
    // eslint-disable-next-line
    const [cookies, setCookies] = useCookies(["access_token"]);
    const navigate = useNavigate();

    const logout = () => {
        setCookies("access_token", "");
        window.localStorage.removeItem("userID");
        navigate("/login");
    }

    const [username, setUsername] = useState("");
    const [scoreList, setScoreList] = useState([]);

    useEffect(() => {
        axios.get(`http://localhost:3001/leaderboard/scores_profile/${window.localStorage.getItem("userID")}`).then((response) => {
            const sortedScores = response.data.sort((a, b) => b.score - a.score);
            setScoreList(sortedScores);
            setScoreList(response.data);
        }).catch((error) => console.log(error));

        axios.get(`http://localhost:3001/user/getUser/${window.localStorage.getItem("userID")}`).then((response) => {
            setUsername(response.data);
        }).catch((error) => console.log(error));
    }, []);
    
    return (
        <div className="main-container">
            <>
                <div className="navbar">
                    <Link className="link profile" to="/profile"> Profile</Link>
                    <Link className="link leaderboard" to="/leaderboard"> Leaderboard</Link>
                    <button className="btn logout" onClick={logout}>SIGN OUT</button>
                </div>
                
            </>  
            <div className="profile">
                <h2 className="profile-main-title">Profile of {username}</h2>
                <div className="main-profile-box">
                    <div className="profile-titles">
                        <div className="music-title">MUSIC</div>
                        <div className="score-title">SCORE</div>
                        <div className="accuracy-title">ACCURACY</div>
                        <div className="combo-title">COMBO</div>
                        <div className="rank-title">RANK</div>
                    </div>
                    <div className="profile-box">
                        <div className="profile-music-box">
                            {scoreList.map((scoreProfile) => {
                                return (
                                    <div className="profile-music">{scoreProfile.music}</div>
                                );
                            })}
                        </div>
                        <div className="profile-score-box">
                            {scoreList.map((scoreProfile) => {
                                return (
                                    <div className="profile-score">{scoreProfile.score}</div>
                                );
                            })}
                        </div>
                        <div className="profile-accuracy-box">
                            {scoreList.map((scoreProfile) => {
                                return (
                                    <div className="profile-accuracy">{scoreProfile.accuracy.toFixed(2)} %</div>
                                );
                            })}
                        </div>
                        <div className="profile-highest-combo-box">
                            {scoreList.map((scoreProfile) => {
                                return (
                                    <div className="profile-combo">{scoreProfile.highestCombo}x</div>
                                );
                            })}
                        </div>
                        <div className="profile-rank-box">
                            {scoreList.map((scoreProfile) => {
                                return (
                                    <div className="profile-rank">{scoreProfile.rank}</div>
                                );
                            })}
                        </div>
                    </div>   
                </div>
            </div>
        </div>
    );
};