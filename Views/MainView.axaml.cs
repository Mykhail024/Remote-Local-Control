using System;
using System.IO;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using RLCClient.ViewModels;
using System.Threading.Tasks;
using System.Text.Json;
using Avalonia.DesignerSupport.Remote.HtmlTransport;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.ComponentModel;
using System.Drawing.Text;
using System.Drawing.Printing;

namespace RLCClient.Views;

public partial class MainView : Window
{
    public TextBoxOutputter? outputter;
    public Settings? settings;

    public MainView()
    {
        InitializeComponent();
        

        MainViewModel mainViewModel = new MainViewModel();
        DataContext = mainViewModel;

        outputter = new TextBoxOutputter(LogOut);
        Console.SetOut(outputter);



        #region Config
        try
        {
            settings = Settings.Load();
        }
        catch(JsonException)
        {
            Console.WriteLine("Error load Config file");
        }
        #endregion

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

        this.Closing += WindowClosing;
    }

    private static async void WindowClosing(object? sender, CancelEventArgs e)
    {
        Window window = (Window)sender;

        e.Cancel = true;
        if (await MessageBox.Show(window, "Disconnect and close program?", MessageBox.MessageBoxButtons.OkCancel) == MessageBox.MessageBoxResult.Ok)
        {

            window.Closing -= WindowClosing;
            window.Close();
        }
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