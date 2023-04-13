using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        if (!PlayerPrefs.HasKey("touche1"))
            SavePrefs();
    }

    public void SavePrefs()
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
        SceneManager.LoadScene("GameMenu");
    }

    public void GoToSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
