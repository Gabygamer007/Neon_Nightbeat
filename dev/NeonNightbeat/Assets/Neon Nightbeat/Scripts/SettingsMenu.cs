using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* 
 - Nom du fichier : SettingsMenu
 - Contexte : Menu pour les "settings"
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class SettingsMenu : MonoBehaviour
{
    public TMP_Text valeurVolume;
    public Slider sliderVolume;
    public Canvas conteneurTouches;
    public Canvas canvasCouleurExt;
    public Canvas canvasCouleurInt;
    private SpriteRenderer spriteRendererCouleurExt;
    private SpriteRenderer spriteRendererCouleurInt;

    private TMP_Text[] texteBoutons;

    private void Start()
    {
        texteBoutons = conteneurTouches.GetComponentsInChildren<TMP_Text>();
        GameObject exempleCouleurExt = canvasCouleurExt.transform.Find("couleurExt").gameObject;
        spriteRendererCouleurExt = exempleCouleurExt.GetComponentInChildren<SpriteRenderer>();
        GameObject exempleCouleurInt = canvasCouleurInt.transform.Find("couleurInt").gameObject;
        spriteRendererCouleurInt = exempleCouleurInt.GetComponentInChildren<SpriteRenderer>();
        LoadPrefs();
    }

    public void SetVolume (float volume)
    { 
        valeurVolume.text = Mathf.RoundToInt(volume).ToString() + "%";
        PlayerPrefs.SetInt("volume", Mathf.RoundToInt(sliderVolume.value));
    }

    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadPrefs()
    {
        valeurVolume.text = PlayerPrefs.GetInt("volume").ToString() + "%";
        sliderVolume.value = PlayerPrefs.GetInt("volume");
        texteBoutons[0].text = PlayerPrefs.GetString("touche1");
        texteBoutons[1].text = PlayerPrefs.GetString("touche2");
        texteBoutons[2].text = PlayerPrefs.GetString("touche3");
        texteBoutons[3].text = PlayerPrefs.GetString("touche4");
        var colorValues = PlayerPrefs.GetString("couleurExt").Split(';');
        spriteRendererCouleurExt.color = new Color(float.Parse(colorValues[0]), float.Parse(colorValues[1]), float.Parse(colorValues[2]));
        colorValues = PlayerPrefs.GetString("couleurInt").Split(';');
        spriteRendererCouleurInt.color = new Color(float.Parse(colorValues[0]), float.Parse(colorValues[1]), float.Parse(colorValues[2]));
    }

    public void choisirCouleur(Transform parentCouleur)
    {
        GameObject buttonObj = EventSystem.current.currentSelectedGameObject;
        Button button = buttonObj.GetComponent<Button>();
        Color couleurBouton = button.GetComponent<Image>().color;
        parentCouleur.GetComponent<SpriteRenderer>().color = couleurBouton;
        PlayerPrefs.SetString(parentCouleur.name, couleurBouton.r.ToString() + ";" + couleurBouton.g.ToString() + ";" + couleurBouton.b.ToString());
    }
}
