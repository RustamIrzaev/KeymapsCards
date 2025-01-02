using System;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace KeymapsCards.ViewModels.Game;

public partial class GuessByKeymapGameViewModel : GameBaseViewModel
{
    private readonly Random _random = new();
    
    [ObservableProperty] private string? _selectedAnswer;
    
    public ObservableCollection<string> AnswerOptions { get; set; } = [];

    public GuessByKeymapGameViewModel()
    {
        GenerateAnswerOptionsForCurrent();
    }
    
    private void GenerateAnswerOptionsForCurrent()
    {
        AnswerOptions.Clear();
        var correctAnswer = CurrentCard.Command;
        
        AnswerOptions.Add(correctAnswer);

        var incorrectAnswers = GameService.KeymapData.Keymaps
            .SelectMany(q => q.Commands)
            .Where(q => q.Command != correctAnswer)
            .Where(q => q.Keymap != CurrentCard.Keymap)
            .OrderBy(_ => _random.Next())
            .Take(3);

        foreach (var answer in incorrectAnswers)
        {
            AnswerOptions.Add(answer.Command);
        }
        
        AnswerOptions = new ObservableCollection<string>(AnswerOptions.OrderBy(_ => _random.Next()));
        OnPropertyChanged(nameof(AnswerOptions));
    }
    
    private void SubmitAnswer()
    {
        if (SelectedAnswer == CurrentCard.Command)
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