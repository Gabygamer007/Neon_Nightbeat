using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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

    public int currentScore;
    public int scorePerNote = 100;
    public TMP_Text scoreText;
    public int currentCombo;
    public TMP_Text currentcomboText;
    public TMP_Text comboText;
    public int currentMultiplier = 0;

    public int badScore;
    public int goodScore;
    public int perfectScore;

    List<double> listAccuracy;
    double accuracy = 0;
    public TMP_Text accuracyText;

    public TMP_Text hitText;

    public GameObject resultsScreen;
    public TMP_Text badHitText, goodHitText, perfectHitText, missText, rankText;
    public int nbBadHit = 0;
    public int nbGoodHit = 0;
    public int nbPerfectHit = 0;
    public int nbMiss = 0;
    public TMP_Text finalScoreText;

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

        TextAsset csv = Resources.Load<TextAsset>("Imagine_Dragons_Warriors");

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

        theMusic.volume = PlayerPrefs.GetInt("volume")/100.0f;

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
                beatScroller.hasStarted = true;

                theMusic.Play();
            }
        }
        else
        {
            if (currentCombo == 100 && !resultsScreen.activeInHierarchy)
            {
                resultsScreen.SetActive(true);

                badHitText.text = nbBadHit.ToString();
                goodHitText.text = nbGoodHit.ToString();
                perfectHitText.text = nbPerfectHit.ToString();
                missText.text = nbMiss.ToString();
            }

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
    
}