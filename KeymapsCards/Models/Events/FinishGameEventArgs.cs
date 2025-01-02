using System;

namespace KeymapsCards.Models.Events;

public class FinishGameEventArgs(bool isGameAborted) : EventArgs
{
    public bool IsGameAborted { get; init; } = isGameAborted;
}