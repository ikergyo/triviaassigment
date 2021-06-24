using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    static int maxId = 0;

    int id;
    string name;
    int score;
    float remainedTime;
    bool isPlaying;

    public static void Initialize()
    {
        maxId = 0;
    }

    public Player(float remainedTime)
    {
        id = ++maxId;
        name = "Player " + id.ToString();
        score = 0;
        this.remainedTime = remainedTime;
        isPlaying = true;
    }

    public string Name { get => name; }
    public int Id { get => id; }
    public int Score { get => score; set => score = value; }
    public float RemainedTime { get => remainedTime; set => remainedTime = value; }
    public bool IsPlaying { get => isPlaying; set => isPlaying = value; }
}
