/* 
 - Nom du fichier : users
 - Contexte : requêtes pour les users
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

import express from 'express';
import jwt from 'jsonwebtoken';
import bcrypt from 'bcrypt';
import { UserModel } from '../models/Users.js';

const router = express.Router();


router.post("/register_post", async (req, res) => {
    const {username, password} = req.body;
    const user = await UserModel.findOne({username: username});

    if (user) {
        return res.json({message: "User already exist!"});
    }

    const hashedPassword = await bcrypt.hash(password, 10);

    const newUser = new UserModel({username, password: hashedPassword});
    await newUser.save();

    res.json({message: "User registered successfully!"});
});

router.post("/login_post", async (req, res) => {
    const {username, password} = req.body;
    const user = await UserModel.findOne({username: username});

    if (!user) {
        return res.json({message: "User doesn't exist!"});
    }

    const isPasswordValid = await bcrypt.compare(password, user.password);

    if (!isPasswordValid) {
        return res.json({message: "Username or password is incorrect!"});
    }

    const token = jwt.sign({id: user._id}, "secret");
    res.json({token, userID: user._id});
});

router.get("/getUser/:id", async (req, res) => {
    const userId = req.params.id;
    const user = await UserModel.findById(userId);
    return res.json(user.username);
});

export {router as UserRouter};