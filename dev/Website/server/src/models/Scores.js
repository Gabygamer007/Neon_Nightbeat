import mongoose from "mongoose";

const ScoreSchema = new mongoose.Schema({
    username: {type: String, required:true},
    score: {type: Number, required:true}
});

export const ScoreModel = mongoose.model("scores", ScoreSchema);