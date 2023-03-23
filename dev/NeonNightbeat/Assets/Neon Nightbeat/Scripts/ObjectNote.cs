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
            } 
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Receptor")
        {
            canBePressed = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Receptor")
        {
            canBePressed = false;
        }

    }
}
