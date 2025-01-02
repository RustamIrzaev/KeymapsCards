using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KeymapsCards.ViewModels.Game;

public partial class ClassicGameViewModel : GameBaseViewModel
{
    [ObservableProperty] private bool _showKeymap;
    
    [RelayCommand]
    public void MarkAsKnown()
    {
        ResetTimer();
        AddUserResponse(true);
        ShowKeymap = true;
    }

    [RelayCommand]
    public void MarkAsUnknown()
    {
        ResetTimer();
        AddUserResponse(false);
        ShowKeymap = true;
    }

    [RelayCommand]
    public void NextCard()
    {
        if (CurrentIndex < Cards.Count - 1)
        {
            StartTimer();
            CurrentIndex++;
            ShowKeymap = false;
            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(NextButtonText));
            OnPropertyChanged(nameof(CurrentStepMessage));
        }
        else
        {
            FinishGame();
        }
    }

    protected override void OnTimeExpired()
    {
        MarkAsUnknown();
    }
}