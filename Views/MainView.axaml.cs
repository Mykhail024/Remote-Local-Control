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
using System.ComponentModel;
using System.Drawing.Text;
using System.Drawing.Printing;
using Avalonia;

namespace RLCClient.Views;

public partial class MainView : Window
{
    private bool SettingsTabElementsIsEnableState 
    {
        set
        {
            ClientName_TextBox.IsEnabled = value;
            ConnectionPort_TextBox.IsEnabled = value;
            ShutDownTimeOut_TextBox.IsEnabled = value;
            RestartTimeOut_TextBox.IsEnabled = value;

            ResetClientName_Button.IsEnabled = value;
            ResetPort_Button.IsEnabled = value;
            ResetShutdownTimeOut_Button.IsEnabled = value;
            ResetRestartTimeOut_Button.IsEnabled = value;
            ResetSettings_Button.IsEnabled = value;
        }
    }
    public TextBoxOutputter? outputter;
    public Settings? settings;

    public MainView()
    {
        InitializeComponent();
        
        MainViewModel mainViewModel = new MainViewModel();
        DataContext = mainViewModel;

        outputter = new TextBoxOutputter(LogOut);
        Console.SetOut(outputter);

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

        InitSettings();

        this.Closing += WindowClosing;
    }

    private void InitSettings()
    {
        try
        {
            settings = Settings.Load();

            ClientName_TextBox.Text = settings.ClientName;
            ConnectionPort_TextBox.Text = settings.Port.ToString();
            ShutDownTimeOut_TextBox.Text = settings.ShutdownTimeOut.ToString();
            RestartTimeOut_TextBox.Text = settings.RestartTimeOut.ToString();

            #region ResetButtonsClick
            ResetClientName_Button.Click += (_, __) =>
            {
                ClientName_TextBox.Text = Settings.DefaultSettings.ClientName;
            };
            ResetPort_Button.Click += (_, __) =>
            {
                ConnectionPort_TextBox.Text = Settings.DefaultSettings.Port.ToString();
            };
            ResetShutdownTimeOut_Button.Click += (_, __) =>
            {
                ShutDownTimeOut_TextBox.Text = Settings.DefaultSettings.ShutdownTimeOut.ToString();
            };
            ResetRestartTimeOut_Button.Click += (_, __) =>
            {
                RestartTimeOut_TextBox.Text = Settings.DefaultSettings.RestartTimeOut.ToString();
            };
            #endregion

            #region TextBoxPropertyChanged
            ConnectionPort_TextBox.PropertyChanged += (sender, __) =>
            {
                TextBox_OnlyNumberFilter(ref sender);
            };
            ShutDownTimeOut_TextBox.PropertyChanged += (sender, __) =>
            {
                TextBox_OnlyNumberFilter(ref sender);
            };
            RestartTimeOut_TextBox.PropertyChanged += (sender, __) =>
            {
                TextBox_OnlyNumberFilter(ref sender);
            };
            #endregion

            ResetSettings_Button.Click += async (_, __) =>
            {
                if (await MessageBox.Show(this, "Reset all settings to default?", MessageBox.MessageBoxButtons.YesNo, "Confirm request") == MessageBox.MessageBoxResult.Yes)
                {
                    ClientName_TextBox.Text = Settings.DefaultSettings.ClientName.ToString();
                    ConnectionPort_TextBox.Text = Settings.DefaultSettings.Port.ToString();
                    ShutDownTimeOut_TextBox.Text = Settings.DefaultSettings.ShutdownTimeOut.ToString();
                    RestartTimeOut_TextBox.Text = Settings.DefaultSettings.RestartTimeOut.ToString();

                }
            };

            ApplySettings_Button.Click += (_, __) =>
            {
                SettingsTabElementsIsEnableState = false;

                settings.ClientName = ClientName_TextBox.Text;
                settings.Port = int.Parse(ConnectionPort_TextBox.Text);
                settings.ShutdownTimeOut = int.Parse(ShutDownTimeOut_TextBox.Text);
                settings.RestartTimeOut = int.Parse(RestartTimeOut_TextBox.Text);

                settings.Save();

                SettingsTabElementsIsEnableState = true;
            };

            MainTabControl.SelectionChanged += async (_, __) =>
            {
                if ((MainTabControl.SelectedItem != SettingsTab) &&
                    (ClientName_TextBox.Text != settings.ClientName ||
                    ConnectionPort_TextBox.Text != settings.Port.ToString() ||
                    ShutDownTimeOut_TextBox.Text != settings.ShutdownTimeOut.ToString() ||
                    RestartTimeOut_TextBox.Text != settings.RestartTimeOut.ToString()))
                {
                    SettingsTab.IsSelected = true;
                    if (await MessageBox.Show(this, $"Save unsaved settings?", MessageBox.MessageBoxButtons.YesNo, "Confirm request") == MessageBox.MessageBoxResult.Yes)
                    {
                        SettingsTabElementsIsEnableState = false;

                        settings.ClientName = ClientName_TextBox.Text;
                        settings.Port = int.Parse(ConnectionPort_TextBox.Text);
                        settings.ShutdownTimeOut = int.Parse(ShutDownTimeOut_TextBox.Text);
                        settings.RestartTimeOut = int.Parse(RestartTimeOut_TextBox.Text);

                        settings.Save();

                        SettingsTabElementsIsEnableState = true;
                    }
                    else
                    {
                        ClientName_TextBox.Text = settings.ClientName;
                        ConnectionPort_TextBox.Text = settings.Port.ToString();
                        ShutDownTimeOut_TextBox.Text = settings.ShutdownTimeOut.ToString();
                        RestartTimeOut_TextBox.Text = settings.RestartTimeOut.ToString();
                    }
                }
            };

        }
        catch (JsonException)
        {
            Console.WriteLine("Error load Config file");
            SettingsTab.IsEnabled = false;
        }

    }

    private static void TextBox_OnlyNumberFilter(ref object? sender)
    {
        TextBox textBox = (TextBox)sender;

        for (ushort i = 0; i < textBox.Text?.Length; i++)
        {
            if (!Char.IsDigit(textBox.Text[i]))
            {
                textBox.Text = textBox.Text.Remove(i, 1);
            }
        }
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