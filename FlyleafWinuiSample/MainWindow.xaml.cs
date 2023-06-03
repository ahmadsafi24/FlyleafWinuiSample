using System;
using FlyleafLib;
using FlyleafLib.MediaPlayer;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using UHelper;
using WinRT.Interop;

namespace FlyleafWinuiSample
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            #region backdrop
            IntPtr handle = WindowNative.GetWindowHandle(this);
            WindowsFrame.ToggleImmersiveDarkMode(handle, true);
            WindowsFrame.ExtendFrameIntoClientArea(handle);
            WindowsFrame.SetBackdropType(handle, BackdropType.Mica);
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
                AppWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                isFullscreen = true;
            }
            else
            {
                AppWindow.SetPresenter(AppWindowPresenterKind.Overlapped);
                OverlappedPresenter overlappedPresenter = AppWindow.Presenter as OverlappedPresenter;
                isFullscreen = false;
            }
        }
    }
}