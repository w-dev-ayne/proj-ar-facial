using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    public Transform cube;

    public void OnTrackableChanged(ARTrackablesChangedEventArgs<ARFace> args)
    {
        foreach (ARFace arg in args.updated)
        {
            cube.transform.position = arg.transform.position;
        }
    }
}
