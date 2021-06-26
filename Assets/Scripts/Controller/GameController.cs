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
    /// <summary>
    /// Initializing the game.
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="category"></param>
    /// <param name="playerCount"></param>
    /// <returns>There is no return value</returns>
    public async Task InitializeGameAsync(string amount, string category, int playerCount = 1)
    {
        uic.SetActivePanel("LoadingPanel");
        Question[] questions = await triviaApi.GetQuestions(amount, category);
        gameState = new GameState(playerCount, questions, timePerQuestions);
        AnswerScript.SubscribeToPressedEvent(OnClickAnswerPressed);
        uic.SubscribeToPopUp(OnClickPopUpPressed);
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
    /// <summary>
    /// Calculating the next question and sending it to the UI
    /// </summary>
    public void NextQuestion()
    {
        Question question = gameState.NextQuestion();
        Answer[] answers = GenerateAnswers(question);        
        uic.ShowQuestion(question.question, gameState.QuestionCounter, answers);
    }
    /// <summary>
    /// Calculating the next round, determines the next player.
    /// </summary>
    void NextRound()
    {
        gameState.IsRoundActive = false;
        if (!gameState.NextPlayer() || gameState.IsQuestionNull())
        {
            FinishQuestion();
            return;
        }
        uic.ShowPlayerStart(gameState.ActualPlayer);
    }
    /// <summary>
    /// Question end method. Checking is there questions in the questions list or the game is finished.
    /// If there are questions it call NextQuestion() and NextRound()
    /// </summary>
    void FinishQuestion()
    {
        if(!gameState.IsQuestionNull())
            uic.ShowQuestionDetails(gameState.CurrentQuestion);
        if (gameState.IsGameEnd())
        {
            FinishGame();
            return;
        }
        NextQuestion();
        NextRound();
    }
    /// <summary>
    /// Uninitializing the game and showing the result of the game.
    /// </summary>
    void FinishGame()
    {
        UninitializeGame();
        uic.ShowResult(gameState.Players);
        uic.SetActivePanel("MenuPanel");
    }
   /// <summary>
   /// Subscribed method for the answer on click event.
   /// </summary>
   /// <param name="answer">Choosed answer</param>
    void OnClickAnswerPressed(Answer answer)
    {
        if (answer.IsCorrect)
            gameState.IncreasePlayerScore();
        NextRound();
    }
    /// <summary>
    /// Subscribed method for the PopUp button on click event.
    /// </summary>
    /// <param name="messageType"></param>
    void OnClickPopUpPressed(MessageType messageType)
    {
        if(messageType == MessageType.RoundStart)
            gameState.IsRoundActive = true;
    }
    /// <summary>
    /// Unitializing the game, it is useful at the end of the game.
    /// </summary>
    void UninitializeGame()
    {
        AnswerScript.UnsubscribeFromPressedEvent(OnClickAnswerPressed);
        uic.UnsubscribeFromPopUp(OnClickPopUpPressed);
        isStateInitialized = false;
    }
    /// <summary>
    /// Generating the answers with random logic. Putting the correct answer to a random place.
    /// </summary>
    /// <param name="question">Actual questions</param>
    /// <returns>Answers array about the generated answers</returns>
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
