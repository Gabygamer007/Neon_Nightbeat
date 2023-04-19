using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
    private KeyCode[] defaultKeys = new KeyCode[4];
    private TMP_Text[] buttonTexts;

    private int buttonIndex = 0;
    private bool isWaitingForInput = false;
    private KeyCode key;

    void Start()
    {
        buttonTexts = GetComponentsInChildren<TMP_Text>();
        for (int i = 0; i < 4; i++)
        {
            defaultKeys[i] = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("touche" + (i + 1), "None"));
        }
    }

    public void WaitForInput(int index)
    {
        isWaitingForInput = true;
        buttonIndex = index;
        buttonTexts[buttonIndex].text = "...";
    }
    private bool IsKeyAlreadyBound(KeyCode key)
    {
        foreach (KeyCode keyCode in defaultKeys)
        {
            if (keyCode == key)
            {
                return true;
            }
        }
        return false;
    }

    private void Update()
    {
        if (isWaitingForInput)
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i) && !IsKeyAlreadyBound((KeyCode)i))
                    {
                        key = (KeyCode)i;
                        isWaitingForInput = false;
                        buttonTexts[buttonIndex].text = key.ToString();
                        string touche = string.Concat("touche" + (buttonIndex + 1));
                        PlayerPrefs.SetString(touche, key.ToString());
                        break;
                    }
                }
            }
            else
            {
                key = defaultKeys[buttonIndex];
                buttonTexts[buttonIndex].text = defaultKeys[buttonIndex].ToString();
                isWaitingForInput = false;
            }
        }
    }
}
