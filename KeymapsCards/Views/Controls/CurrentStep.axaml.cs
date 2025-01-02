using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace KeymapsCards.Views.Controls;

public partial class CurrentStep : UserControl
{
    public static readonly StyledProperty<string> MessageProperty =
        AvaloniaProperty.Register<CurrentStep, string>(nameof(Message));

    public string Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
    
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonWithIcon, ICommand>(nameof(Command));

    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    
    public CurrentStep()
    {
        InitializeComponent();
    }
}