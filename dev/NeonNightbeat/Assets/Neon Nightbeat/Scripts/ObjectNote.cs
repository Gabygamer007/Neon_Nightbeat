using UnityEngine;
using System;

/* 
 - Nom du fichier : ObjectNote
 - Contexte : Vérification lorsqu'on clique sur un note pour sa précision
 - Auteurs : Matis Gaetjens et Gabriel Tremblay 
*/

public class ObjectNote : MonoBehaviour
{
    public event Action NoteMissed;
    public event Action NotePerfect;
    public event Action NoteGood;
    public event Action NoteBad;

    public bool canBePressed;

    public KeyCode keyToPress;
    
    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(keyToPress)){
            if (canBePressed)
            {

                if (Mathf.Abs(transform.position.y) < 3.4 || Mathf.Abs(transform.position.y) > 4.6)
                {
                    GameManager.instance.BadHit();
                    NoteBad?.Invoke();
                }
                else if (Mathf.Abs(transform.position.y) < 3.80 || Mathf.Abs(transform.position.y) > 4.2)
                {
                    GameManager.instance.GoodHit();
                    NoteGood?.Invoke();
                }
                else
                {
                    GameManager.instance.PerfectHit();
                    NotePerfect?.Invoke();
                }
                gameObject.SetActive(false);
            } 
        }
    }

    public void SetColor(Color color)
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Receptor"))
        {
            canBePressed = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Receptor") && gameObject.activeSelf)
        {
            NoteMissed?.Invoke();
            canBePressed = false;
            GameManager.instance.NoteMissed(); 
        }

    }
}
