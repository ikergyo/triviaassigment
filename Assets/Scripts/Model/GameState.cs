using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    Question[] questions;
    Player[] players;

    int questionCount;
    int playerCounter;
    int questionCounter;
    Player actualPlayer;
    Question currentQuestion;
    bool isRoundActive;

    public GameState(int playerNum, Question[] questions, float timePerQuestions)
    {
        playerCounter = 0;
        questionCounter = 0;
        this.questions = questions;
        questionCount = questions.Length;
        Player.Initialize();
        players = new Player[playerNum];
        isRoundActive = false;
        InitializePlayers(timePerQuestions);
    }

    public Question CurrentQuestion { get => currentQuestion; }
    public int QuestionCount { get => questionCount; }
    public Player[] Players { get => players; }
    public bool IsRoundActive { get => isRoundActive; set => isRoundActive = value; }
    public Player ActualPlayer { get => actualPlayer; set => actualPlayer = value; }
    public int QuestionCounter { get => questionCounter; }

    void InitializePlayers(float timePerQuestions)
    {
        float remainedTime = timePerQuestions * questionCount;
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player(remainedTime);
        }
    }
    public void IncreasePlayerScore()
    {
        actualPlayer.Score++;
    }
    public bool IsQuestionNull()
    {
        return currentQuestion == null;
    }
    public bool IsGameEnd()
    {
        return (!(questionCounter < questionCount) || !IsTherePlayingPlayer());
    }
    bool IsTherePlayingPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsPlaying)
                return true;
        }
        return false;
    }
    public bool NextPlayer()
    {
        Player tmpPlayer;
        while(playerCounter < players.Length)
        {
            if((tmpPlayer = players[playerCounter++]).IsPlaying)
            {
                actualPlayer = tmpPlayer;
                return true;
            }
        }
        return false;
    }
    public Question NextQuestion()
    {
        playerCounter = 0;
        return currentQuestion = questions[questionCounter++];
    }
    public bool ElapseTime(float deltaTime)
    {
        if (actualPlayer.RemainedTime > 0)
        {
            actualPlayer.RemainedTime -= deltaTime;
            return true;
        }
        actualPlayer.RemainedTime = 0;
        actualPlayer.IsPlaying = false;
        return false;

    }
}
