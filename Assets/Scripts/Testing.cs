using System;
using JetBrains.Annotations;

[Serializable]
public struct Testing
{
    [CanBeNull] public string Message;
    public int Score;

    public Testing(String message, int score)
    {
        Message = message;
        Score = score;
    }
}