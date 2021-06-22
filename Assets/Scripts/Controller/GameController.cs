using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameController : MonoBehaviour
{

    const string anyCategory = "Any Category";

    [SerializeField]
    UIController uic;

    ITriviaApi triviaApi;
    Question[] questions;
    List<Category> categories;
    Player[] players;

    int questionNum = 0;
    int roundCounter = 0;

    public Question NextQuestion()
    {
        return null;
    }

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
        questions = await triviaApi.GetQuestions(amount, category);
        questionNum = questions.Length;
        InitializePlayers(playerCount);
        NextRound();
        uic.SetActivePanel("QuizPanel");
    }

    void InitializePlayers(int playerCount)
    {
        players = new Player[playerCount];
        for (int i = 0; i < playerCount; i++)
        {
            players[i] = new Player();
        }
    }

    void NextRound()
    {
        if (!(roundCounter < questionNum))
        {
            Finish();
            return;
        }
        Question currentQuestion = questions[roundCounter];
        Answer[] answers = GenerateAnswers(currentQuestion);
        uic.ShowQuestion(currentQuestion.question, answers);

        roundCounter++;
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

    void Finish()
    {

    }


    
}
