using System;
using KeymapsCards.Models.Enums;
using KeymapsCards.Models.JsonModels;

namespace KeymapsCards.Models.Events;

public class StartGameEventArgs : EventArgs
{
    public KeymapData KeymapData { get; init; }
    public int NumberOfCards { get; init; }
    public GameMode GameMode { get; init; }
    public bool IsTimedModeEnabled { get; init; }
}