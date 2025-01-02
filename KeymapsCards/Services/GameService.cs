using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using KeymapsCards.Helpers;
using KeymapsCards.Models;
using KeymapsCards.Models.Enums;
using KeymapsCards.Models.JsonModels;

namespace KeymapsCards.Services;

public class GameService
{
    private readonly StatisticsService _statisticsService;
    private readonly Random _random = new();
    private List<KeymapModel> _cards = [];
    private int _numberOfCards;
    private GameMode _mode = GameMode.Classic;
    private Statistics _statistics = new();
    
    public KeymapData KeymapData { get; private set; }
    public List<UserResponse> UserResponses { get; } = [];
    public bool IsGameFinished { get; private set; }
    public ReadOnlyCollection<KeymapModel> Cards => _cards.AsReadOnly();
    public bool IsTimedGameMode { get; private set; }

    public GameService(StatisticsService statisticsService)
    {
        _statisticsService = statisticsService;
    }
    
    public void AddUserResponse(UserResponse response)
    {
        if (IsGameFinished)
            return;
        
        UserResponses.Add(response);
    }
    
    public void StartGame(GameMode mode, KeymapData keymapData, int numberOfCards, bool isTimedGameMode)
    {
        KeymapData = keymapData;
        _numberOfCards = numberOfCards;
        _mode = mode;
        IsTimedGameMode = isTimedGameMode;
        
        _statistics = new Statistics
        {
            GameMode = mode,
            Start = DateTime.Now,
            NumberOfCards = numberOfCards,
            Software = keymapData.Software,
            KeyboardScheme = keymapData.KeyboardScheme,
            IsTimedGameMode = IsTimedGameMode
        };

        UserResponses.Clear();
        IsGameFinished = false;
        _cards.Clear();
        
        PrepareGame();
    }

    public void FinishGame()
    {
        IsGameFinished = true;
        var sortedResponses = UserResponses.OrderByDescending(r => r.IsKnown).ToList();
        
        UserResponses.Clear();
        foreach (var response in sortedResponses)
        {
            UserResponses.Add(response);
        }
        
        _statistics.End = DateTime.Now;
        _statistics.Responses = UserResponses;
        _statisticsService.SaveStatistics(_statistics);
    }
    
    private void PrepareGame()
    {
        var allCommands = new List<KeymapModel>();
        
        foreach (var section in KeymapData.Keymaps)
        {
            KeymapHelper.ReplaceKeymapSymbols(section);
            section.Commands.ForEach(entry => entry.Section = section.Section);
            allCommands.AddRange(section.Commands);
        }
        
        var maxCards = Math.Min(allCommands.Count, _numberOfCards);
        
        _cards = new(allCommands.OrderBy(x => _random.Next()).Take(maxCards));
    }
}