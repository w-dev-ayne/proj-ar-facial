using System;
using DG.Tweening;
using UnityEngine;

public class AppearElement : MonoBehaviour
{
    public Define.AppearDirection direction; // enum 변수로 방향 설정

    private Vector3 targetPosition; // 최종 이동할 포지션
    [SerializeField]
    private float duration = 0.3f;

    private void Awake()
    {
        targetPosition = this.transform.localPosition;

        switch (direction)
        {
            case Define.AppearDirection.left:
                this.transform.position = this.transform.position + new Vector3(-500, 0, 0);
                break;
            case Define.AppearDirection.right:
                this.transform.position = this.transform.position + new Vector3(500, 0, 0);
                break;
            case Define.AppearDirection.top:
                this.transform.position = this.transform.position + new Vector3(0, 300, 0);
                break;
            case Define.AppearDirection.bottom:
                this.transform.position = this.transform.position +  new Vector3(0, -300, 0);
                break;
            default:
                break;
        }
    }

    private void OnEnable()
    {
        Play();
    }

    private void Play()
    {
        this.transform.DOLocalMove(targetPosition, duration, true);
    }
}
