using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceBoardOnPlane : Singleton<PlaceBoardOnPlane>
{
    public GameObject boardPrefab;
    public GameObject placePromptPanel;

    private GameObject board;

    private ARRaycastManager _raycastManager;
    static List<ARRaycastHit> _hits = new List<ARRaycastHit>();
    private Camera _camera;
    public bool isPlacing { get; private set; }

    private void Start()
    {
        _raycastManager = GetComponent<ARRaycastManager>();
        //_camera = Camera.main;
    }

    public void StartPlacingBoard()
    {
        if (!_camera)
            _camera = Camera.main;

        _PromptToPlace(true);
        isPlacing = true;
    }


    private void _PromptToPlace(bool show)
    {
        placePromptPanel.SetActive(show);
    }

    private void _ReportBoardDistance()
    {
        Vector3 boardPosition = board.transform.position;
        boardPosition.y = 0;
        Vector3 cameraPosition = _camera.transform.position;
        cameraPosition.y = 0;
        float distance = (boardPosition - cameraPosition).magnitude;

        // ScreenLog.Log("Board distance: " + distance);
    }


    //--------------------------------
    // Similar to PlaceOnPlane.cs

    void Update()
    {
        if (!isPlacing)
            return;

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (_raycastManager.Raycast(touchPosition, _hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = _hits[0].pose;

            if (board == null)
            {
                board = Instantiate(boardPrefab, hitPose.position, hitPose.rotation);
                board.SetActive(true);
                // board.transform.eulerAngles.x += 90;
            }
            else
            {
                board.transform.position = hitPose.position;
            }

            // rotate towards camera, keeping it upright
            board.transform.LookAt(_camera.transform.position);
            board.transform.eulerAngles =
                new Vector3(0, board.transform.eulerAngles.y - 180, 0);

            isPlacing = false;
            _PromptToPlace(false);
        }
    }


    //------------------------------------
    // Taken from PlaceOnPlane.cs

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

}


// -------------------------------------
// USAGE EXAMPLE
// in GameController.cs

//public PlaceBoardOnPlane placeOnPlane;

//public void PlaceBoard()
//{
//    ScreenLog.Log("PlaceBoard");

//    StartCoroutine(_DoPlaceBoard());
//}

//private IEnumerator _DoPlaceBoard()
//{
//    gameUI.SetActive(false);

//    // skip a frame to let input manager clear from button press
//    yield return null;

//    placeOnPlane.StartPlacingBoard();
//    while (placeOnPlane.isPlacing)
//    {
//        yield return null;
//    }

//    gameUI.SetActive(true);
//}