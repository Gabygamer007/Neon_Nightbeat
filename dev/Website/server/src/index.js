/* 
 - Nom du fichier : index
 - Contexte : ficher principal de l'application
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

import express from "express";
import cors from 'cors';
import mongoose from 'mongoose';
import {UserRouter} from './routes/users.js';
import {ScoreRouter} from './routes/scores.js';

const app = express();

app.use(express.json());
app.use(cors());

app.use("/user", UserRouter);
app.use("/leaderboard", ScoreRouter);

mongoose.connect("mongodb+srv://matisgaetjens:3lie2oo9@neonnightbeatdb.ya38q6y.mongodb.net/NeonNightbeatDB?retryWrites=true&w=majority")
    .then(() => {
        app.listen(3001, () => console.log("SERVER STARTED!"));
    })
    .catch((error) => {
        console.log(error);
    })





