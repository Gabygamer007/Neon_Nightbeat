using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;

    [SerializeField]
    public GameObject prefabNote;

    public Transform conteneurRecepteurs;
    private Transform[] recepteurs = new Transform[4];

    public AudioSource theMusic;
    public bool startPlaying;
    private bool canStart = false;
    public BeatScroller beatScroller;
    public static GameManager instance;
    private bool gamePaused = false;
    private bool gameUnpausing = false;

    private int currentScore;
    private int scorePerNote = 100;
    public TMP_Text scoreText;
    private int currentCombo;
    public TMP_Text currentcomboText;
    public TMP_Text comboText;
    private int currentMultiplier = 0;

    private int badScore;
    private int goodScore;
    private int perfectScore;

    List<double> listAccuracy;
    double accuracy = 0;
    public TMP_Text accuracyText;

    public TMP_Text hitText;

    public GameObject resultsScreen;
    public TMP_Text badHitText, goodHitText, perfectHitText, missText, rankText;
    private int nbBadHit = 0;
    private int nbGoodHit = 0;
    private int nbPerfectHit = 0;
    private int nbMiss = 0;
    public TMP_Text finalScoreText;
    public TMP_Text textCountdown;
    private List<Transform> notes = new List<Transform>();

    private bool speedUp = false;

    void Start()
    {
        instance = this;

        scoreText.text = currentScore.ToString();
        currentcomboText.text = "";
        comboText.text = "";
        badScore = scorePerNote / 2;
        goodScore = scorePerNote;
        perfectScore = scorePerNote + (scorePerNote / 2);

        listAccuracy = new List<double>();

        text.text = "Loading...";
        MusicNoteFactory factory = new MusicNoteFactory();

        TextAsset csv = Resources.Load<TextAsset>(GameMenu.instance.music);

        ButtonController scriptBouton;

        for (int i = 0; i < 4; i++)
        {
            scriptBouton = conteneurRecepteurs.GetChild(i).gameObject.GetComponent<ButtonController>();
            scriptBouton.keyToPress = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("touche" + (i + 1)));
        }
        
        using (var reader = new StreamReader(new MemoryStream(csv.bytes)))
        {
            string touche = "";
            while (!reader.EndOfStream)
            {
                Vector3 position = new Vector3(0, 0, 1);
                Color couleur = new Color();
                var line = reader.ReadLine();
                var values = line.Split(';');
                position.x = (int.Parse(values[1]) * 2) - 5;
                if (float.TryParse(values[2], out float result))
                {
                    position.y = result;
                }
                if (values[1] == "1" || values[1] == "4")
                {
                    var colorValues = PlayerPrefs.GetString("couleurExt").Split(';');
                    couleur = new Color(float.Parse(colorValues[0]), float.Parse(colorValues[1]), float.Parse(colorValues[2]));
                }
                else if (values[1] == "2" || values[1] == "3")
                {
                    var colorValues = PlayerPrefs.GetString("couleurInt").Split(';');
                    couleur = new Color(float.Parse(colorValues[0]), float.Parse(colorValues[1]), float.Parse(colorValues[2]));
                }
                touche = PlayerPrefs.GetString("touche" + values[1]);
                factory.CreateMusicNote(prefabNote, position, couleur, (KeyCode)System.Enum.Parse(typeof(KeyCode), touche));
            }
        }

        theMusic.clip = (AudioClip)Resources.Load(GameMenu.instance.music, typeof(AudioClip));
        theMusic.volume = PlayerPrefs.GetInt("volume")/100.0f;

        notes = new List<Transform>(beatScroller.GetComponentsInChildren<Transform>());
        notes.Remove(beatScroller.transform);

        text.text = "Press any key to start";
        canStart = true;

        beatScroller.beatTempo = GameMenu.instance.tempo/60f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!startPlaying)
        {
            if (Input.anyKeyDown && canStart)
            {
                startPlaying = true;
                text.text = "";
                beatScroller.hasStarted = true;
                theMusic.Play();
            }
        }
        else if (gamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gameUnpausing)
                {
                    StartCoroutine(UnPause());
                    gameUnpausing = true;
                }
            }
            else if (Input.GetKeyDown(KeyCode.Space) && !gameUnpausing)
            {
                SceneManager.LoadScene("GameMenu");
            }
            
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gamePaused = true;
                beatScroller.hasStarted = false;
                theMusic.Pause();
                text.text = "Game paused \n Press Escape to unpause \n Press Space to leave";
                EnabledDisableNotes(!gamePaused);
                
            }
            if (!theMusic.isPlaying && !resultsScreen.activeInHierarchy &&  !gamePaused)
            {
                resultsScreen.SetActive(true);

                badHitText.text = nbBadHit.ToString();
                goodHitText.text = nbGoodHit.ToString();
                perfectHitText.text = nbPerfectHit.ToString();
                missText.text = nbMiss.ToString();
            }
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (!speedUp)
            //    {
            //        beatScroller.beatTempo *= 10;
            //        theMusic.pitch *= 10.0f;
            //        speedUp = true;
            //    }
            //    else
            //    {
            //        beatScroller.beatTempo /= 10;
            //        theMusic.pitch /= 10.0f;
            //        speedUp = false;
            //    }
            //
            //}


            // Change le rank en fonction de l'accuracy
            string rank = "F";

            if (accuracy > 40)
            {
                rank = "D";
                if (accuracy > 55)
                {
                    rank = "C";
                    if (accuracy > 75)
                    {
                        rank = "B";
                        if (accuracy > 85)
                        {
                            rank = "A";
                            if (accuracy > 95)
                            {
                                rank = "S";
                                if (accuracy == 100)
                                {
                                    rank = "SS";
                                }
                            }
                        }
                    }
                }
            }

            rankText.text = rank;

            finalScoreText.text = currentScore.ToString();
        }
        if (!gamePaused && !gameUnpausing)
        {
            foreach (Transform note in notes)
            {
                if (note.transform.position.y > 6f)
                {
                    note.gameObject.SetActive(false);
                }
                else if (note.transform.position.y < 6.0f && note.transform.position.y > 4.99f)
                {
                    note.gameObject.SetActive(true);
                }
                else if (note.transform.position.y < -6f)
                {
                    note.gameObject.SetActive(false);
                }
            }
        }
        
    }

    public void NoteHit()
    {
        scoreText.text = currentScore.ToString();
        currentCombo++;
        currentcomboText.text = currentCombo.ToString();
        comboText.text = "COMBO";

        currentMultiplier = Mathf.FloorToInt(Mathf.Sqrt(currentCombo));
        UpdateAccuracy();
    }

    public void BadHit()
    {
        currentScore += badScore + ((badScore / 10) * currentMultiplier);
        listAccuracy.Add(33.33);
        NoteHit();
        hitText.text = "BAD";
        hitText.color = Color.yellow;
        nbBadHit++;
    }

    public void GoodHit()
    {
        currentScore += goodScore + ((goodScore / 10) * currentMultiplier);
        listAccuracy.Add(66.66);
        NoteHit();
        hitText.text = "GOOD";
        hitText.color = Color.cyan;
        nbGoodHit++;
    }

    public void PerfectHit()
    {
        currentScore += perfectScore + ((perfectScore / 10) * currentMultiplier);
        listAccuracy.Add(100.00);
        NoteHit();
        hitText.text = "PERFECT!";
        hitText.color = Color.green;
        nbPerfectHit++;
    }

    public void NoteMissed()
    {
        currentCombo = 0;
        currentcomboText.text = currentCombo.ToString();
        comboText.text = "COMBO";
        currentMultiplier = 0;
        hitText.text = "MISS";
        hitText.color = Color.red;
        listAccuracy.Add(0);
        UpdateAccuracy();
        nbMiss++;
    }

    public void UpdateAccuracy()
    {
        for (int i = 0; i < listAccuracy.Count; i++)
        {
            accuracy = 0;
            accuracy += listAccuracy.Sum();
            accuracy /= listAccuracy.Count;
            accuracyText.text = accuracy.ToString("F2") + " %";
        }
    }

    public void EnabledDisableNotes(bool enable)
    {
        foreach (Transform note in beatScroller.transform)
        {
            ObjectNote[] scripts = note.GetComponents<ObjectNote>();
            foreach (ObjectNote script in scripts)
            {
                script.enabled = enable;
            }
        }
    }

    IEnumerator UnPause()
    {
        text.text = "Starting in";
        textCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);
        textCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);
        textCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);
        textCountdown.text = "";
        text.text = "";
        theMusic.UnPause();
        beatScroller.hasStarted = true;
        gamePaused = false;
        gameUnpausing = false;
        EnabledDisableNotes(!gamePaused);
    }
    
}