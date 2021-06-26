using System;
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
    /// <summary>
    /// Intitializing the players, creating player instances.
    /// </summary>
    /// <param name="timePerQuestions">Calculated time for players, reamined time</param>
    void InitializePlayers(float timePerQuestions)
    {
        float remainedTime = timePerQuestions * questionCount;
        for (int i = 0; i < players.Length; i++)
        {
            players[i] = new Player(remainedTime);
        }
    }
    /// <summary>
    /// Increasing the actual player's score.
    /// </summary>
    public void IncreasePlayerScore()
    {
        actualPlayer.Score++;
    }
    /// <summary>
    /// Checking is there question.
    /// </summary>
    /// <returns>Returns true if there is no actual question</returns>
    public bool IsQuestionNull()
    {
        return currentQuestion == null;
    }
    /// <summary>
    /// Checking the game is ended
    /// </summary>
    /// <returns>Returns true if the game finished</returns>
    public bool IsGameEnd()
    {
        return (!(questionCounter < questionCount) || !IsTherePlayingPlayer());
    }
    /// <summary>
    /// Checking is there playing player.
    /// </summary>
    /// <returns>Returns true if there is at least one playing player</returns>
    bool IsTherePlayingPlayer()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].IsPlaying)
                return true;
        }
        return false;
    }
    /// <summary>
    /// Setting up the next player to the actual player if there is remained plaer in this round
    /// </summary>
    /// <returns>Returns true if next player has been found. False if the round is ended</returns>
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
    /// <summary>
    /// Calculating the next question
    /// </summary>
    /// <returns>Returns the next question</returns>
    public Question NextQuestion()
    {
        playerCounter = 0;
        if (!(questionCounter < questionCount))
            throw new ArgumentException("Index out of range, there is no more questions");
        return currentQuestion = questions[questionCounter++];
    }
    /// <summary>
    /// Setting up the elapsed time for the actual user
    /// </summary>
    /// <param name="deltaTime">Elapsed time</param>
    /// <returns>Returns true if the remained time bigger than zero</returns>
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
