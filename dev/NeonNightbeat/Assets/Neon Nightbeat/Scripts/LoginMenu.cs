using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

/* 
 - Nom du fichier : LoginMenu
 - Contexte : Si les "credentials" sont bon, on "set" son "username" et on change de scène
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class LoginMenu : MonoBehaviour
{
    private DatabaseAccess db;
    public TMP_InputField inputUsername;
    public TMP_InputField inputPassword;
    public TMP_Text textInvalidUser;

    void Start()
    {
        db = new DatabaseAccess();
    }

    public void PressedLogin()
    {
        string username = inputUsername.text;
        string password = inputPassword.text;
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
