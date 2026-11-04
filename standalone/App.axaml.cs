using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using standalone.ViewModels;
using standalone.Views;
using System.Linq;

namespace standalone
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                DisableAvaloniaDataAnnotationValidation(); // cos default od avaloni idk

                var splashScreen = new SplashscreenWindow();
                desktop.MainWindow = splashScreen;
                splashScreen.Show();

                splashScreen.KeyDown += (_, e) =>
                {
                    if (e.Key == Key.Space)
                    {
                        var mainWindow = new MainWindow
                        {
                            DataContext = new MainWindowViewModel(),
                        };
                        desktop.MainWindow = mainWindow;
                        mainWindow.Show();
                        splashScreen.Close();
                    }
                };
            }
            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators
                    .OfType<DataAnnotationsValidationPlugin>()
                    .ToArray();

            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}