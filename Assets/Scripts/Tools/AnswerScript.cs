using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void AnswerPressedDelegate(Answer answer);

public class AnswerScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text textField;

    Answer answer;
    Image sourceImage;

    public Answer Answer { get => answer; }

    static event AnswerPressedDelegate AnswerPressedEvent;

    public static void SubscribeToPressedEvent(AnswerPressedDelegate pressedAction)
    {
        AnswerPressedEvent += pressedAction;
    }
    public static void UnsubscribeFromPressedEvent(AnswerPressedDelegate pressedAction)
    {
        AnswerPressedEvent -= pressedAction;
    }

    void Start()
    {
        sourceImage = GetComponent<Image>();
    }

    public void AddAnswer(Answer answer)
    {
        this.answer = answer;
        Initialize();
    }
    
    public void AddSourceImage(Sprite image)
    {
        sourceImage.sprite = image;
    }
    void Initialize()
    {
        textField.text = answer.Option;
    }

    public void OnClickAnswer()
    {
        AnswerPressedEvent?.Invoke(answer);
    }
}
