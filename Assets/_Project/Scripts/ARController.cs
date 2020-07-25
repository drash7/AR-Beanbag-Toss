//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.XR.ARFoundation;
//using UnityEngine.XR.ARSubsystems;
//using UnityEngine.Events;

//[System.Serializable]
//public class ARModeEvent : UnityEvent<bool> { }

//public class ARController : MonoBehaviour
//{

//    public ARPlaneManager planeManager;

//    public ARModeEvent onARMode = new ARModeEvent();
//    // public GameController gc = new GameController();

//    //public bool isARAvailable;
//    //public bool isARRunning;

//    private void Start()
//    {
//        StartCoroutine(WaitForARReady());
//    }

//    IEnumerator WaitForARReady()
//    {
//        yield return ARSession.CheckAvailability();

//        while (ARSession.state != ARSessionState.Unsupported && ARSession.state != ARSessionState.NeedsInstall && ARSession.state < ARSessionState.Ready)
//        {
//            yield return null;
//        }

//        if (ARSession.state < ARSessionState.Ready)
//        {
//            // start normal mode
//            onARMode.Invoke(false);
//        }
//        else
//        {
//            // start AR mode
//            onARMode.Invoke(true);
//            // gc.PlaceBoardOnPlane();
//        }
//    }

//    //private void Start()
//    //{
//    //    StartCoroutine(WaitForARReady());
//    //}

//    //IEnumerator WaitForARReady()
//    //{

//    //    // support check:
//    //    // while (checking)    {
//    //    //     if AR state == unsupported, unsupported = true, return
//    //    //     if state >= Ready, done checking
//    //    // }

//    //    // prompt user to scan the room
//    //    // wait while (trackables.count == 0)
//    //    // hide prompt to scan
//    //    // arRunning = true;



//    //}

//    public bool IsARAvailable()
//    {
//        return (ARSession.state != ARSessionState.Unsupported) &&
//            (ARSession.state != ARSessionState.NeedsInstall);
//    }

//    public bool IsARRunning()
//    {
//        return (ARSession.state >= ARSessionState.Ready) && 
//            (planeManager.trackables.count > 0);
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;

[System.Serializable]
public class BooleanEvent : UnityEvent<bool> { }

[System.Serializable]
public class ARModeEvent : UnityEvent<bool> { }

public class ARController : MonoBehaviour
{
    [Tooltip("Un-check this to never allow AR mode in the app")]
    public bool allowARMode = true;

    public ARModeEvent onARMode = new ARModeEvent();

    [Header("AR Objects")]
    public ARSession arSession;
    public ARSessionOrigin arOrigin;
    public ARPlaneManager planeManager;

    public GameObject scanRoomUI;

    [Header("Normal Objects")]
    public Camera defaultCamera;

    public static BooleanEvent OnARRunning = new BooleanEvent();


    public bool IsARAvailable => (allowARMode &&
                                  ARSession.state != ARSessionState.Unsupported &&
                                  ARSession.state != ARSessionState.NeedsInstall);

    public bool IsARRunning => (allowARMode &&
                                ARSession.state >= ARSessionState.Ready &&
                                planeManager.trackables.count > 0);


    public void Start()
    {
#if UNITY_EDITOR
        allowARMode = false;
#endif
        if (allowARMode)
        {
            EnableAR(true);
            StartCoroutine(_WaitForARReady());
        }
        else
        {
            // ScreenLog.Log("Non-AR mode only");
            EnableAR(false);
            OnARRunning.Invoke(false);
        }
    }

    public void EnableAR(bool enable)
    {
        arSession.gameObject.SetActive(enable);
        arOrigin.gameObject.SetActive(enable);
        arSession.enabled = enable;

        defaultCamera.gameObject.SetActive(!enable);
    }



    IEnumerator _WaitForARReady()
    {
        // wait for for AR session  support check
        bool checking = true;
        while (checking)
        {
            // not supported on this device? exit coroutine
            if (ARSession.state == ARSessionState.Unsupported)
            {
                // ScreenLog.Log("AR Not Supported on this device");
                OnARRunning.Invoke(false);
                yield break;
            }

            // is supported and session is ready? exit loop and continue
            if (ARSession.state >= ARSessionState.Ready)
            {
                checking = false;
            }

            // keep waiting for SupportChecker
            yield return null;
        }

        // ScreenLog.Log("AR supported");

        // wait for environment scan
        _PromptToScan(true);
        while (planeManager.trackables.count == 0)
        {
            yield return null;
        }

        _PromptToScan(false);

        // ScreenLog.Log("Tracking planes");
        OnARRunning.Invoke(true);
    }

    private void _PromptToScan(bool show)
    {
        scanRoomUI.SetActive(show);
    }
}

//// -------------------------------------
//// USAGE EXAMPLE
//// in GameController.cs

//private void Awake()
//{
//    // NOTE: need to call the Singleton's Awake in this Awake or the Instance value won't get set
//    base.Awake();

//    // make sure the listeners are registered before ARcontroller start is called
//    ARController.OnARRunning.AddListener(OnARRunningListener);
//}


//private void OnARRunningListener(bool ar)
//{
//    ScreenLog.Log("OnARRunningListener: " + ar);

//    normalMode = !ar;
//    normalScene.SetActive(normalMode);
//    if (ar)
//    {
//        PlaceBoard();
//    }
//    else
//    {
//        gameUI.SetActive(true);
//    }

//    ResetGame();
//}

