import {Link, useNavigate} from "react-router-dom";
import { useCookies } from "react-cookie";
import { useState, useEffect } from "react";
import axios from "axios";

export const Leaderboard = () => {
    // eslint-disable-next-line
    const [cookies, setCookies] = useCookies(["access_token"]);
    const navigate = useNavigate();

    const logout = () => {
        setCookies("access_token", "");
        window.localStorage.removeItem("UserID");
        navigate("/login");
    }

    const [scoreList, setScoreList] = useState([]);

    useEffect(() => {
        axios.get("http://localhost:3001/leaderboard/scores").then((response) => {
            const sortedScores = response.data.sort((a, b) => b.score - a.score);
            setScoreList(sortedScores);
        });
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
            <div className="leaderboard">
                <h2 className="leaderboard-main-title">LEADERBOARD</h2>
                <div className="main-leaderboard-box">
                    <div className="leaderboard-titles">
                        <div className="username-title">USERNAME</div>
                        <div className="score-title">SCORE</div>
                        <div className="accuracy-title">ACCURACY</div>
                        <div className="combo-title">COMBO</div>
                        <div className="rank-title">RANK</div>
                    </div>
                    <div className="leaderboard-box">
                        <div className="username-box">
                            {scoreList.map((scoreLeaderboard) => {
                                return (
                                    <div className="leaderboard-username">{scoreLeaderboard.username}</div>
                                );
                            })}

                        </div>
                        <div className="score-box">
                            {scoreList.map((scoreLeaderboard) => {
                                return (
                                    <div className="leaderboard-score">{scoreLeaderboard.score}</div>
                                );
                            })}
                        </div>
                        <div className="accuracy-box">
                            {scoreList.map((scoreLeaderboard) => {
                                return (
                                    <div className="leaderboard-accuracy">{scoreLeaderboard.accuracy.toFixed(2)} %</div>
                                );
                            })}
                        </div>
                        <div className="highest-combo-box">
                            {scoreList.map((scoreLeaderboard) => {
                                return (
                                    <div className="leaderboard-combo">{scoreLeaderboard.highestCombo}x</div>
                                );
                            })}
                        </div>
                        <div className="rank-box">
                            {scoreList.map((scoreLeaderboard) => {
                                return (
                                    <div className="leaderboard-rank">{scoreLeaderboard.rank}</div>
                                );
                            })}
                        </div>
                    </div>   
                </div>
            </div>
        </div>
    );
};