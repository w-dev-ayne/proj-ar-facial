using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenderScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.SenderScene;
        Debug.Log($"{SceneType} Init");

        Managers.UI.ShowPopupUI<UI_Sender>();

        return true;
    }
}
