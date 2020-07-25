using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleTrigger : MonoBehaviour
{

    // public GameObject gameController;
    // private ScoresController scoresController;

    void Start()
    {
        // scoresController = gameController.GetComponent<ScoresController>();
    }

    public void OnTriggerEnter(Collider other)
    {

        UnityEngine.Debug.Log("Hole hit");

        if (other.tag.Equals("Beanbag"))
        {
            ScoresController.Instance.HitHole();
            // scoresController.HitHole();
        }
    }
}
