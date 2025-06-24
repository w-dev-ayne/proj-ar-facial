using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.Collections;
using System.Linq;
using System;

public class FaceBlendShapeLogger : MonoBehaviour
{

    private ARFace arFace;
    private ARKitFaceSubsystem faceSubsystem;
    private ARFaceManager arFaceManager;

    private string logText = String.Empty;
    List<ARKitBlendShapeCoefficient> blendShapes = new List<ARKitBlendShapeCoefficient>();

    void Awake()
    {
        arFace = GetComponent<ARFace>();
        arFaceManager = FindAnyObjectByType<ARFaceManager>();
    }

    void OnEnable()
    {
        faceSubsystem = (ARKitFaceSubsystem)arFaceManager.subsystem;
    }

    void Update()
    {
        if (faceSubsystem == null || arFace == null)
            return;

#if UNITY_IOS
        logText = String.Empty;

        var trackableId = arFace.trackableId;
        blendShapes = faceSubsystem.GetBlendShapeCoefficients(trackableId, Allocator.Temp).ToList();

        for (int i = 0; i < blendShapes.Count; i++)
        {
            var blendShape = blendShapes[i];
            logText += $"{blendShape.blendShapeLocation} : {blendShape.coefficient:F3}\n";
        }

        Managers.UI.FindPopup<UI_Debug>().UpdateLogText(logText);
#endif
    }
}