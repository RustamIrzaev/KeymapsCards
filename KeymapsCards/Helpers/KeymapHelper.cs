using System;
using KeymapsCards.Models.JsonModels;

namespace KeymapsCards.Helpers;

public static class KeymapHelper
{
    public static void ReplaceKeymapSymbols(KeymapSection section)
    {
        foreach (var command in section.Commands)
        {
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
            {
                command.Keymap = command.Keymap
                    .Replace("<Command>", "⌘")
                    .Replace("<Option>", "⌥")
                    .Replace("<Shift>", "⇧")
                    .Replace("<Control>", "⌃");
            }
            else if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
            {
                command.Keymap = command.Keymap
                    .Replace("<Command>", "Cmd")
                    .Replace("<Option>", "Alt")
                    .Replace("<Shift>", "Shift")
                    .Replace("<Control>", "Ctrl");
            }

            command.Keymap = command.Keymap
                .Replace("<Space>", "␣")
                .Replace("<Delete>", "⌫")
                .Replace("<Enter>", "↩")
                .Replace("<Return>", "↩")
                .Replace("<Tab>", "⇥")
                .Replace("-", "–")
                .Replace("`", "ˋ")
                // .Replace("+", "⌧")
                .Replace("<Up>", "↑")
                .Replace("<Down>", "↓")
                .Replace("<Left>", "←")
                .Replace("<Right>", "→");
        }
    }
}