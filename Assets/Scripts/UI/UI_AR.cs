using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UI_AR : UI_Popup
{
    public UnityEvent onClickGetTextureButton;
    enum Objects
    {
        
    }

    enum Buttons
    {
        ReloadButton,
        GetTextureButton,
        CaptureButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        
        
        if (base.Init() == false)
            return false;
        return true;
    }
}
