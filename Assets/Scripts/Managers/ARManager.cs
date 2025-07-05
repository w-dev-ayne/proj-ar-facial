using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    public static ARManager Instance;
    public UnityEvent<ARFace> onTrackableAdded;
    public UnityEvent<ARFace> onTrackableOut;
    public UnityEvent<ARFace> onTrackableIn;
    private bool isTracking = false;


    void Awake()
    {
        Instance = this;
    }

    public void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARFace> changes)
    {
        foreach (ARFace face in changes.added) // 얼굴 최초 인식 처리
        {
            isTracking = true;
            onTrackableAdded?.Invoke(face);
        }

        foreach (ARFace face in changes.updated)
        {
            if (face.trackingState == TrackingState.None && isTracking == true) // 얼굴 인식 안될 때 처리
            {
                Debug.Log("Track Out");
                isTracking = false;
                onTrackableOut?.Invoke(face);
            }
            else if (face.trackingState == TrackingState.Tracking && isTracking == false) // 얼굴 인식 다시 되는 순간 처리
            {
                Debug.Log("Tracking In");
                isTracking = true;
                onTrackableIn?.Invoke(face);
            }
        }

        foreach (var face in changes.removed)
        {
            Debug.Log($"Face {face} is Removed.");
        }
    }
}
