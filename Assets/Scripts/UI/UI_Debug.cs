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
    public Color32 faceTrackingStatusNone;
    public Color32 faceTrackingStatusTracking;
    public Color32 faceTrackingStatusOut;

    enum Objects
    {
        LogTextContentObject,
        TrackingStatusObject,
        NetworkStatusObject
    }

    enum Buttons
    {
        CameraButton,
        SettingButton
    }

    enum Texts
    {
        LogText,
        TrackingStatusText,
        NetworkStatusText
    }

    public override bool Init()
    {
        BindObject(typeof(Objects));
        BindButton(typeof(Buttons));
        BindText(typeof(Texts));

        GetButton((int)Buttons.CameraButton).BindEvent(OnClickCameraButton);
        GetButton((int)Buttons.SettingButton).BindEvent(OnClickSettingButton);
        GetObject((int)Objects.TrackingStatusObject).GetComponent<Image>().color = this.faceTrackingStatusNone;

        SetActiveCameraIcon(false);
        UpdateNetworkStatus(false);
        UpdateTrackingStatus(Define.FaceTrackingStatus.None);

        ARManager.Instance.onTrackableAdded.AddListener((face) =>
        {
            UpdateTrackingStatus(Define.FaceTrackingStatus.Tracking);
        });


        ARManager.Instance.onTrackableIn.AddListener((face) =>
        {
            UpdateTrackingStatus(Define.FaceTrackingStatus.Tracking);
        });

        ARManager.Instance.onTrackableOut.AddListener((face) =>
        {
            UpdateTrackingStatus(Define.FaceTrackingStatus.Out);
        });

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

    public void UpdateTrackingStatus(Define.FaceTrackingStatus status)
    {
        Image image = GetObject((int)Objects.TrackingStatusObject).GetComponent<Image>();
        TextMeshProUGUI tmp = GetText((int)Texts.TrackingStatusText);

        switch (status)
        {
            case Define.FaceTrackingStatus.None:
                image.color = this.faceTrackingStatusNone;
                tmp.color = this.faceTrackingStatusNone;
                tmp.text = "Face None";
                break;
            case Define.FaceTrackingStatus.Tracking:
                image.color = this.faceTrackingStatusTracking;
                tmp.color = this.faceTrackingStatusTracking;
                tmp.text = "Face In";
                break;
            case Define.FaceTrackingStatus.Out:
                image.color = this.faceTrackingStatusOut;
                tmp.color = this.faceTrackingStatusOut;
                tmp.text = "Face Out";
                break;
        }
    }

    public void UpdateNetworkStatus(bool isConnect)
    {
        Image image = GetObject((int)Objects.NetworkStatusObject).GetComponent<Image>();
        TextMeshProUGUI tmp = GetText((int)Texts.NetworkStatusText);

        if (isConnect)
        {
            image.color = this.faceTrackingStatusTracking;
            tmp.color = this.faceTrackingStatusTracking;
            tmp.text = "Connect";
        }
        else
        {
            image.color = this.faceTrackingStatusNone;
            tmp.color = this.faceTrackingStatusNone;
            tmp.text = "Disconnect";
        }
    }
}
