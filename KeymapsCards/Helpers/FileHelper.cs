using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using KeymapsCards.Models.JsonModels;
using Newtonsoft.Json;

namespace KeymapsCards.Helpers;

public static class FileHelper
{
    private const string KeymapsDirectory = "KeymapsData";
        
    public static void EnsureKeymapsDirectoryExists()
    {
        if (!Directory.Exists(KeymapsDirectory))
            Directory.CreateDirectory(KeymapsDirectory);
    }
    
    public static List<KeymapData> LoadAllKeymapFiles()
    {
        var keymapFiles = Directory.GetFiles(KeymapsDirectory, "keymaps-*.json", SearchOption.AllDirectories);
        var allKeymaps = new List<KeymapData>();

        try
        {
            foreach (var file in keymapFiles)
            {
                var json = File.ReadAllText(file);

                if (!JsonValidator.IsJsonSchemaValid(json, file))
                    continue;
                
                var keymapData = JsonConvert.DeserializeObject<KeymapData>(json);

                if (keymapData == null) 
                    continue;
                
                if (keymapData.Disabled)
                    continue;
                
                foreach (var keymap in keymapData.Keymaps)
                {
                    keymap.Commands = keymap.Commands.Where(q => !q.Disabled).ToList();
                }
                    
                keymapData.TotalCommands = keymapData.Keymaps.Sum(q => q.Commands.Count);
                    
                allKeymaps.Add(keymapData);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return allKeymaps;
    }
}