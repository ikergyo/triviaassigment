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
    UIErrorHandler errorHandler;

    #region MenuPanel
    [SerializeField]
    TMP_InputField amountField;

    [SerializeField]
    TMP_Dropdown categoriesField;
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
    #endregion

    event StartGaneEventHandler StartEventAsync;
    GameObject activePanel;

    // Start is called before the first frame update
    void Start()
    {
        InitializeActivePanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowQuestion(string question, Answer[] answers)
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
    }
    void ClearGrid()
    {
        foreach (Transform child in answerGrid.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
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
            if(panel.tag == tag)
            {
                panel.SetActive(true);
                return;
            }
        }

    }
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

    public bool ValidateMenuForm(string amount, string category)
    {
        if (!int.TryParse(amount, out int am) || am <= 0)
            return false;
        if (category == "Loading...")
            return false;
        return true;
    }

    public async void OnClickStart()
    {
        string amount = amountField.text;
        string category = categoriesField.options[categoriesField.value].text;
        if(!ValidateMenuForm(amount, category))
        {
            errorHandler.ShowError("Amount is not valid, or there is no category selected (it is still loading the categories)");
            return;
        }
        await StartEventAsync?.Invoke(amount, category, 1);
    }
}
