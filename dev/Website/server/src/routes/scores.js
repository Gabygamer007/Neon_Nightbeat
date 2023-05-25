import express from 'express';
import { ScoreModel } from '../models/Scores.js';

const router = express.Router();

router.get("/scores", async (req, res) => {
    try {
        const response = await ScoreModel.find({});
        res.json(response);
    } catch (err) {
        res.json(err);
    }
});

export {router as ScoreRouter};