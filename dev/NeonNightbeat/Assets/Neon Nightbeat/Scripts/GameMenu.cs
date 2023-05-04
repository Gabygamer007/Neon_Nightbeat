using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string music = "";
    public int tempo = 0;
    public static GameMenu instance;

    void Start()
    {
        instance = this;
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayMusic()
    {
        if (music != "0")
        {
            SceneManager.LoadScene("PlayingGame");
        }
    }

    public void Music1()
    {
        music = "Imagine_Dragons_Warriors";
        tempo = 234;
    }

    public void Music2()
    {
        music = "Silhouette";
        tempo = 273;
    }

    public void Music3()
    {
        music = "Ransom";
        tempo = 270;
    }

}
