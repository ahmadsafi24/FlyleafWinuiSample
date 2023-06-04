using FlyleafWinuiSample.ViewModels;
using Microsoft.UI.Xaml;

namespace FlyleafWinuiSample;
public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();
        m_window.Activate();
    }

    private Window m_window;

    #region Todo: use singleton dep injection
    public static MainViewModel MainViewModel { get; } = new();

    #endregion
}

