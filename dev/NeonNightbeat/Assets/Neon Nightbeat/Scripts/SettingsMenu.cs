using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class SettingsMenu : MonoBehaviour
{
    public TMP_Text valeurVolume;
    public Slider sliderVolume;
    public Canvas conteneurTouches;

    public TMP_Text[] texteBoutons;
    public string[] valeursBoutons;

    private void Start()
    {
        texteBoutons = conteneurTouches.GetComponentsInChildren<TMP_Text>();
        valeursBoutons = texteBoutons.Select(button => button.text).ToArray();
        LoadPrefs();
    }

    public void SetVolume (float volume)
    { 
        valeurVolume.text = Mathf.RoundToInt(volume).ToString() + "%";
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPrefs()
    {
        valeurVolume.text = PlayerPrefs.GetInt("volume").ToString() + "%";
        texteBoutons[0].text = PlayerPrefs.GetString("touche1");
        texteBoutons[1].text = PlayerPrefs.GetString("touche2");
        texteBoutons[2].text = PlayerPrefs.GetString("touche3");
        texteBoutons[3].text = PlayerPrefs.GetString("touche4");
        Debug.Log(PlayerPrefs.GetString("touche4"));
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("volume", Mathf.RoundToInt(sliderVolume.value));
        PlayerPrefs.SetString("touche1", valeursBoutons[0].ToString());
        PlayerPrefs.SetString("touche2", valeursBoutons[1].ToString());
        PlayerPrefs.SetString("touche3", valeursBoutons[2].ToString());
        PlayerPrefs.SetString("touche4", valeursBoutons[3].ToString());
        //PlayerPrefs.SetString("couleurExt", "1,0;0,067;0,47");
        //PlayerPrefs.SetString("couleurInt", "0,004;1,0;0,957");
    }
}
