using System;
using System.IO;
using System.Linq;
using ActiproSoftware.Extensions;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform;
using KeymapsCards.Helpers;
using KeymapsCards.Services;
using KeymapsCards.ViewModels;
using KeymapsCards.ViewModels.Game;
using KeymapsCards.Views;
using Microsoft.Extensions.DependencyInjection;

namespace KeymapsCards;

public partial class App : Application
{
    public IServiceProvider Services { get; } = InitProviders();
    public new static App? Current => (App)Application.Current;
    public string AppVersion { get; private set; }
    public string? KeymapJsonSchema { get; private set; }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        
        GetAssemblyFileVersion();
        LoadEmbeddedJsonKeymapSchema();
        FileHelper.EnsureKeymapsDirectoryExists();
    }
    
    public Window? GetMainWindowDescriptor()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }
    
        return null;
    }
    
    public override void OnFrameworkInitializationCompleted()
    {
        var vm = Services.GetRequiredService<MainViewModel>();
        Services.GetRequiredService<StatisticsService>().LoadStatistics();
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            desktop.MainWindow = new MainWindow
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void GetAssemblyFileVersion()
    {
        AppVersion = GetType().Assembly.GetFileVersion()?.ToString() ?? "";
    }

    private void LoadEmbeddedJsonKeymapSchema()
    {
        try
        {
            using var resource = AssetLoader.Open(new Uri("avares://KeymapsCards/Assets/keymap-schema.json"));
            using var reader = new StreamReader(resource);
            KeymapJsonSchema = reader.ReadToEnd();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    private static ServiceProvider InitProviders()
    {
        var collection = new ServiceCollection();
        
        collection.AddSingleton<GameService>();
        collection.AddSingleton<StatisticsService>();
        
        collection.AddTransient<MainViewModel>();
        collection.AddTransient<SelectionScreenViewModel>();
        collection.AddTransient<GameOverViewModel>();
        collection.AddTransient<ShowStatisticsViewModel>();
        collection.AddTransient<DownloadManagerViewModel>();
        
        collection.AddTransient<ClassicGameViewModel>();
        collection.AddTransient<TypingGameViewModel>();
        collection.AddTransient<GuessByActionGameViewModel>();
        collection.AddTransient<GuessByKeymapGameViewModel>();
        
        return collection.BuildServiceProvider();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}