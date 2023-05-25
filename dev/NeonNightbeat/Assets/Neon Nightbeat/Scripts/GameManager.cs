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
    private int scoreMultiplier;
    private bool ghostMode;

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
    public GameObject ghostPanel;
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
    public Transform prefabObserverEffect;
    private string musicName;

    private StateMachine theStateMachine;

    void Start() // tout initialiser quand on arrive dans la scene "PlayingGame"
    {
        instance = this;

        scoreText.text = currentScore.ToString();
        currentcomboText.text = "";
        comboText.text = "";
        badScore = scorePerNote / 2;
        goodScore = scorePerNote;
        perfectScore = scorePerNote + (scorePerNote / 2);

        scoreMultiplier = GameMenu.instance.multiplier;
        ghostMode = GameMenu.instance.ghostMode;
        musicName = GameMenu.instance.music;

        listAccuracy = new List<double>();

        text.text = "Loading...";
        MusicNoteFactory factory = new MusicNoteFactory();

        TextAsset csv = Resources.Load<TextAsset>(musicName);

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

        theMusic.clip = (AudioClip)Resources.Load(musicName, typeof(AudioClip));
        theMusic.volume = PlayerPrefs.GetInt("volume")/100.0f;

        notes = new List<Transform>(beatScroller.GetComponentsInChildren<Transform>());
        notes.Remove(beatScroller.transform);

        foreach (Transform note in notes)// donner un observer sur chaque objet note et initialiser les variables dans l'observer
        {
            Observer observer = note.gameObject.AddComponent<Observer>();
            observer.subjectToObserve = note.GetComponent<ObjectNote>();
            observer.prefabEffect = prefabObserverEffect;
        }


        text.text = "Press any key to start";

        beatScroller.beatTempo = GameMenu.instance.tempo/60f;

        theStateMachine = new StateMachine();
        theStateMachine.Init(new WaitForStart());
    }

    // Update is called once per frame
    void Update()
    {   
        theStateMachine.Update(); // changer les etats dans la machine d'etats
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
        currentScore += (badScore + ((badScore / 10) * currentMultiplier)) * scoreMultiplier;
        listAccuracy.Add(33.33);
        NoteHit();
        hitText.text = "BAD";
        hitText.color = Color.yellow;
        nbBadHit++;
    }

    public void GoodHit()
    {
        currentScore += (goodScore + ((goodScore / 10) * currentMultiplier)) * scoreMultiplier;
        listAccuracy.Add(66.66);
        NoteHit();
        hitText.text = "GOOD";
        hitText.color = Color.cyan;
        nbGoodHit++;
    }

    public void PerfectHit()
    {
        currentScore += (perfectScore + ((perfectScore / 10) * currentMultiplier)) * scoreMultiplier;
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

    public void EnabledDisableNotes(bool enable) // ne pas pouvoir appuyer sur les notes quand on pause
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
    }

    public AudioSource TheMusic
    {
        get { return theMusic; }
    }

    public StateMachine TheStateMachine
    {
        get { return theStateMachine; }
    }

    public int NbBadHit{
        get { return nbBadHit; }
    }
    public int NbGoodHit {
        get { return nbGoodHit; }
    }
    public int NbPerfectHit{
        get { return nbPerfectHit; }
    }
    public int NbMiss{
        get { return nbMiss; }
    }

    public double Accuracy
    {
        get { return accuracy; }
    }

    public int CurrentScore
    {
        get { return currentScore; }
    }

    public int HighestCombo
    {
        get { return highestCombo; }
    }

    public List<Transform> Notes
    {
        get { return notes; }
    }

    public double ScoreMultiplier
    {
        get { return scoreMultiplier; }
    }

    public bool IsGhostMode
    {
        get { return ghostMode; }
    }

    public string MusicName
    {
        get { return musicName; }
    }
}