using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button logOutButton;
    void Start()
    {
        if (!PlayerPrefs.HasKey("touche1")) { // Si il n'a pas le playerprefs touche1, c'est qu'il n'a normalement aucun playerprefs
            SavePrefs();
        }
        if (PlayerPrefs.HasKey("username")){ // S'il n'est pas connecté
            logOutButton.gameObject.SetActive(true);
        }
    }

    public void SavePrefs() // PlayerPrefs est basé dans le registry de l'ordinateur, donc réinstaller le jeu ne change rien aux settings
    {
        PlayerPrefs.SetString("touche1", "A");
        PlayerPrefs.SetString("touche2", "S");
        PlayerPrefs.SetString("touche3", "D");
        PlayerPrefs.SetString("touche4", "F");
        PlayerPrefs.SetString("couleurExt", "1,0;0,067;0,47");
        PlayerPrefs.SetString("couleurInt", "0,004;1,0;0,957");
        PlayerPrefs.SetInt("volume", 50);
        PlayerPrefs.Save();
    }

    public void PlayGame()
    {
        if(PlayerPrefs.GetString("username") != "")
        {
            SceneManager.LoadScene("GameMenu");
        }
        else
        {
            SceneManager.LoadScene("LoginMenu");
        }
        
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("username");
        logOutButton.gameObject.SetActive(false);
    }
}
