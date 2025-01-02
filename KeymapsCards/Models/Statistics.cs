using System;
using System.Collections.Generic;
using System.Linq;
using KeymapsCards.Models.Enums;

namespace KeymapsCards.Models;

public class Statistics
{
    public GameMode GameMode { get; set; }
    public int NumberOfCards { get; set; }
    public List<UserResponse> Responses { get; set; } = [];
    public bool IsTimedGameMode { get; set; }
    
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Software { get; set; }
    public string KeyboardScheme { get; set; }
    
    public string Duration
    {
        get
        {
            var duration = (End - Start).TotalSeconds;
            return $"{duration:0.00} s";
        }
    }
    
    public string CorrectAnswers
    {
        get
        {
            var correctAnswers = Responses.Count(r => r.IsKnown);
            return $"{correctAnswers}/{Responses.Count}";
        }
    }
    
    public bool IsFullyAnswered => Responses.All(r => r.IsKnown);
}