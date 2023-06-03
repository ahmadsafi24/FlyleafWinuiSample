using System;
using System.Runtime.InteropServices;
using Microsoft.UI;
using Microsoft.UI.Windowing;

namespace FlyleafWinuiSample.Helpers;

public static class HelperMethods
{            //this.SizeChanged += (_, _) =>
             //{
             //    double scaleAdjustment = GetScaleAdjustment(handle);

    //    gridDragRegion.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    //    var customDragRegionPosition = gridDragRegion.TransformToVisual(null).TransformPoint(new Point(0, 0));

    //    List<Windows.Graphics.RectInt32> dragRectsList = new();

    //    Windows.Graphics.RectInt32 dragRectL;
    //    dragRectL.X = (int)(customDragRegionPosition.X * scaleAdjustment);
    //    dragRectL.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
    //    dragRectL.Height = (int)((gridDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
    //    dragRectL.Width = (int)((gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectsList.Add(dragRectL);

    //    Windows.Graphics.RectInt32 dragRectR;
    //    dragRectR.X = (int)((customDragRegionPosition.X + gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectR.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
    //    dragRectR.Height = (int)((gridDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
    //    dragRectR.Width = (int)((gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectsList.Add(dragRectR);

    //    Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

    //    titleBar.SetDragRectangles(dragRects);
    //};

    //public static Window GetWindowForElement(UIElement element)
    //{
    //    if (element.XamlRoot != null)
    //    {
    //        foreach (Window window in _activeWindows)
    //        {
    //            if (element.XamlRoot == window.Content.XamlRoot)
    //            {
    //                return window;
    //            }
    //        }
    //    }
    //    return null;
    //}

    //public void OnTitleBarSizeChanged(object sender, SizeChangedEventArgs args)
    //{
    //    double scaleAdjustment = GetScaleAdjustment();

    //    gridDragRegion.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
    //    var customDragRegionPosition = gridDragRegion.TransformToVisual(null).TransformPoint(new Point(0, 0));

    //    List<Windows.Graphics.RectInt32> dragRectsList = new();

    //    Windows.Graphics.RectInt32 dragRectL;
    //    dragRectL.X = (int)(customDragRegionPosition.X * scaleAdjustment);
    //    dragRectL.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
    //    dragRectL.Height = (int)((gridDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
    //    dragRectL.Width = (int)((gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectsList.Add(dragRectL);

    //    Windows.Graphics.RectInt32 dragRectR;
    //    dragRectR.X = (int)((customDragRegionPosition.X + gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectR.Y = (int)(customDragRegionPosition.Y * scaleAdjustment);
    //    dragRectR.Height = (int)((gridDragRegion.ActualHeight - customDragRegionPosition.Y) * scaleAdjustment);
    //    dragRectR.Width = (int)((gridDragRegion.ActualWidth / 2) * scaleAdjustment);
    //    dragRectsList.Add(dragRectR);

    //    Windows.Graphics.RectInt32[] dragRects = dragRectsList.ToArray();

    //    appWindow.TitleBar.SetDragRectangles(dragRects);
    //}

    [DllImport("Shcore.dll", SetLastError = true)]
    private static extern int GetDpiForMonitor(IntPtr hmonitor, Monitor_DPI_Type dpiType, out uint dpiX, out uint dpiY);

    public static double GetScaleAdjustment(IntPtr hWnd)
    {
        WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
        DisplayArea displayArea = DisplayArea.GetFromWindowId(wndId, DisplayAreaFallback.Primary);
        IntPtr hMonitor = Win32Interop.GetMonitorFromDisplayId(displayArea.DisplayId);

        // Get DPI.
        int result = GetDpiForMonitor(hMonitor, Monitor_DPI_Type.MDT_Default, out uint dpiX, out uint _);
        if (result != 0)
        {
            throw new Exception("Could not get DPI for monitor.");
        }

        uint scaleFactorPercent = (uint)(((long)dpiX * 100 + (96 >> 1)) / 96);
        return scaleFactorPercent / 100.0;
    }

    internal enum Monitor_DPI_Type : int
    {
        MDT_Effective_DPI = 0,
        MDT_Angular_DPI = 1,
        MDT_Raw_DPI = 2,
        MDT_Default = MDT_Effective_DPI
    }
}
