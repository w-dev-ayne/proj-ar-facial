using UnityEngine;

public class FaceToCenter : MonoBehaviour
{
    private Transform centerTransform;

    void Awake()
    {
        this.centerTransform = Camera.main.transform.GetChild(0);
        transform.position = centerTransform.position;
    }

    void Update()
    {
        if (centerTransform == null)
            return;

        //transform.position = centerTransform.position;
    }
}