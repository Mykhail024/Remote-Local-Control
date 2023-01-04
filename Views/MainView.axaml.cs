using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using RLCClient.ViewModels;
using System.Threading.Tasks;

namespace RLCClient.Views;

public partial class MainView : Window
{

    public MainView()
    {
        InitializeComponent();
        MainViewModel mainViewModel = new MainViewModel();
        DataContext = mainViewModel;
//dadawd
        #region Testing
        Dispatcher.UIThread.Post(async () =>
        {
            await Task.Delay(1000);
            mainViewModel.BatteryChargingState = true;
            int i = 0;
            while (i < 101)
            {

                await Task.Delay(100);
                mainViewModel.BatteryPercent = i;
                i++;
            }
            await Task.Delay(1000);
            mainViewModel.BatteryChargingState = false;
        }, DispatcherPriority.Background);
        #endregion
    }

    #region TitleLabel
    public void BeginMoveDrag(object? sender, PointerPressedEventArgs e)
    {
        PlatformImpl?.BeginMoveDrag(e);
    }
    public void ButtonCloseClick(object sender, RoutedEventArgs e)
    {
        Close();
    }
    public void ButtonMinimizeClick(object sender, RoutedEventArgs e)
    {
        this.WindowState = WindowState.Minimized;
    }
    #endregion

}