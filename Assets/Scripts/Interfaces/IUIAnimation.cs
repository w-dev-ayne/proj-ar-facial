using UnityEngine;
using UnityEngine.Events;

public interface IUIAnimation
{
    public void Play();
    public void DisappearAnim(UnityAction onComplete);
}
