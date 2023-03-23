using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private Color baseColor;
    private Color pressedColor;

    public KeyCode keyToPress;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;
        pressedColor = baseColor;
        pressedColor.a = 1f;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            spriteRenderer.color = pressedColor;
        }

        if (Input.GetKeyUp(keyToPress))
        {
            spriteRenderer.color = baseColor;
        }


    }
}
