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

    public void ShowError(string message)
    {
        ShowMessage("Error", message, "Ok", MessageType.Error);
    }
    public void OnClickOk(GameObject gObject, MessageType type)
    {
        activePopUps.Remove(gObject);
        Destroy(gObject);
        if (activePopUps.Count == 0)
            this.gameObject.SetActive(false);
        OkPressedEvent?.Invoke(type);
    }

}
