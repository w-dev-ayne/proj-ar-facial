using UnityEngine;
using UnityEngine.XR.ARKit;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using Unity.Collections;
using System.Linq;
using System;
using System.Text;
using extOSC;

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

    private string ConvertToVSeeFaceName(string original)
    {
        if (string.IsNullOrEmpty(original)) return original;

        // 앞글자 소문자 변환, 나머지 유지 (CamelCase 유지)
        return char.ToLower(original[0]) + original.Substring(1);
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

    var trackableId = arFace.trackableId;
    blendShapes = faceSubsystem.GetBlendShapeCoefficients(trackableId, Allocator.Temp).ToList();

    string data = BuildVSeeFaceString(blendShapes, out string log);
    byte[] sendData = Encoding.UTF8.GetBytes(data);
    sender.SendData(sendData);

    Managers.UI.FindPopup<UI_Debug>().UpdateLogText(log);

        /*
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
        */

        /*
                logText = String.Empty;

                var trackableId = arFace.trackableId;
                blendShapes = faceSubsystem.GetBlendShapeCoefficients(trackableId, Allocator.Temp).ToList();

                for (int i = 0; i < blendShapes.Count; i++)
                {
                    var blendShape = blendShapes[i];
                    string arkitName = (blendShape.blendShapeLocation.ToString());
                    float value = blendShape.coefficient;

                    var message = new OSCMessage("/VMC/Ext/Blend/Val");
                    if (BlendShapeMapper.BlendShapeMap.TryGetValue(arkitName, out string key))
                    {
                        message.AddValue(OSCValue.String(key));
                    }
                    else
                    {
                        message.AddValue(OSCValue.String(arkitName));
                    }
                    message.AddValue(OSCValue.Float(value));
                    sender.SendData(message);
                    logText += $"{blendShape.blendShapeLocation} : {blendShape.coefficient:F3}\n";
                }

                var applyMsg = new OSCMessage("/VMC/Ext/Blend/Apply");
                sender.SendData(applyMsg);

                Managers.UI.FindPopup<UI_Debug>().UpdateLogText(logText);
                blendShapes.Clear();
        */
#endif
    }

    private string BuildVSeeFaceString(List<ARKitBlendShapeCoefficient> blendShapes, out string logText)
    {
        StringBuilder log = new StringBuilder();
        StringBuilder sb = new StringBuilder();

        // sb.Append($"trackingStatus-{(arFace.trackingState == UnityEngine.XR.ARSubsystems.TrackingState.Tracking ? 1 : 0)}|");
        
        sb.Append($"trackingStatus-1|");

        // 1) BlendShape key-value 쌍 "key-value|" (0~1 float -> 0~100 int)
        foreach (var blendShape in blendShapes)
        {
            string arkitName = blendShape.blendShapeLocation.ToString();

            if (BlendShapeMapper.ARKitToFacialMocap.TryGetValue(arkitName, out string vseeKey))
            {
                int valueInt = Mathf.RoundToInt(blendShape.coefficient * 100f);
                sb.Append($"{vseeKey}-{valueInt}|");
                log.Append($"{blendShape.blendShapeLocation} : {blendShape.coefficient:F3}\n");
            }
        }

        // 2) 특수 키 예시 (hapihapi), 값은 0으로 고정
        sb.Append("hapihapi-0|");

        Vector3 faceEuler = NormalizeEuler(transform.localEulerAngles);

        string headSection =
            $"=head#{-faceEuler.x:F2},{-faceEuler.y:F2},{faceEuler.z:F2}," +
            $"{transform.localPosition.x:F4},{transform.localPosition.y:F4},{-transform.localPosition.z:F4}|";

        Vector3 right = NormalizeEuler(arFace.rightEye.localEulerAngles);
        Vector3 left = NormalizeEuler(arFace.leftEye.localEulerAngles); ;

        string rightEyeSection =
            $"rightEye#{-right.x:F2}," +
            $"{-right.y:F2}," +
            $"{right.z:F2}|";

        string leftEyeSection =
            $"leftEye#{-left.x:F2}," +
            $"{-left.y:F2}," +
            $"{left.z:F2}|";

        sb.Append(headSection);
        sb.Append(rightEyeSection);
        sb.Append(leftEyeSection);

        logText = log.ToString();

        return sb.ToString();
    }

    Vector3 NormalizeEuler(Vector3 euler)
    {
        float NormalizeAngle(float angle)
        {
            angle = angle % 360f;
            if (angle > 180f) angle -= 360f;
            if (angle < -180f) angle += 360f;
            return angle;
        }
        return new Vector3(
            NormalizeAngle(euler.x),
            NormalizeAngle(euler.y),
            NormalizeAngle(euler.z)
        );
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

