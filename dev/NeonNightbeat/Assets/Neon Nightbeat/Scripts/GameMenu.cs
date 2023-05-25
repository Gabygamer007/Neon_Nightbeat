using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameMenu : MonoBehaviour
{
    public string music = "";
    public int tempo = 0;
    public static GameMenu instance;
    public int multiplier;
    public bool ghostMode = false;

    [SerializeField]
    private TMP_Text multiplierText;
    [SerializeField]
    private Button ghostModeButton;

    void Start()
    {
        instance = this;
    }

    private void Update()
    {
        if (ghostMode)
        {
            ghostModeButton.GetComponent<Image>().color = Color.green;
        }
        else
        {
            ghostModeButton.GetComponent<Image>().color = Color.red;
        }

        multiplierText.text = multiplier + " x";
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
        ghostMode = false;
        multiplier = 1;
    }

    public void Music2()
    {
        music = "Silhouette";
        tempo = 637;
        ghostMode = false;
        multiplier = 3;
    }

    public void Music3()
    {
        music = "Ransom";
        tempo = 450;
        ghostMode = false;
        multiplier = 2;
    }

    public void GhostModeClicked()
    {
        if (multiplier != 0)
        {
            ghostMode = !ghostMode;
            if (ghostMode)
            {
                multiplier += 1;
            }
            else
            {
                multiplier -= 1;
            }
        }
        
    }

}
