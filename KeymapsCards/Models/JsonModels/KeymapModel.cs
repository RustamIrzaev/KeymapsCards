using Newtonsoft.Json;

namespace KeymapsCards.Models.JsonModels;

public class KeymapModel
{
    public string Command { get; set; }
    public string Keymap { get; set; }
    
    [JsonIgnore]
    public string Section { get; set; }

    public bool Disabled { get; set; } = false;
}