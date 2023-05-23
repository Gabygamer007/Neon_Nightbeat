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
    private GameObject prefabNote;

    public Transform conteneurRecepteurs;

    public AudioSource theMusic;
    public BeatScroller beatScroller;
    public static GameManager instance;

    private int currentScore;
    private int scorePerNote = 300;
    public TMP_Text scoreText;
    private int currentCombo;
    public TMP_Text currentcomboText;
    public TMP_Text comboText;
    private int currentMultiplier = 0;
    private int highestCombo = 0;
    public TMP_Text highestComboText;

    private int badScore;
    private int goodScore;
    private int perfectScore;

    List<double> listAccuracy;
    double accuracy = 0;
    public TMP_Text accuracyText;
    public TMP_Text accuracyResult;

    public TMP_Text hitText;

    public GameObject resultsScreen;
    public GameObject gameScreen;
    public GameObject recepteursButtons;
    public TMP_Text badHitText, goodHitText, perfectHitText, missText, rankText;
    private int nbBadHit = 0;
    private int nbGoodHit = 0;
    private int nbPerfectHit = 0;
    private int nbMiss = 0;
    public TMP_Text finalScoreText;
    public TMP_Text textCountdown;
    private List<Transform> notes = new List<Transform>();
    public bool speedUp = false;

    private StateMachine theStateMachine;

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

        beatScroller.beatTempo = GameMenu.instance.tempo/60f;

        theStateMachine = new StateMachine();
        theStateMachine.Init(new WaitForStart());
    }

    // Update is called once per frame
    void Update()
    {   
        theStateMachine.Update();
    }

    public void NoteHit()
    {
        scoreText.text = currentScore.ToString();
        currentCombo++;
        currentcomboText.text = currentCombo.ToString();
        comboText.text = "COMBO";

        currentMultiplier = Mathf.FloorToInt(Mathf.Sqrt(currentCombo));
        UpdateAccuracy();

        if (highestCombo < currentCombo)
        {
            highestCombo = currentCombo;
        }
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

    public void GoBack()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public TMP_Text Text
    {
        get { return text; }
        set { text = value; }
    }

    public BeatScroller BeatScroller
    {
        get { return beatScroller; }
        set { beatScroller = value; }
    }

    public AudioSource TheMusic
    {
        get { return theMusic; }
        set { theMusic = value; }
    }

    public StateMachine TheStateMachine
    {
        get { return theStateMachine; }
        set { theStateMachine = value; }
    }

    public int NbBadHit{
        get { return nbBadHit; }
        set { nbBadHit = value; }
    }
    public int NbGoodHit {
        get { return nbGoodHit; }
        set { nbBadHit = value; }
    }
    public int NbPerfectHit{
        get { return nbPerfectHit; }
        set { nbBadHit = value; }
    }
    public int NbMiss{
        get { return nbMiss; }
        set { nbBadHit = value; }
    }

    public double Accuracy
    {
        get { return accuracy; }
        set { accuracy = value; }
    }

    public int CurrentScore
    {
        get { return currentScore; }
        set { currentScore = value; }
    }

    public int HighestCombo
    {
        get { return highestCombo; }
        set { highestCombo = value;}
    }

    public List<Transform> Notes
    {
        get { return notes; }
    }
}