using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectNote : MonoBehaviour
{
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
                gameObject.SetActive(false);

                if (Mathf.Abs(transform.position.y) < 3.5 || Mathf.Abs(transform.position.y) > 4.5)
                {
                    GameManager.instance.BadHit();
                }
                else if (Mathf.Abs(transform.position.y) < 3.75 || Mathf.Abs(transform.position.y) > 4.25)
                {
                    GameManager.instance.NormalHit();
                }
                else if (Mathf.Abs(transform.position.y) < 3.9 || Mathf.Abs(transform.position.y) > 4.1)
                {
                    GameManager.instance.GoodHit();
                }
                else
                {
                    GameManager.instance.PerfectHit();
                }
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
            canBePressed = false;
            GameManager.instance.NoteMissed();
        }

    }
}
