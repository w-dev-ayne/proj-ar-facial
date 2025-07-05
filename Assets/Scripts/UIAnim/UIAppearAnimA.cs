using System;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UIAppearAnimA : MonoBehaviour, IUIAnimation
{
    [SerializeField] private bool playOnAwake = true;

    public void Play()
    {
        this.transform.localScale = Vector3.one * 0.8f;
        this.transform.DOScale(Vector3.one * 1.0f, 0.5f).SetEase(Ease.OutBack);
    }
    
    private void OnEnable()
    {
        if (playOnAwake)
            Play();
    }

    public void DisappearAnim(UnityAction onComplete)
    {
        this.transform.localScale = Vector3.one * 1.0f;
        this.transform.DOScale(Vector3.zero, 0.25f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
