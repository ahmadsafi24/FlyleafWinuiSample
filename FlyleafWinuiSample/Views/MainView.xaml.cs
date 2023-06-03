using System;
using System.Collections.Generic;
using FlyleafWinuiSample.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Windows.Graphics;
using Windows.Storage;
using WinuiHelperLib.UI.Window;
namespace FlyleafWinuiSample.Views;

public sealed partial class MainView : Page
{
    private readonly MainViewModel MainViewModel = App.MainViewModel;
    public MainView()
    {
        DataContext = MainViewModel;
        this.Loaded += MainView_Loaded;
        this.AllowDrop = true;
        this.InitializeComponent();
    }

    WindowId parentwinid;
    AppWindow appWindow;
    AppWindowTitleBar appWindowTitleBar;
    IntPtr parentWindow_handle;
    private void MainView_Loaded(object sender, RoutedEventArgs e)
    {
        parentwinid = this.XamlRoot.ContentWindow.WindowId;
        appWindow = AppWindow.GetFromWindowId(parentwinid);
        appWindowTitleBar = appWindow.TitleBar;
        parentWindow_handle = Win32Interop.GetWindowFromWindowId(parentwinid);
        SizeChanged += MainView_SizeChanged;
        MainView_SizeChanged(null, null);
    }

    //rectposleft= NavigationBackButtonWidth + PaneToggleButtonWidth - 4(borders)
    const int rectposleft = 84;
    const int rectpostop = 0;
    int rectheight;
    int rectwidth;
    double scaleadj;
    private void MainView_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        rectheight = appWindowTitleBar.Height;
        rectwidth = appWindow.Size.Width - rectposleft;
        scaleadj = WindowManager.GetScaleAdjustment(parentWindow_handle);

        List<RectInt32> dragablerects = new() { new RectInt32(rectposleft * (int)scaleadj, rectpostop * (int)scaleadj, rectwidth * (int)scaleadj, rectheight * (int)scaleadj) };

        appWindowTitleBar.SetDragRectangles(dragablerects.ToArray());
    }

    protected override void OnDragOver(DragEventArgs e)
    {
        base.OnDragOver(e);
    }

    protected override void OnDragEnter(DragEventArgs e)
    {
        base.OnDragEnter(e);
        e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Link;
        e.DragUIOverride.Caption = "Open";
    }

    protected override void OnDrop(DragEventArgs e)
    {
        base.OnDrop(e);

        IReadOnlyList<IStorageItem> items = e.DataView.GetStorageItemsAsync().GetAwaiter().GetResult();
        DispatcherQueue.TryEnqueue(() =>
        {
            if (items[0].IsOfType(StorageItemTypes.File))
            {
                MainViewModel.Player.Open(items[0].Path);
            }
        });
    }

    protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (e.GetCurrentPoint(this).Properties.MouseWheelDelta > 0)
        {
            MainViewModel.Player.Audio.Volume += 5;
        }
        else
        {
            MainViewModel.Player.Audio.Volume -= 5;
        }
    }



    public bool IsControlsVisible
    {
        get { return (bool)GetValue(IsControlsVisibleProperty); }
        set { SetValue(IsControlsVisibleProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsControlsVisible.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsControlsVisibleProperty =
        DependencyProperty.Register("IsControlsVisible", typeof(bool), typeof(MainView), new PropertyMetadata(true, IsControlsVisibleChanged));

    private static void IsControlsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        MainView mainView = d as MainView;
        bool isControlsVisible = (bool)mainView.GetValue(e.Property);
        WindowId parentwinid = mainView.XamlRoot.ContentWindow.WindowId;
        AppWindow appWindow = AppWindow.GetFromWindowId(parentwinid);

        if (appWindow.Presenter.Kind is AppWindowPresenterKind.Overlapped or AppWindowPresenterKind.Default)
        {
            OverlappedPresenter overlappedPresenter = appWindow.Presenter as OverlappedPresenter;
            overlappedPresenter.SetBorderAndTitleBar(isControlsVisible, isControlsVisible);
        }
    }
}
