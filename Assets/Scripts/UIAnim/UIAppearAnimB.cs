using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class UIAppearAnimB : MonoBehaviour, IUIAnimation
{
    [SerializeField] private bool playOnAwake = true;

    public void Play()
    {
        this.transform.position += new Vector3(Screen.width, 0, 0);

        this.transform.DOMoveX(Screen.width / 2, 0.3f);
    }
    
    private void OnEnable()
    {
        if (playOnAwake)
            Play();
    }

    public void DisappearAnim(UnityAction onComplete)
    {
        this.transform.DOMoveX(Screen.width * 2, 0.3f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
