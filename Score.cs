using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score: IComparable
{
    private string name;
    private int score;

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int ScoreP
    {
        get { return this.score; }
        set { this.score = value; }
    }

    public Score(string n, int s)
    {
        this.name = n;
        this.score = s;
    }

    public int CompareTo(object obj)
    {
        Score other = obj as Score;
        return -this.score.CompareTo(other.score);
    }

    public override string ToString()
    {
        return name + "\t\t" + score;
    }
}
