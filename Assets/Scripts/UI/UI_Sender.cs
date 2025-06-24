using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using System;

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

        ARFaceSender.Instance.StartSendData(ip, port);
    }
}
