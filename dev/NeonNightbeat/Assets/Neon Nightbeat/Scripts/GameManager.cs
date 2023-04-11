using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    public GameObject prefabNote;

    public AudioSource theMusic;

    public bool startPlaying;

    private bool canStart = false;

    public BeatScroller theBS;

    void Start()
    {
        text.text = "Loading...";
        MusicNoteFactory factory = new MusicNoteFactory();
        factory.CreateMusicNote(prefabNote, new Vector3(-3.0f, 22.56f), new Color(255f/255f, 17f/255f, 120f/255f), KeyCode.A);
        text.text = "Press any key to start";
        canStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown && canStart)
            {
                startPlaying = true;
                Destroy(GameObject.Find("TextStart"));
                theBS.hasStarted = true;

                theMusic.Play();
            }
        }
    }

    
}