using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MongoDB.Driver;
using MongoDB.Bson;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenu : MonoBehaviour
{
    private DatabaseAccess db;
    public TMP_InputField textUsername;
    public TMP_InputField textPassword;
    public TMP_Text textInvalidUser;

    void Start()
    {
        db = new DatabaseAccess();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressedLogin()
    {
        string username = textUsername.text;
        string password = textPassword.text;
        if (db.CheckUserCredentials(username, password))
        {
            PlayerPrefs.SetString("username", username);
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            textInvalidUser.gameObject.SetActive(true);
            StartCoroutine(RemoveText());
        }
    }
    
    IEnumerator RemoveText()
    {
        yield return new WaitForSeconds(2f);
        textInvalidUser.gameObject.SetActive(false);
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
