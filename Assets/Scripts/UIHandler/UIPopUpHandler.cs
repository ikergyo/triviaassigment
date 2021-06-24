using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MessageType { Error, Info, RoundStart}
public delegate void PopUpButtonPressedDelegate(MessageType messageType);

public class UIPopUpHandler : MonoBehaviour
{

    [SerializeField]
    TMP_Text titleField;

    [SerializeField]
    TMP_Text messageField;

    MessageType lastMessageType;

    static event PopUpButtonPressedDelegate OkPressedEvent;
    public static void SubscribeToPressedEvent(PopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent += pressedAction;
    }
    public static void UnsubscribeFromPressedEvent(PopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent -= pressedAction;
    }

    public void ShowMessage(string title, string message, MessageType type)
    {
        titleField.text = title;
        messageField.text = message;
        this.gameObject.SetActive(true);
        lastMessageType = type;
    }

    public void ShowError(string message)
    {
        ShowMessage("Error", message, MessageType.Error);
    }
    public void OnClickOk()
    {
        this.gameObject.SetActive(false);
        OkPressedEvent?.Invoke(lastMessageType);
    }

}
