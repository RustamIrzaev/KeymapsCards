using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using KeymapsCards.Extensions;
using KeymapsCards.Helpers;
using KeymapsCards.Models.Enums;
using KeymapsCards.Models.Events;
using KeymapsCards.Models.JsonModels;

namespace KeymapsCards.ViewModels;

public partial class SelectionScreenViewModel : ViewModelBase
{
    public ObservableCollection<string> SoftwareList { get; set; }
    public ObservableCollection<string> KeyboardSchemes { get; set; }
    public ObservableCollection<GameMode> GameModes { get; set; }
    public int MinNumberOfCards => AppData.MinNumberOfCards;
    public int MaxNumberOfCards => AppData.MaxNumberOfCards;
    public string StartGameText => IsTimedModeEnabled ? "Start Timed Game" : "Start Game";
    public bool CanTimedModeBeApplied => SelectedGameMode == GameMode.Classic;
    public required Action<StartGameEventArgs> StartLearningRequested { get; init; }
    public required Action ShowStatisticsRequested { get; init; }
    public required Action DownloadManagerRequested { get; init; }
    
    [ObservableProperty] private string _selectedSoftware;
    [ObservableProperty] private string? _selectedKeyboardScheme;
    [ObservableProperty] private GameMode _selectedGameMode;
    [ObservableProperty] private KeymapData _selectedKeymapData;
    [ObservableProperty] private int _numberOfCards = AppData.InitialNumberOfCards;
    [ObservableProperty] private bool _isTimedModeEnabled;
    
    private readonly Dictionary<string, List<KeymapData>> _keymapData;
    
    public SelectionScreenViewModel()
    {
        _keymapData = FileHelper.LoadAllKeymapFiles()
            .GroupBy(q => q.Software)
            .OrderBy(q => q.Key)
            .ToDictionary(q => q.Key, q => q.ToList());

        GameModes = new ObservableCollection<GameMode>(Enum.GetValues<GameMode>()
            .Where(mode => !mode.IsDisabled())
            .ToList());
        SoftwareList = new ObservableCollection<string>(_keymapData.Keys);

        if (SoftwareList.Any())
            SelectedSoftware = SoftwareList.First();

        if (GameModes.Any())
            SelectedGameMode = GameModes.First();
    }
    
    partial void OnIsTimedModeEnabledChanged(bool value)
    {
        OnPropertyChanged(nameof(StartGameText));
    }
    
    partial void OnSelectedGameModeChanged(GameMode value)
    {
        OnPropertyChanged(nameof(CanTimedModeBeApplied));
        
        if (!CanTimedModeBeApplied)
            IsTimedModeEnabled = false;
    }

    partial void OnSelectedKeyboardSchemeChanged(string? value)
    {
        SelectedKeymapData = _keymapData[SelectedSoftware].First(k => k.KeyboardScheme == SelectedKeyboardScheme);
        OnPropertyChanged(nameof(SelectedKeymapData));
    }
    
    partial void OnSelectedSoftwareChanged(string value)
    {
        if (_keymapData.TryGetValue(SelectedSoftware, out var keymaps))
        {
            KeyboardSchemes = new ObservableCollection<string>(keymaps.Select(k => k.KeyboardScheme).OrderBy(q => q));

            OnPropertyChanged(nameof(KeyboardSchemes));
            
            SelectedKeyboardScheme = KeyboardSchemes.Any() ? KeyboardSchemes.First() : null;
            OnPropertyChanged(nameof(SelectedKeyboardScheme));
        }
        else
        {
            KeyboardSchemes.Clear();
        }
    }
    
    public void StartLearning()
    {
        StartLearningRequested.Invoke(new StartGameEventArgs
        {
            KeymapData = SelectedKeymapData,
            NumberOfCards = NumberOfCards,
            GameMode = SelectedGameMode,
            IsTimedModeEnabled = IsTimedModeEnabled
        });
    }

    public void ShowStatistics()
    {
        ShowStatisticsRequested.Invoke();
    }

    public void KeymapDownloadManager()
    {
        DownloadManagerRequested.Invoke();
    }
    
    public void RandomizeCards()
    {
        if (MinNumberOfCards > SelectedKeymapData.TotalCommands)
        {
            NumberOfCards = new Random().Next(SelectedKeymapData.TotalCommands, Math.Min(SelectedKeymapData.TotalCommands, MaxNumberOfCards));
            return;
        }
        
        NumberOfCards = new Random().Next(MinNumberOfCards, Math.Min(SelectedKeymapData.TotalCommands, MaxNumberOfCards));
    }

    public void OpenRepositoryUrl()
    {
        var rawUrl = SelectedKeymapData.Repository;
        
        if (Uri.TryCreate(rawUrl, UriKind.Absolute, out var url))
        {
            Process.Start(new ProcessStartInfo(url.AbsoluteUri) { UseShellExecute = true });
        }
    }
}