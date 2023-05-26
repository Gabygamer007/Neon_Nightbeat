using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class BaseState // State pattern qui gère les états d'une partie
{
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();
}

public class WaitForStart : BaseState
{
    private GameManager gameInstance = GameManager.instance;
    public override void Enter()
    {

    }
    public override void Update()
    {
        if (Input.anyKeyDown)
        {
            gameInstance.Text.text = "";
            gameInstance.beatScroller.hasStarted = true;
            gameInstance.theMusic.Play();
            gameInstance.TheStateMachine.Transition(new PlayingGame());
        }
    }
    public override void Exit()
    {
        if (gameInstance.IsGhostMode == true)
        {
            gameInstance.ghostPanel.SetActive(true);
        }
    }
}

public class PlayingGame : BaseState
{
    private GameManager gameInstance = GameManager.instance;
    public override void Enter()
    {

    }
    public override void Update()
    {
        if (!gameInstance.TheMusic.isPlaying)
        {
            gameInstance.TheStateMachine.Transition(new ResultScreen());
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameInstance.TheStateMachine.Transition(new PausedState());
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameInstance.speedUp)
            {
                gameInstance.beatScroller.beatTempo *= 10;
                gameInstance.theMusic.pitch *= 10.0f;
                gameInstance.speedUp = true;
            }
            else
            {
                gameInstance.beatScroller.beatTempo /= 10;
                gameInstance.theMusic.pitch /= 10.0f;
                gameInstance.speedUp = false;
            }
        }

        foreach (Transform note in gameInstance.Notes)
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
    public override void Exit()
    {

    }
}

public class PausedState : BaseState
{
    private bool unpausing = false;
    private GameManager gameInstance = GameManager.instance;
    public override void Enter()
    {
        gameInstance.beatScroller.hasStarted = false;
        gameInstance.theMusic.Pause();
        gameInstance.Text.text = "Game paused \n Press Escape to unpause \n Press Space to leave";
        gameInstance.EnabledDisableNotes(false);
    }
    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!unpausing)
            {
                gameInstance.StartCoroutine(UnPause());
                unpausing = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !unpausing)
        {
            SceneManager.LoadScene("GameMenu");
        }  
    }
    public override void Exit()
    {

    }
    IEnumerator UnPause()
    {
        gameInstance.Text.text = "Starting in";
        gameInstance.textCountdown.text = "3";
        yield return new WaitForSeconds(1.0f);
        gameInstance.textCountdown.text = "2";
        yield return new WaitForSeconds(1.0f);
        gameInstance.textCountdown.text = "1";
        yield return new WaitForSeconds(1.0f);
        gameInstance.textCountdown.text = "";
        gameInstance.Text.text = "";
        gameInstance.theMusic.UnPause();
        gameInstance.beatScroller.hasStarted = true;
        unpausing = false;
        gameInstance.EnabledDisableNotes(true);
        gameInstance.TheStateMachine.Transition(new PlayingGame());
    }
}

public class ResultScreen : BaseState
{
    private GameManager gameInstance = GameManager.instance;
    private DatabaseAccess db;
    public override void Enter()
    {
        db = new DatabaseAccess();

        if (gameInstance.IsGhostMode == true)
        {
            gameInstance.ghostPanel.SetActive(false);
        }


        // Change le rank en fonction de l'accuracy
        string rank = "F";
        if (gameInstance.Accuracy > 40)
        {
            rank = "D";
            if (gameInstance.Accuracy > 55)
            {
                rank = "C";
                if (gameInstance.Accuracy > 75)
                {
                    rank = "B";
                    if (gameInstance.Accuracy > 85)
                    {
                        rank = "A";
                        if (gameInstance.Accuracy > 95)
                        {
                            rank = "S";
                            if (gameInstance.Accuracy == 100)
                            {
                                rank = "SS";
                            }
                        }
                    }
                }
            }
        }

        gameInstance.rankText.text = rank;

        gameInstance.finalScoreText.text = gameInstance.CurrentScore.ToString();
        gameInstance.highestComboText.text = gameInstance.HighestCombo + "x";
        gameInstance.accuracyResult.text = gameInstance.Accuracy.ToString("F2") + " %";

        gameInstance.gameScreen.SetActive(false);
        gameInstance.recepteursButtons.SetActive(false);
        gameInstance.resultsScreen.SetActive(true);

        gameInstance.badHitText.text = gameInstance.NbBadHit.ToString();
        gameInstance.goodHitText.text = gameInstance.NbGoodHit.ToString();
        gameInstance.perfectHitText.text = gameInstance.NbPerfectHit.ToString();
        gameInstance.missText.text = gameInstance.NbMiss.ToString();

        db.EnterScores(PlayerPrefs.GetString("username"), gameInstance.MusicName, gameInstance.CurrentScore, gameInstance.Accuracy, gameInstance.HighestCombo, rank);
    }
    public override void Update()
    {

    }
    public override void Exit()
    {

    }
}

public class StateMachine
{
    private BaseState currentState;

    public void Init(BaseState startingState)
    {
        currentState = startingState;
        currentState.Enter();
    }

    public void Transition(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState.Update();
    }
}