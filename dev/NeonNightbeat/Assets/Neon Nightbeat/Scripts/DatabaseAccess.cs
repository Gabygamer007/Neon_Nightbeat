using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using BCryptNet = BCrypt.Net;
public class DatabaseAccess
{
    private string connectionString = "mongodb+srv://matisgaetjens:3lie2oo9@neonnightbeatdb.ya38q6y.mongodb.net/NeonNightbeatDB?retryWrites=true&w=majority";
    private MongoClient client;
    private IMongoDatabase database;
    private IMongoCollection<BsonDocument> collectionUsers;
    private IMongoCollection<BsonDocument> collectionScores;
    public DatabaseAccess()
    {
        client = new MongoClient(connectionString);
        database = client.GetDatabase("NeonNightbeatDB");
        collectionUsers = database.GetCollection<BsonDocument>("users");
        collectionScores = database.GetCollection<BsonDocument>("scores");
    }

    public bool CheckUserCredentials(string username, string password)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("username", username);
        var result = collectionUsers.Find(filter).FirstOrDefault();

        if (result != null)
        {
            string hashedPassword = result.GetValue("password").AsString;
            bool passwordMatch = BCryptNet.BCrypt.Verify(password, hashedPassword);
            return passwordMatch;
        }

        return false;
    }

    public void EnterScores(string username, string musicName,  int score, double accuracy, int highestCombo, string rank)
    {
        BsonDocument document = new BsonDocument
        {
            { "username", username },
            { "music", musicName },
            { "score", score },
            { "accuracy", accuracy },
            { "highestCombo", highestCombo },
            { "rank", rank }
        };
        collectionScores.InsertOne(document);
    }
}