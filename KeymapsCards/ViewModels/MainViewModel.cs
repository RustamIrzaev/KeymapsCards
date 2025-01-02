using System;
using CommunityToolkit.Mvvm.ComponentModel;
using KeymapsCards.Extensions;
using KeymapsCards.Models.Enums;
using KeymapsCards.Models.Events;
using KeymapsCards.ViewModels.Game;

namespace KeymapsCards.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty] private bool _alwaysOnTop;
    [ObservableProperty] private ViewModelBase? _currentViewModel;
    
    public MainViewModel()
    {
        NavigateToSelectionMenu();
    }
    
    public void ToggleAlwaysOnTop()
    {
        AlwaysOnTop = !AlwaysOnTop;
        var window = App.Current?.GetMainWindowDescriptor();
        
        if (window != null)
            window.Topmost = AlwaysOnTop;
    }

    partial void OnCurrentViewModelChanged(ViewModelBase? value)
    {
        Title = value?.Title ?? "";
    }

    private void NavigateToGame(StartGameEventArgs args)
    {
        GameService.StartGame(args.GameMode, args.KeymapData, args.NumberOfCards, args.IsTimedModeEnabled);
        
        var isTimedAddition = args.IsTimedModeEnabled ? ", Timed" : "";
        var title = $"[{args.GameMode.GetDescription()}{isTimedAddition}] {GameService.KeymapData.Software} / {GameService.KeymapData.KeyboardScheme}";
        
        CurrentViewModel = args.GameMode switch
        {
            GameMode.Classic => new ClassicGameViewModel
            {
                GameOverRequested = NavigateToGameOver,
                Title = title
            },
            GameMode.GuessByAction => new GuessByActionGameViewModel
            {
                GameOverRequested = NavigateToGameOver,
                Title = title
            },
            GameMode.GuessByKeymap => new GuessByKeymapGameViewModel
            {
                GameOverRequested = NavigateToGameOver,
                Title = title
            },
            GameMode.TypingTest => new TypingGameViewModel
            {
                GameOverRequested = NavigateToGameOver,
                Title = title
            },
            _ => throw new ArgumentOutOfRangeException(nameof(args.GameMode), args.GameMode, null)
        };
    }
    
    private void NavigateToGameOver(FinishGameEventArgs args)
    {
        if (!args.IsGameAborted)
        {
            GameService.FinishGame();
            CurrentViewModel = new GameOverViewModel
            {
                StartNewGameRequested = NavigateToSelectionMenu,
                Title = "Game Over"
            };
        }
        else
        {
            NavigateToSelectionMenu();
        }
    }

    private void NavigateToSelectionMenu()
    {
        CurrentViewModel = new SelectionScreenViewModel
        {
            StartLearningRequested = NavigateToGame,
            ShowStatisticsRequested = () =>
            {
                CurrentViewModel = new ShowStatisticsViewModel
                {
                    BackRequested = NavigateToSelectionMenu,
                    Title = "Statistics"
                };
            },
            DownloadManagerRequested = () =>
            {
                CurrentViewModel = new DownloadManagerViewModel
                {
                    BackRequested = NavigateToSelectionMenu,
                    Title = "Download Manager"
                };
            }
        };
    }
}