using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using KeymapsCards.Models;
using KeymapsCards.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KeymapsCards.ViewModels;

public partial class ShowStatisticsViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isClearStatisticsRequested;
    private readonly StatisticsService _statisticsService;
    public ObservableCollection<Statistics> StatisticsData { get; }
    public required Action BackRequested { get; init; }

    public ShowStatisticsViewModel()
    {
        _statisticsService = (App.Current?.Services.GetRequiredService(typeof(StatisticsService)) as StatisticsService)!;
        StatisticsData = new ObservableCollection<Statistics>(_statisticsService.Statistics);
    }
    
    public void GoBack()
    {
        BackRequested.Invoke();
    }
    
    public void ClearStatistics()
    {
        if (IsClearStatisticsRequested)
        {
            _statisticsService.ClearStatistics();
            StatisticsData.Clear();
            IsClearStatisticsRequested = false;
        }
        else
            IsClearStatisticsRequested = true;
    }
    
    public void CancelClearStatistics()
    {
        IsClearStatisticsRequested = false;
    }
}