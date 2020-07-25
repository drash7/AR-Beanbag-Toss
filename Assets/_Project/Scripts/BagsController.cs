using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagsController : Singleton<BagsController>
{

    public Transform throwLocation;
    public Camera mainCamera;
    public GameObject beanbagPrefab;
    public GameObject beanbagInstance;
    private GameObject bagsContainer;

    // Start is called before the first frame update
    void Start()
    {
        throwLocation.position = mainCamera.transform.position;
        bagsContainer = new GameObject();
    }

    public void ThrowBag()
    {
        beanbagInstance = Instantiate(beanbagPrefab, throwLocation.position, Quaternion.identity);
        beanbagInstance.transform.position = throwLocation.position;
        beanbagInstance.transform.parent = bagsContainer.transform;
    }

    //public void ResetBags()
    //{
    //    Destroy(bagsContainer)
    //}

    // Update is called once per frame
    void Update()
    {
        throwLocation.position = mainCamera.transform.position;
    }
}
