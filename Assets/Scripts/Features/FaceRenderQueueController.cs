using UnityEngine;

public class FaceRenderQueueController : MonoBehaviour
{
    [SerializeField]
    private int renderQueue = 4000;

    void Awake()
    {
        GetComponent<Renderer>().material.renderQueue = this.renderQueue;   
    }
}
