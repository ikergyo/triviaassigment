using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum MessageType { Error, Info, RoundStart, RoundEnd, QuestionStart, QuestionEnd }
public delegate void UIPopUpButtonPressedDelegate(MessageType messageType);

public class UIPopUpHandler : MonoBehaviour
{

    [SerializeField]
    GameObject popUpPrefab;

    List<GameObject> activePopUps;

    event UIPopUpButtonPressedDelegate OkPressedEvent;

    private void Awake()
    {
        activePopUps = new List<GameObject>();
    }

    public void SubscribeToPressedEvent(UIPopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent += pressedAction;
    }
    public void UnsubscribeFromPressedEvent(UIPopUpButtonPressedDelegate pressedAction)
    {
        OkPressedEvent -= pressedAction;
    }
    /// <summary>
    /// Creating a popup window and showing the details of the window: title, message, text of the button.
    /// </summary>
    /// <param name="title">Title of the window</param>
    /// <param name="message">Message</param>
    /// <param name="buttonText">Text of the button</param>
    /// <param name="type">Type of the message</param>
    public void ShowMessage(string title, string message,string buttonText, MessageType type)
    {
        this.gameObject.SetActive(true);
        GameObject popUp = Instantiate(popUpPrefab,this.transform);
        popUp.transform.SetAsFirstSibling();
        activePopUps.Add(popUp);
        PopUpScript pus = popUp.GetComponent<PopUpScript>();
        pus.SubscribeToPressedEvent(OnClickOk);
        pus.ShowMessage(title, message, buttonText, type);
    }
    /// <summary>
    /// Showing an error message with Error title.
    /// </summary>
    /// <param name="message">Message of the error</param>
    public void ShowError(string message)
    {
        ShowMessage("Error", message, "Ok", MessageType.Error);
    }
    /// <summary>
    /// Subscribed event to the PopUp button on click event. It delete the pop up window object after the button clicked and call the subscribed method of the events.
    /// </summary>
    /// <param name="gObject">PopUp window object</param>
    /// <param name="type">Type of the message</param>
    public void OnClickOk(GameObject gObject, MessageType type)
    {
        activePopUps.Remove(gObject);
        Destroy(gObject);
        if (activePopUps.Count == 0)
            this.gameObject.SetActive(false);
        OkPressedEvent?.Invoke(type);
    }

}
