import mongoose from "mongoose";

const UserSchema = new mongoose.Schema({
    username: {type: String, required:true, unique:true},
    password: {type:String, required:true}
});

export const UserModel = mongoose.model("users", UserSchema);

export const verifyToken = (req, res, next) => {
    const token = req.header.authorization;
    if (token) {
        jwt.verify(token, "secret", (err) => {
            if (err) return res.sendStatus(403);
            next();
        });
    } else {
        res.sendStatus(401);
    }
}