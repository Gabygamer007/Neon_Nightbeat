using UnityEngine;

/* 
 - Nom du fichier : ButtonController
 - Contexte : Change la couleur des récepteurs lorsqu'on clique dessus avec la touche prédéfini
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class ButtonController : MonoBehaviour // Un script qui ne fait que changer la couleur des boutons quand on appuie sur les touches compatibles
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
