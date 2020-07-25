using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTrigger : MonoBehaviour
{
    // public GameObject gameController;
    // private ScoresController scoresController;

    void Start()
    {
        // scoresController = gameController.GetComponent<ScoresController>();
    }

    void OnTriggerEnter(Collider other)
    {
        UnityEngine.Debug.Log("board hit");

        if (other.tag.Equals("Beanbag"))
        {
            ScoresController.Instance.HitBoard();
            // scoresController.HitBoard();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Beanbag"))
        {
            ScoresController.Instance.UnhitBoard();
            // scoresController.UnhitBoard();
        }
    }
}
