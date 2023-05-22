import express from "express";
import cors from 'cors';
import mongoose from 'mongoose';

import {UserRouter} from './routes/users.js';

const app = express();

app.use(express.json());
app.use(cors());

app.use("/login", UserRouter);
app.use("/register", UserRouter);

mongoose.connect("mongodb+srv://matisgaetjens:3lie2oo9@neonnightbeatdb.ya38q6y.mongodb.net/NeonNightbeatDB?retryWrites=true&w=majority")

app.listen(3001, () => console.log("SERVER STARTED!"));



