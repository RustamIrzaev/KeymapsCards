using CommunityToolkit.Mvvm.ComponentModel;
using KeymapsCards.Services;
using Microsoft.Extensions.DependencyInjection;

namespace KeymapsCards.ViewModels;

public class ViewModelBase : ObservableObject
{
    private string _title = $"Keymaps Cards {App.Current?.AppVersion}";
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    protected GameService GameService { get; }
    
    protected ViewModelBase()
    {
        GameService = (App.Current?.Services.GetRequiredService(typeof(GameService)) as GameService)!;
    }
}