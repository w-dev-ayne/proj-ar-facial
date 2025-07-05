using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;
using UnityEngine.XR.ARFoundation;

public class UI_Sender : UI_Popup
{

    enum Objects
    {
        IPInputFieldObject,
        PortInputFieldObject
    }

    enum Buttons
    {
        SendDataButton,
        CloseButton
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));

        GetButton((int)Buttons.SendDataButton).BindEvent(OnClickSendDataButton);
        GetButton((int)Buttons.CloseButton).BindEvent(ClosePopupUI);

        if (base.Init() == false)
            return false;
        return true;
    }

    private void OnClickSendDataButton()
    {
        string ip = GetObject((int)Objects.IPInputFieldObject).GetComponent<TMP_InputField>().text;
        int port = Int32.Parse(GetObject((int)Objects.PortInputFieldObject).GetComponent<TMP_InputField>().text);

        if (!ARFaceSender.Instance.isStart)
        {
            ARFaceSender.Instance.StartSendData(ip, port);
            GetButton((int)Buttons.SendDataButton).transform.GetChild(0).GetChild(0).gameObject.SetActive(!ARFaceSender.Instance.isStart);

        }
        else
        {
            ARFaceSender.Instance.StopSendData();
            GetButton((int)Buttons.SendDataButton).transform.GetChild(0).GetChild(0).gameObject.SetActive(!ARFaceSender.Instance.isStart);
        }

        Managers.UI.FindPopup<UI_Debug>().UpdateNetworkStatus(ARFaceSender.Instance.isStart);
    }
}
