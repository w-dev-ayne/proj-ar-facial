using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.Collections;
using System.Linq;
using System;
using System.Text;

public class FaceBlendShapeLogger : MonoBehaviour
{

    private ARFace arFace;
    private ARKitFaceSubsystem faceSubsystem;
    private ARFaceManager arFaceManager;
    private ARFaceSender sender;

    private string logText = String.Empty;
    List<ARKitBlendShapeCoefficient> blendShapes = new List<ARKitBlendShapeCoefficient>();
    
    void Awake()
    {
        arFace = GetComponent<ARFace>();
        arFaceManager = FindAnyObjectByType<ARFaceManager>();
    }

    void Start()
    {
        sender = ARFaceSender.Instance;
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
        
        Dictionary<string, float> sendDict = new Dictionary<string, float>();

        for (int i = 0; i < blendShapes.Count; i++)
        {
            var blendShape = blendShapes[i];
            sendDict[blendShape.blendShapeLocation.ToString()] = blendShape.coefficient;
            logText += $"{blendShape.blendShapeLocation} : {blendShape.coefficient:F3}\n";
        }

        string json = JsonUtility.ToJson(new BlendShapePacket(sendDict));
        byte[] data = Encoding.UTF8.GetBytes(json);
        sender.SendData(data);

        Managers.UI.FindPopup<UI_Debug>().UpdateLogText(logText);
#endif
    }

    [System.Serializable]
    public class BlendShapePacket
    {
        public List<Entry> entries = new();
        public BlendShapePacket(Dictionary<string, float> dict)
        {
            foreach (var kv in dict)
            {
                entries.Add(new Entry { name = kv.Key, value = kv.Value });
            }
        }


        [System.Serializable]
        public class Entry
        {
            public string name;
            public float value;
        }
    }
}