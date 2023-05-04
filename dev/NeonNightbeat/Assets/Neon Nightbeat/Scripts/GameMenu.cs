using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string music = "Imagine_Dragons_Warriors";
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
        music = "Imagine_Dragons_Warriors";
    }

    public void Music2()
    {
        music = "Departure";
    }

}
