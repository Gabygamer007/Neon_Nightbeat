using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    public static IDictionary<int, string> touches = new Dictionary<int, string>();

    void Start()
    {
        SavePrefs();
    }
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetString("touche1", "A");
        PlayerPrefs.SetString("touche2", "S");
        PlayerPrefs.SetString("touche3", "D");
        PlayerPrefs.SetString("touche4", "F");
        PlayerPrefs.SetString("couleurExt", "1,0;0,067;0,47");
        PlayerPrefs.SetString("couleurInt", "0,004;1,0;0,957");
        PlayerPrefs.Save();
    }

    public void LoadPrefs()
    {

    }
}
