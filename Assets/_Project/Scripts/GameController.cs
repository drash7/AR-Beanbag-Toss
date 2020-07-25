using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : Singleton<GameController>
{
    // public GameObject normalScene;
    public PlaceBoardOnPlane placeOnPlane;
    public GameObject gameUI;
    public GameObject winPanel;
    public GameObject lostPanel;
    // public Button reanchorButton;
    // public GameObject gameController;
    // private ScoresController scoresController;

    private bool normalMode;

    public void Awake()
    {
        // NOTE: need to call the Singleton's Awake in this Awake or the Instance value won't get set
        base.Awake();

        // use Awake to make sure the listeners are registered before ARcontroller start is called
        ARController.OnARRunning.AddListener(OnARRunningListener);
    }

    private void Start()
    {
        // normalScene.SetActive(false);
        gameUI.SetActive(false);
        // scoresController = gameController.GetComponent<ScoresController>();
    }

    private void OnARRunningListener(bool ar)
    {
        // ScreenLog.Log("OnARRunningListener: " + ar);

        normalMode = !ar;
        // normalScene.SetActive(normalMode);
        if (ar)
        {
            PlaceBoard();
        }
        else
        {
            gameUI.SetActive(true);
        }

        ResetGame();
    }

    public void PlaceBoard()
    {
        // ScreenLog.Log("PlaceBoard");

        StartCoroutine(_DoPlaceBoard());
    }

    private IEnumerator _DoPlaceBoard()
    {
        gameUI.SetActive(false);

        // skip a frame to let input manager clear from button press
        yield return null;

        placeOnPlane.StartPlacingBoard();
        while (placeOnPlane.isPlacing)
        {
            yield return null;
        }

        gameUI.SetActive(true);
    }

    public void GameStarted()
    {
        // _AllowReanchor(false);
    }

    public void WinGame()
    {
        _GameOver(winPanel);
    }

    public void LoseGame()
    {
        _GameOver(lostPanel);
    }

    public void ResetGame()
    {
        // ScreenLog.Log("ResetGame");

        _ResetUI();
        // BagsController.Instance.ResetBags();
        // BagsController.Instance.BagOnDeck();
        // scoresController.ResetScore();
        ScoresController.Instance.ResetScore();
        // _AllowReanchor(true);
    }


    private void _ResetUI()
    {
        winPanel.SetActive(false);
        lostPanel.SetActive(false);
        // _AllowReanchor(false);
    }

    private void _GameOver(GameObject messagePanel)
    {
        messagePanel.SetActive(true);
        // BagsController.Instance.RemoveBagOnDeck();
    }

    //private void _AllowReanchor(bool allow)
    //{
    //    if (normalMode)
    //    {
    //        // todo maybe add a distance slider like in the VR version
    //        reanchorButton.gameObject.SetActive(false);
    //    }
    //    else
    //    {
    //        reanchorButton.gameObject.SetActive(allow);
    //    }
    //}
}