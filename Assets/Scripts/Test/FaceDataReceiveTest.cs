using UnityEngine;

public class FaceDataReceiveTest : MonoBehaviour
{
    public Transform rightEye;
    public Transform lefyEye;


    public void SetRightEyeTransform(Vector3 vector)
    {
        rightEye.localEulerAngles = vector;
    }
    
    public void SetLeftEyeTransform(Vector3 vector)
    {
        lefyEye.localEulerAngles = vector;
    }
}
