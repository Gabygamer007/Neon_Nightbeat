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

    public AudioSource theMusic;
    public bool startPlaying;
    private bool canStart = false;
    public BeatScroller beatScroller;
    public static GameManager instance;
    public int currentScore;
    public int scorePerNote = 50;
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

    void Start()
    {
        instance = this;

        scoreText.text = "" + currentScore;
        currentcomboText.text = "";
        comboText.text = "";
        badScore = scorePerNote / 2;
        goodScore = scorePerNote + (scorePerNote / 2);
        perfectScore = scorePerNote * 2;

        listAccuracy = new List<double>();

        text.text = "Loading...";
        MusicNoteFactory factory = new MusicNoteFactory();

        TextAsset csv = Resources.Load<TextAsset>("Imagine_Dragons_Warriors");

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
    }

    public void NoteHit()
    {
        //Debug.Log("Hit");
        //currentScore += scorePerNote + ((scorePerNote / 10) * currentCombo);
        scoreText.text = "" + currentScore;
        currentCombo++;
        currentcomboText.text = "" + currentCombo;
        comboText.text = "COMBO";

        currentMultiplier = Mathf.FloorToInt(Mathf.Sqrt(currentCombo));

        for (int i = 0; i < listAccuracy.Count; i++)
        {
            accuracy = 0;
            accuracy += listAccuracy.Sum();
            accuracy /= listAccuracy.Count;
            accuracyText.text = decimal.Round(((decimal)accuracy), 2) + " %";
        }
    }

    public void BadHit()
    {
        currentScore += badScore + ((badScore / 10) * currentMultiplier);
        listAccuracy.Add(25);
        NoteHit();
    }

    public void NormalHit()
    {
        currentScore += scorePerNote + ((scorePerNote / 10) * currentMultiplier);
        listAccuracy.Add(50);
        NoteHit();
    }

    public void GoodHit()
    {
        currentScore += goodScore + ((goodScore / 10) * currentMultiplier);
        listAccuracy.Add(75);
        NoteHit();
    }

    public void PerfectHit()
    {
        currentScore += perfectScore + ((perfectScore / 10) * currentMultiplier);
        listAccuracy.Add(100);
        NoteHit();
    }

    public void NoteMissed()
    {
        currentCombo = 0;
        currentcomboText.text = "" + currentCombo;
        comboText.text = "COMBO";
        currentMultiplier = 0;
    }
    
}