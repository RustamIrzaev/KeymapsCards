using System.ComponentModel;
using KeymapsCards.Attributes;

namespace KeymapsCards.Models.Enums;

public enum GameMode
{
    Classic,
    [Description("Guess by Action")]
    GuessByAction,
    [Description("Guess by Keymap")]
    GuessByKeymap,
    [Description("Typing Test")]
    [DisabledValue]
    TypingTest
}