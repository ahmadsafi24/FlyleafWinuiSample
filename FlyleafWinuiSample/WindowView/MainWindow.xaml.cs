using System;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using WinRT.Interop;
using WinuiHelperLib;

namespace FlyleafWinuiSample.WindowView
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region backdrop
            IntPtr handle = WindowNative.GetWindowHandle(this);
            var windowFrame = new WinuiHelperLib.UI.Window.WindowManager();
            windowFrame.ToggleImmersiveDarkMode(handle, true);
            windowFrame.ExtendFrameIntoClientArea(handle);
            windowFrame.SetBackdropType(handle, WinuiHelperLib.UI.Window.BackdropType.Mica);
            this.SystemBackdrop = new Microsoft.UI.Xaml.Media.MicaBackdrop();
            #endregion

            AppWindowTitleBar titleBar = AppWindow.TitleBar;
            titleBar.ExtendsContentIntoTitleBar = true;
            titleBar.IconShowOptions = IconShowOptions.HideIconAndSystemMenu;

            var transparent = Microsoft.UI.Colors.Transparent;
            titleBar.BackgroundColor = transparent;
            titleBar.ButtonBackgroundColor = transparent;
            titleBar.InactiveBackgroundColor = transparent;
            titleBar.ButtonInactiveBackgroundColor = transparent;

            this.Content.DoubleTapped += Content_DoubleTapped;
            this.AppWindow.Closing += AppWindow_Closing;
        }

        private void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
        {
            Player player = App.MainViewModel.Player;
            if (player.Status != Status.Stopped)
            {
                player.Stop();
            }
        }

        bool isFullscreen = false;
        private void Content_DoubleTapped(object sender, Microsoft.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            if (isFullscreen == false)
            {
                AppWindow.Hide();
                AppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                isFullscreen = true;
                AppWindow.Show();
            }
            else
            {
                AppWindow.Hide();
                AppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
                OverlappedPresenter overlappedPresenter = AppWindow.Presenter as OverlappedPresenter;
                isFullscreen = false;
                AppWindow.Show();
            }
        }
    }
}