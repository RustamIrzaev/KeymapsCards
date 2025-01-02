using System;
using System.Collections.ObjectModel;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using KeymapsCards.Models;
using KeymapsCards.Models.Events;
using KeymapsCards.Models.JsonModels;

namespace KeymapsCards.ViewModels.Game;

public partial class GameBaseViewModel : ViewModelBase
{
    protected readonly ObservableCollection<KeymapModel> Cards;
    protected int CurrentIndex;
    
    private Timer _timer;
    
    [ObservableProperty] public int _timeLeft;
    public int MaximumTimeLimit => AppData.TimedGameLimit;
    public string CurrentStepMessage => $"Card {CurrentIndex + 1} of {Cards.Count}";
    public string NextButtonText => CurrentIndex == Cards.Count - 1 ? "Finish" : "Next";
    public KeymapModel CurrentCard => Cards[CurrentIndex];
    public bool IsTimedGameMode => GameService.IsTimedGameMode;
    public required Action<FinishGameEventArgs> GameOverRequested { get; init; }
    
    protected GameBaseViewModel()
    {
        Cards = new ObservableCollection<KeymapModel>(GameService.Cards);
        StartTimer();
    }
    
    protected void StartTimer()
    {
        if (!GameService.IsTimedGameMode)
            return;
        
        _timer?.Dispose();
        _timer = new Timer(TimerCallback, null, 1000, 1000);
        TimeLeft = AppData.TimedGameLimit;
    }
    
    private void TimerCallback(object? state)
    {
        TimeLeft--;
        
        if (TimeLeft <= 0)
        {
            OnTimeExpired();
        }
    }
    
    protected virtual void OnTimeExpired()
    {
        ResetTimer();
    }

    protected void ResetTimer()
    {
        _timer?.Dispose();
    }
    
    protected void AddUserResponse(bool isKnown)
    {
        GameService.AddUserResponse(new UserResponse
        {
            Command = CurrentCard.Command,
            Keymap = CurrentCard.Keymap,
            Section = CurrentCard.Section,
            IsKnown = isKnown
        });
    }

    protected void FinishGame()
    {
        _timer?.Dispose();
        GameOverRequested?.Invoke(new FinishGameEventArgs(false));
    }

    public void AbortGame()
    {
        _timer?.Dispose();
        GameOverRequested?.Invoke(new FinishGameEventArgs(true));
    }
}