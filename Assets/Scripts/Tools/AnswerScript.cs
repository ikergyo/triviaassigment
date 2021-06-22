using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    [SerializeField]
    TMP_Text textField;

    Answer answer;
    Image sourceImage;

    public Answer Answer { get => answer; }

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

    }
}
