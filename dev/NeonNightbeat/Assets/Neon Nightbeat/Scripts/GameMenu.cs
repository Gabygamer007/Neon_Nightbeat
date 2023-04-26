using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public int music = 0;
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
        SceneManager.LoadScene("PlayingGame");
    }

    public void Music1()
    {
        music = 1;
    }

    public void Music2()
    {
        music = 2;
    }

}
