using Avalonia.Controls;
using Avalonia.Input;
using KeymapsCards.ViewModels.Game;

namespace KeymapsCards.Views.Game;

public partial class TypingGameView : UserControl
{
    public TypingGameView()
    {
        InitializeComponent();

        AttachedToVisualTree += (_, _) => Focus();
    }
    
    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (DataContext is TypingGameViewModel vm)
        {
            var key = e.Key.ToString();

            if (e.KeyModifiers != KeyModifiers.None)
            {
                key = $"{e.KeyModifiers}+{key}";
            }
            
            vm.HandleKeyInput(key);
        }
    }
}