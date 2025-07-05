using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Popup : UI_Base
{
    [SerializeField]
    private bool animation = false;
    [SerializeField]
    private Define.AnimationType type;
    public delegate void loadHandler();
    
    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        Managers.UI.SetCanvas(gameObject, true);

        if (animation)
            SetAnimation();

        return true;
    }
    
    private void SetAnimation()
    {
        switch (type)
        {
            case Define.AnimationType.A:
                if (!this.transform.GetChild(0).GetChild(0).TryGetComponent<UIAppearAnimA>(out UIAppearAnimA animA))
                {
                    this.transform.GetChild(0).GetChild(0).AddComponent<UIAppearAnimA>();
                }
                break;
            case Define.AnimationType.B:
                if (!this.transform.GetChild(0).GetChild(0).TryGetComponent<UIAppearAnimB>(out UIAppearAnimB animB))
                {
                    this.transform.GetChild(0).GetChild(0).AddComponent<UIAppearAnimB>();
                }
                break;
            case Define.AnimationType.C:
                if (!this.transform.GetChild(0).GetChild(0).TryGetComponent<UIAppearAnimC>(out UIAppearAnimC animC))
                {
                    this.transform.GetChild(0).GetChild(0).AddComponent<UIAppearAnimC>();
                }
                break;
            default:
                break;
        }
        
    }

    public virtual void ClosePopupUI()
    {
        if (animation)
        {
            this.transform.GetChild(0).GetChild(0).GetComponent<IUIAnimation>().DisappearAnim(() =>
            {
                Managers.UI.ClosePopupUI(this);
            });
        }
        else
        {
            Managers.UI.ClosePopupUI(this);
        }
    }

    private void OnDestroy()
    {
        
    }
}
