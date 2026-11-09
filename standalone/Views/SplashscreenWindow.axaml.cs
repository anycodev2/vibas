using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace standalone.Views;

public partial class SplashscreenWindow : Window
{
    // RecentProjects and community button colors
    private static readonly IBrush ActiveBackground = Brush.Parse("#22A7F2");
    private static readonly IBrush ActiveForeground = Brushes.White;
    private static readonly IBrush InactiveBackground = Brushes.White;
    private static readonly IBrush InactiveForeground = Brushes.Black;
    public SplashscreenWindow()
    {
        InitializeComponent();
    }
    private void ButtonClick_recentProjects(object? sender, RoutedEventArgs e) 
        => SetActive(ButtonRecentProjects);
    private void ButtonClick_community(object? sender, RoutedEventArgs e)
        => SetActive(ButtonCommunity);
    private void SetActive(Button active)
    {
        foreach (var button in new[] {ButtonRecentProjects, ButtonCommunity})
        {
            bool isActive = button == active;
            button.Background = isActive ? ActiveBackground : InactiveBackground;
            button.Foreground = isActive ? ActiveForeground : InactiveForeground;
        }
    }
}