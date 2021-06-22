using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer
{
    string option;
    bool isCorrect;


    public Answer(string option, bool isCorrect)
    {
        this.option = option;
        this.isCorrect = isCorrect;
    }

    public string Option { get => option; }
    public bool IsCorrect { get => isCorrect; }
    
}
