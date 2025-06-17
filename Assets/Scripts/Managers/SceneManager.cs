using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    private Define.Scene _curSceneType = Define.Scene.Unknown;

    public Define.Scene CurrentSceneType
    {
        get
        {
            if (_curSceneType != Define.Scene.Unknown)
                return _curSceneType;
            return CurrentScene.SceneType;
        }
        set {  _curSceneType = value; }
    }

    public BaseScene CurrentScene { get { return GameObject.Find("@Scene").GetComponent<BaseScene>(); } }

    public void Init()
    {

    }

    public void ChangeScene(Define.Scene type)
    {
        CurrentScene.Clear();

        _curSceneType = type;
        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        char[] letters = name.ToLower().ToCharArray();
        letters[0] = char.ToUpper(letters[0]);
        return new string(letters);
    }
}
