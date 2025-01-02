using System;

namespace KeymapsCards.ViewModels.Game;

public class TypingGameViewModel : GameBaseViewModel
{
    public void HandleKeyInput(string key)
    {
        Console.WriteLine(key);
    }
}