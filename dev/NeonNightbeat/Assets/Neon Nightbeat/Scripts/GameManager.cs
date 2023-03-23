using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    public AudioSource theMusic;

    public bool startPlaying;

    public BeatScroller theBS;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown)
            {
                startPlaying = true;
                Destroy(GameObject.Find("TextStart"));
                theBS.hasStarted = true;

                theMusic.Play();
            }
        }
    }
}
