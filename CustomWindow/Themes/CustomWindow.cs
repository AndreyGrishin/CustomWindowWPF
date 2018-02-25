using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace CustomWindow.Themes
{
    /// <summary>
    /// Extensions for window
    /// </summary>
    internal static class LocalExtensions
    {
        public static void ForWindowFromTemplate(this object templateFrameworkElement, Action<Window> action)
        {
            if (((FrameworkElement)templateFrameworkElement).TemplatedParent is Window window)
            {
                action(window);
            }
        }

        public static IntPtr GetWindowHandle(this Window window)
        {
            var helper = new WindowInteropHelper(window);
            return helper.Handle;
        }
    }

    public partial class CustomWindow
    {
        private const int TopPositionOffset = 8;
        private const int TopPaddingOffset = 6;
        private const int LeftPaddingOffset = 7;
        private const int RightPaddingOffest = 7;
        private const int BottomPaddingOffset = 5;
        private const int NoTopMost = -2;

        private const int NoZOrder = 0x0004;

        private static int _defaultShellWidth;
        private static int _defaultShellHeight;

        private static bool _isFirstLoaded = true;

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is Window windowSender)
            {
                windowSender.StateChanged += WindowStateChanged;

                _defaultShellHeight = (int)windowSender.RestoreBounds.Size.Height;
                _defaultShellWidth = (int)windowSender.RestoreBounds.Size.Width;
            }

            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;

            WindowStateChanged(sender, e);
        }
        /// <summary>
        /// Calls when StaticProperty of SystemParameters.WorkArea changed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void SystemParameters_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var windowSender = Application.Current.MainWindow;

            windowSender.Height = _defaultShellHeight;
            windowSender.Width = _defaultShellWidth;

            WindowStateChanged(windowSender, e);
        }

        /// <summary>
        /// Import user32.dll for SetWindowPos function.
        /// </summary>
        /// <param name="hWnd">handle of window</param>
        /// <param name="hWndInsertAfter">Z index</param>
        /// <param name="x">Left coord</param>
        /// <param name="y">Top coord</param>
        /// <param name="cx">Width</param>
        /// <param name="cy">Height</param>
        /// <param name="uFlags">Flags</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        /// <summary>
        /// Event handler for WindowStateChanged event. Resizes window to current work area if it Maximized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void WindowStateChanged(object sender, EventArgs e)
        {
            var window = (Window)sender;

            var handle = window.GetWindowHandle();
            var containerBorder = (Border)window.Template.FindName("PART_Container", window);

            if (window.WindowState == WindowState.Maximized)
            {
                var screen = System.Windows.Forms.Screen.FromHandle(handle);
                Rect workArea = SystemParameters.WorkArea;

                if (screen.Primary)
                {
                    if (_isFirstLoaded)
                    {
                        double screenWidth = SystemParameters.PrimaryScreenWidth;
                        double screenHeight = SystemParameters.PrimaryScreenHeight;
                        containerBorder.Padding = new Thickness(
                            workArea.Left + LeftPaddingOffset,
                            workArea.Top + TopPaddingOffset,
                            screenWidth - workArea.Right + RightPaddingOffest,
                            screenHeight - workArea.Bottom + BottomPaddingOffset);

                        _isFirstLoaded = false;
                    }
                    else
                    {
                        SetWindowPos(handle,
                            (IntPtr)NoTopMost,
                            (int)SystemParameters.WorkArea.Left,
                            (int)SystemParameters.WorkArea.Top - TopPositionOffset,
                            (int)SystemParameters.WorkArea.Width,
                            (int)SystemParameters.WorkArea.Height + TopPositionOffset,
                            NoZOrder);

                        containerBorder.Padding = new Thickness(0, TopPaddingOffset, 0, 0);
                    }
                }
            }
            else
            {
                containerBorder.Padding = new Thickness(0, 0, 0, 0);
            }
        }

        /// <summary>
        /// Minimized window state
        /// </summary>
        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(SystemCommands.MinimizeWindow);
        }

        /// <summary>
        /// Maximized application window or restore window state
        /// </summary>
        private void MaximizeButton_OnClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(window =>
            {
                if (window.WindowState == WindowState.Maximized)
                {
                    SystemCommands.RestoreWindow(window);
                }
                else
                {
                    SystemCommands.MaximizeWindow(window);
                }
            });
        }

        /// <summary>
        /// Close application window
        /// </summary>
        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            sender.ForWindowFromTemplate(SystemCommands.CloseWindow);
        }
    }
}
