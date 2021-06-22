using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIErrorHandler : MonoBehaviour
{

    [SerializeField]
    TMP_Text titleField;

    [SerializeField]
    TMP_Text messageField;

    public void ShowError(string message)
    {
        titleField.text = "Error";
        messageField.text = message;
        this.gameObject.SetActive(true);
    }
    public void OnClickOk()
    {
        this.gameObject.SetActive(false);
    }

}
