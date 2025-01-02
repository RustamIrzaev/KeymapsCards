using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace KeymapsCards.Models.JsonModels;

public class KeymapData
{
    public string Software { get; set; }
    [JsonProperty("keyboard_scheme")] 
    public string KeyboardScheme { get; set; }
    [JsonProperty("file_version")] 
    public string FileVersion { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    [JsonProperty("repo")] 
    public string Repository { get; set; }
    public List<KeymapSection> Keymaps { get; set; }
    public bool Disabled { get; set; } = false;
    public Guid Id { get; set; }
    
    [JsonIgnore]
    public int TotalCommands { get; set; }
}