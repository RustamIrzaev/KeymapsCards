using Avalonia;
using Avalonia.Controls;

namespace KeymapsCards.Views.Controls;

public partial class TimeLeftProgress : UserControl
{
    public static readonly StyledProperty<int> MaximumTimeLimitProperty =
        AvaloniaProperty.Register<TimeLeftProgress, int>(nameof(MaximumTimeLimit));

    public int MaximumTimeLimit
    {
        get => GetValue(MaximumTimeLimitProperty);
        set => SetValue(MaximumTimeLimitProperty, value);
    }

    public static readonly StyledProperty<int> TimeLeftProperty =
        AvaloniaProperty.Register<TimeLeftProgress, int>(nameof(TimeLeft));

    public int TimeLeft
    {
        get => GetValue(TimeLeftProperty);
        set => SetValue(TimeLeftProperty, value);
    }
    
    public TimeLeftProgress()
    {
        InitializeComponent();
    }
}