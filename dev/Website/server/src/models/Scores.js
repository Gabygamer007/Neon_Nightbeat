/* 
 - Nom du fichier : Scores
 - Contexte : Modèle des scores pour la base de données
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

import mongoose from "mongoose";

const ScoreSchema = new mongoose.Schema({
    username: {type: String, required:true},
    music: {type: String, required:true},
    score: {type: Number, required:true},
    accuracy: {type: Number, required:true},
    highestCombo: {type: Number, required:true},
    rank: {type: String, required:true}
});

export const ScoreModel = mongoose.model("scores", ScoreSchema);