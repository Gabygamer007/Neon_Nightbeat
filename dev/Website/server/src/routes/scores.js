import express from 'express';
import { ScoreModel } from '../models/Scores.js';
import { UserModel } from '../models/Users.js';

const router = express.Router();

router.get("/scores_leaderboard", async (req, res) => {
    try {
        const response = await ScoreModel.find({});
        res.json(response);
    } catch (err) {
        res.json(err);
    }
});

router.get("/scores_profile/:id", async (req, res) => {
    try {
        const userId = req.params.id;
        const user = await UserModel.findById(userId);
        const response = await ScoreModel.find({username : user.username});
        res.json(response);
    } catch (err) {
        res.json(err);
    }
});

export {router as ScoreRouter};