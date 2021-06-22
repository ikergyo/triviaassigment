using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    static int maxId = 0;

    int id;
    string name;
    int score;

    public Player()
    {
        id = ++maxId;
        name = "Player " + id.ToString();
        score = 0;
    }

    public string Name { get => name; }
    public int Id { get => id; }
    public int Score { get => score; set => score = value; }
}
