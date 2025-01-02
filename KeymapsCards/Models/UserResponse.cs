namespace KeymapsCards.Models;

public class UserResponse
{
    public string Command { get; set; }
    public string Keymap { get; set; }
    public string Section { get; set; }
    public bool IsKnown { get; set; }
}