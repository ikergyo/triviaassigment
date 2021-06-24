using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{

    const string anyCategory = "Any Category";
    const float timePerQuestions = 10f;

    [SerializeField]
    UIController uic;

    ITriviaApi triviaApi;
    List<Category> categories;

    bool isStateInitialized = false;

    GameState gameState;


    // Start is called before the first frame update
    async void Start()
    {
        triviaApi = new OpenTriviaApi();
        categories = await triviaApi.GetCategories();
        Category any = new Category();
        any.name = anyCategory;
        any.id = 0.ToString();
        categories.Insert(0, any);
        uic.UpdateCategories(categories);
        uic.SubscribeToStart(InitializeGameAsync);
    }

    public async Task InitializeGameAsync(string amount, string category, int playerCount = 1)
    {
        uic.SetActivePanel("LoadingPanel");
        Question[] questions = await triviaApi.GetQuestions(amount, category);
        gameState = new GameState(playerCount, questions, timePerQuestions);
        AnswerScript.SubscribeToPressedEvent(OnClickAnswerPressed);
        UIPopUpHandler.SubscribeToPressedEvent(OnClickPopUpPressed);
        NextRound();
        isStateInitialized = true;
        uic.SetActivePanel("QuizPanel");
        uic.InitializeQuiz(gameState.QuestionCount);
    }

    void Update()
    {
        if (!isStateInitialized)
            return;
        if (!gameState.IsRoundActive)
            return;
        float delta = Time.deltaTime;
        uic.RefreshTime(gameState.ActualPlayer);
        if (!gameState.ElapseTime(delta))
        {
            NextRound();
        }
    }

    public void NextQuestion()
    {
        Question question = gameState.NextQuestion();
        Answer[] answers = GenerateAnswers(question);        
        uic.ShowQuestion(question.question, gameState.QuestionCounter, answers);
    }

    void NextRound()
    {
        gameState.IsRoundActive = false;
        if (!gameState.NextPlayer() || gameState.IsQuestionNull())
        {
            FinishQuestion();
            return;
        }
        uic.ShowRound(gameState.ActualPlayer);
    }
    void FinishQuestion()
    {
        if (gameState.IsGameEnd())
        {
            FinishGame();
            return;
        }
        NextQuestion();
        NextRound();
    }
    void FinishGame()
    {
        UninitializeGame();
        uic.ShowResult(gameState.Players);
        uic.SetActivePanel("MenuPanel");
    }
   
    void OnClickAnswerPressed(Answer answer)
    {
        if (answer.IsCorrect)
            gameState.IncreasePlayerScore();
        NextRound();
    }
    void OnClickPopUpPressed(MessageType messageType)
    {
        if(messageType == MessageType.RoundStart)
            gameState.IsRoundActive = true;
    }

    void UninitializeGame()
    {
        AnswerScript.UnsubscribeFromPressedEvent(OnClickAnswerPressed);
        UIPopUpHandler.UnsubscribeFromPressedEvent(OnClickPopUpPressed);
        isStateInitialized = false;
    }

    Answer[] GenerateAnswers(Question question)
    {
        Answer[] answers = new Answer[question.incorrect_answers.Length + 1];
        int correctNum = Random.Range(0, answers.Length);
        answers[correctNum] = new Answer(question.correct_answer, true);
        for (int i = 0; i < question.incorrect_answers.Length; i++)
        {
            if (answers[i] != null)
            {
                answers[answers.Length - 1] = new Answer(question.incorrect_answers[i], false);
                continue;
            }
            answers[i] = new Answer(question.incorrect_answers[i], false);
        }
        return answers;
    }





}
