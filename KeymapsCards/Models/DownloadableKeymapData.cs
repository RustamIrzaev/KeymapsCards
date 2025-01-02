namespace KeymapsCards.Models;

public class DownloadableKeymapData
{
    public string Software { get; set; }
    public string KeymapSchema { get; set; }
    public string FileVersion { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public bool IsInstalled { get; set; }
}