using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ScoresController : Singleton<ScoresController>
{

    private int currentScore;
    public Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentScore = 0;
        scoreText.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HitHole()
    {

        UnityEngine.Debug.Log("hithole called");

        currentScore += 3;
        if (currentScore == 21)
        {
            GameController.Instance.WinGame();
        }
        else if (currentScore > 21)
        {
            GameController.Instance.LoseGame();
        }
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void HitBoard()
    {

        UnityEngine.Debug.Log("hitboard called");

        currentScore += 1;
        if (currentScore == 21)
        {
            GameController.Instance.WinGame();
        }
        else if (currentScore > 21)
        {
            GameController.Instance.LoseGame();
        }
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void UnhitBoard()
    {
        currentScore -= 1;
        if (currentScore == 21)
        {
            GameController.Instance.WinGame();
        }
        else if (currentScore > 21)
        {
            GameController.Instance.LoseGame();
        }
        scoreText.text = "Score: " + currentScore.ToString();
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "Score: " + currentScore.ToString();
    }

}
