using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public delegate void PopUpButtonPressedDelegate(GameObject gObject, MessageType messageType);
public class PopUpScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text titleField;

    [SerializeField]
    TMP_Text messageField;

    [SerializeField]
    TMP_Text buttonTextField;

    MessageType type;

    private void Start()
    {
        this.gameObject.SetActive(true);
        
    }

    event PopUpButtonPressedDelegate OkPressedEvent;
    public void SubscribeToPressedEvent(PopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent += pressedAction;
    }
    public void UnsubscribeFromPressedEvent(PopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent -= pressedAction;
    }

    public void ShowMessage(string title, string message, string buttonText, MessageType type)
    {
        titleField.text = title;
        messageField.text = message;
        buttonTextField.text = buttonText;
        this.gameObject.SetActive(true);
        this.type = type;
    }

    public void OnClickOk()
    {
        this.gameObject.SetActive(false);
        OkPressedEvent?.Invoke(this.gameObject, type);
    }
}
