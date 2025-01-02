using System.Collections.Generic;

namespace KeymapsCards.Models.JsonModels;

public class KeymapSection
{
    public string Section { get; set; }
    public List<KeymapModel> Commands { get; set; }
}