using System;
using UnityEngine;

public class CharacterData : MonoBehaviour, ICharacterData
{
    private UiStats uiStats;
    public int Score {  get { return _score; } private set { _score = value; } }
    private int _score;

    public void SetScore( int score)
    {
        score = Mathf.Abs(score);
        Score += score;
        uiStats.SetScore(Score);
    }

    public void SetUi(UiStats ui)
    {
        uiStats = ui;
    }
}

public interface ICharacterData
{
    public int Score { get; }
    public void SetScore(int score);
    public void SetUi(UiStats ui);
}