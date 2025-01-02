using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using KeymapsCards.Helpers;
using KeymapsCards.Models;

namespace KeymapsCards.ViewModels;

public class DownloadManagerViewModel : ViewModelBase
{
    public ObservableCollection<DownloadableKeymapData> AvailableKeymaps { get; } = [];
    public required Action BackRequested { get; init; }

    public DownloadManagerViewModel()
    {
        LoadLocalKeymaps();
    }
    
    private void LoadLocalKeymaps()
    {
        var localKeymaps = FileHelper.LoadAllKeymapFiles()
            .OrderBy(q => q.Software)
            .ThenBy(q => q.KeyboardScheme)
            .ToList();

        foreach (var keymap in localKeymaps)
        {
            AvailableKeymaps.Add(new DownloadableKeymapData
            {
                Software = keymap.Software,
                FileVersion = keymap.FileVersion,
                KeymapSchema = keymap.KeyboardScheme,
                Description = keymap.Description,
                Author = keymap.Author,
                IsInstalled = true
            });
        }
    }
    
    public void GoBack()
    {
        BackRequested.Invoke();
    }

    public void OpenKeymapsGitHub()
    {
        Process.Start(new ProcessStartInfo(AppData.KeymapsGitHubUrl) { UseShellExecute = true });
    }
}