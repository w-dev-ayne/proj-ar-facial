using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using UnityEngine.XR.ARFoundation;

public class UI_Debug : UI_Popup
{
    enum Objects
    {
        LogTextContentObject
    }

    enum Buttons
    {
        CameraButton,
        SettingButton
    }

    enum Texts
    {
        LogText
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.CameraButton).BindEvent(OnClickCameraButton);
        GetButton((int)Buttons.SettingButton).BindEvent(OnClickSettingButton);

        SetActiveCameraIcon(true);

        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickCameraButton()
    {
        Camera.main.GetComponent<ARCameraBackground>().enabled = !Camera.main.GetComponent<ARCameraBackground>().enabled;
        SetActiveCameraIcon(Camera.main.GetComponent<ARCameraBackground>().enabled);
    }

    private void OnClickSettingButton()
    {
        Managers.UI.ShowPopupUI<UI_Sender>();
    }

    private void SetActiveCameraIcon(bool isOn)
    {
        GetButton((int)Buttons.CameraButton).transform.GetChild(0).gameObject.SetActive(!isOn);
        GetButton((int)Buttons.CameraButton).transform.GetChild(1).gameObject.SetActive(isOn);
    }

    public void UpdateLogText(string message)
    {
        TextMeshProUGUI tmp = GetText((int)Texts.LogText);
        RectTransform content = GetObject((int)Objects.LogTextContentObject).GetComponent<RectTransform>();

        tmp.text = message;

        float contentHeight = tmp.preferredHeight;

        var size = content.sizeDelta;
        size.y = contentHeight;
        content.sizeDelta = size;
    }
}
