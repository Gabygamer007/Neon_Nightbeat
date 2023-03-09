using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayMusic()
    {
        SceneManager.LoadScene("PlayingGame");
    }
    
}
