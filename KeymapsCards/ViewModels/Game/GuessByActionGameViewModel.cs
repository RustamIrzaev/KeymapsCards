using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KeymapsCards.ViewModels.Game;

public partial class GuessByActionGameViewModel : GameBaseViewModel
{
    private readonly Random _random = new();
    
    [ObservableProperty] private string? _selectedAnswer;
    
    public ObservableCollection<string> AnswerOptions { get; set; } = [];

    public GuessByActionGameViewModel()
    {
        GenerateAnswerOptionsForCurrent();
    }
    
    private void GenerateAnswerOptionsForCurrent()
    {
        AnswerOptions.Clear();
        var correctAnswer = CurrentCard.Keymap;
        
        AnswerOptions.Add(correctAnswer);

        var incorrectAnswers = GameService.KeymapData.Keymaps
            .SelectMany(q => q.Commands)
            .Where(q => q.Keymap != correctAnswer)
            .Where(q => q.Command != CurrentCard.Command)
            .OrderBy(_ => _random.Next())
            .Take(3);

        foreach (var answer in incorrectAnswers)
        {
            AnswerOptions.Add(answer.Keymap);
        }
        
        AnswerOptions = new ObservableCollection<string>(AnswerOptions.OrderBy(_ => _random.Next()));
        OnPropertyChanged(nameof(AnswerOptions));
    }
    
    private void SubmitAnswer()
    {
        if (SelectedAnswer == CurrentCard.Keymap)
        {
            AddUserResponse(true);
        }
        else
        {
            AddUserResponse(false);
        }
    }
    
    [RelayCommand]
    public void NextCard()
    {
        SubmitAnswer();
        
        if (CurrentIndex < Cards.Count - 1)
        {
            CurrentIndex++;
            SelectedAnswer = null;
            
            GenerateAnswerOptionsForCurrent();
            OnPropertyChanged(nameof(CurrentCard));
            OnPropertyChanged(nameof(SelectedAnswer));
            OnPropertyChanged(nameof(NextButtonText));
            OnPropertyChanged(nameof(CurrentStepMessage));
        }
        else
        {
            FinishGame();
        }
    }
}