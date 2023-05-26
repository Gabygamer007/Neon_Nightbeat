/* 
 - Nom du fichier : Users
 - Contexte : Modèle des users pour la base de données
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

import mongoose from "mongoose";

const UserSchema = new mongoose.Schema({
    username: {type: String, required:true, unique:true},
    password: {type:String, required:true}
});

export const UserModel = mongoose.model("users", UserSchema);