using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
using static TMPro.TMP_Dropdown;
using static UnityEngine.Rendering.DebugUI;
using System;
using System.Threading.Tasks;

public delegate Task StartGaneEventHandler(string amount, string category, int playerCount);

public class UIController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> panels;

    [SerializeField]
    UIPopUpHandler popUpHandler;

    #region MenuPanel
    [SerializeField]
    TMP_InputField amountField;

    [SerializeField]
    TMP_Dropdown categoriesField;

    [SerializeField]
    TMP_InputField playerCountField;
    #endregion

    #region QuizPanel
    [SerializeField]
    GameObject answerGrid;

    [SerializeField]
    GameObject answerPrefab;

    [SerializeField]
    List<Sprite> spriteForAnswer;

    [SerializeField]
    TMP_Text questionField;

    [SerializeField]
    TMP_Text maxQuestionField;

    [SerializeField]
    TMP_Text questionCountField;

    [SerializeField]
    TMP_Text maxScoreField;

    [SerializeField]
    TMP_Text scoreCountField;

    [SerializeField]
    TMP_Text playerDetailsField;

    [SerializeField]
    TMP_Text timerField;
    #endregion

    event StartGaneEventHandler StartEventAsync;
    GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        InitializeActivePanel();
    }

    #region Common Methods
    void InitializeActivePanel()
    {
        foreach (var panel in panels)
        {
            if (panel.activeSelf)
            {
                activePanel = panel;
                return;
            }
        }
    }

    public void SetActivePanel(string tag)
    {
        activePanel?.SetActive(false);
        foreach (var panel in panels)
        {
            if (panel.tag == tag)
            {
                panel.SetActive(true);
                activePanel = panel;
                return;
            }
        }
    }
    public void SubscribeToPopUp(UIPopUpButtonPressedDelegate buttonPressed)
    {
        popUpHandler.SubscribeToPressedEvent(buttonPressed);
    }
    public void UnsubscribeFromPopUp(UIPopUpButtonPressedDelegate buttonPressed)
    {
        popUpHandler.UnsubscribeFromPressedEvent(buttonPressed);
    }
    #endregion

    #region QuizMethods
    public void InitializeQuiz(int questionCount)
    {
        maxQuestionField.text = maxScoreField.text = questionCount.ToString();
        questionCountField.text = 0.ToString();
    }

    public void ShowQuestion(string question, int questionNumber, Answer[] answers)
    {
        ClearGrid();
        for (int i = 0; i < answers.Length; i++)
        {
            var prefab = Instantiate(answerPrefab);
            AnswerScript ansScr = prefab.GetComponent<AnswerScript>();
            ansScr.AddAnswer(answers[i]);
            ansScr.transform.parent = answerGrid.transform;
        }
        questionField.text = question;
        questionCountField.text = questionNumber.ToString();
    }

    internal void UnsubscribeFromPressedEvent(Action<MessageType> onClickPopUpPressed)
    {
        throw new NotImplementedException();
    }

    public void ShowPlayerStart(Player player)
    {
        playerDetailsField.text = player.Name;
        scoreCountField.text = player.Score.ToString();
        popUpHandler.ShowMessage("Round", player.Name + "'s round", "Ok", MessageType.RoundStart);
    }

    public void ShowQuestionDetails(Question question)
    {
        string questionString = "Round completed, the correct answer was: " + question.correct_answer;
        popUpHandler.ShowMessage("Next question", questionString, "Next", MessageType.QuestionEnd);
    }

    public void RefreshTime(Player player)
    {
        float time = player.RemainedTime;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        string rTime = String.Format("{0:00}:{1:00}", minutes, seconds);
        timerField.text = rTime;
    }
    public void ShowResult(Player[] players)
    {
        string resultMessage = "";
        for (int i = 0; i < players.Length; i++)
        {
            resultMessage += "Player: " + players[i].Name + ", score: " + players[i].Score + System.Environment.NewLine;
        }
        popUpHandler.ShowMessage("Result", resultMessage, "Done", MessageType.Info);

    }
    void ClearGrid()
    {
        foreach (Transform child in answerGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
    #endregion

    #region Menu Methods
    public void UpdateCategories(List<Category> categories)
    {
        categoriesField.ClearOptions();
        List<OptionData> list = new List<OptionData>();
        for (int i = 0; i < categories.Count; i++)
        {
            list.Add(new OptionData(categories[i].name));
        }
        categoriesField.AddOptions(list);
    }

    public void SubscribeToStart(StartGaneEventHandler action)
    {
        StartEventAsync += action;
    }

    public bool ValidateMenuForm(string amount, string category, string playersCount)
    {
        if (!int.TryParse(amount, out int am) || am <= 0)
            return false;
        if (category == "Loading...")
            return false;
        if (!int.TryParse(playersCount, out int playersNum) || playersNum <= 0)
            return false;
        return true;
    }

    public async void OnClickStart()
    {
        string amount = amountField.text;
        string category = categoriesField.options[categoriesField.value].text;
        string playersCount = playerCountField.text;
        if(!ValidateMenuForm(amount, category, playersCount))
        {
            popUpHandler.ShowError("Amount is not valid or number of players is not valid, or there is no category selected (it is still loading the categories)");
            return;
        }
        await StartEventAsync?.Invoke(amount, category, int.Parse(playersCount));
    }
    #endregion

}
