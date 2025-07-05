using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIAppearAnimC : MonoBehaviour, IUIAnimation
{
    [SerializeField] private bool playOnAwake = true;
    private float value;

    public void Play()
    {
        this.transform.position -= new Vector3(0, this.value, 0);
        this.transform.DOMoveY(0 / 2, 0.3f);
    }
    
    private void OnEnable()
    {
        value = this.GetComponent<RectTransform>().sizeDelta.y;
        if (playOnAwake)
            Play();
    }

    public void DisappearAnim(UnityAction onComplete)
    {
        this.transform.DOMoveY(-value, 0.3f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
