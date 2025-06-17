using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : BaseScene
{
    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        SceneType = Define.Scene.Main;
        Debug.Log($"{SceneType} Init");
        
        Managers.Sound.PlayAudioClip(Define.Sound.Bgm, Managers.Sound.clips["BGM_Main"]);

        return true;
    }
}
