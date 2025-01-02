using System;
using System.Collections.ObjectModel;
using System.Linq;
using KeymapsCards.Models;

namespace KeymapsCards.ViewModels;

public class GameOverViewModel : ViewModelBase
{
    public ObservableCollection<UserResponse> UserResponses { get; set; }
    public string StatisticsMessage => $"You knew {UserResponses.Count(r => r.IsKnown)} out of {UserResponses.Count} shortcuts";
    public required Action StartNewGameRequested { get; init; }
    
    public GameOverViewModel()
    {
        UserResponses = new ObservableCollection<UserResponse>(GameService.UserResponses);
        
        OnPropertyChanged(nameof(UserResponses));
        OnPropertyChanged(nameof(StatisticsMessage));
    }
    
    public void StartNewGame()
    {
        StartNewGameRequested.Invoke();
    }
}