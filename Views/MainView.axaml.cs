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
    public Settings CurrentSettings
    {
        get
        {
            Settings _settings = new Settings()
            {
                ClientName = ClientName_TextBox.Text,
                Port = int.Parse(ConnectionPort_TextBox.Text),
                ShutdownTimeOut = int.Parse(ShutDownTimeOut_TextBox.Text),
                RestartTimeOut = int.Parse(RestartTimeOut_TextBox.Text)
            };
            return _settings;
        }
        set
        {
            ClientName_TextBox.Text = value.ClientName;
            ConnectionPort_TextBox.Text = value.Port.ToString();
            ShutDownTimeOut_TextBox.Text = value.ShutdownTimeOut.ToString();
            RestartTimeOut_TextBox.Text = value.RestartTimeOut.ToString();
        }
    }
    private bool isSettingsSaved
    {
        get
        {
            if (CurrentSettings.Equals(Settings.Load()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
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

            CurrentSettings = Settings.Load();

            #region ResetButtonsClick
            ResetClientName_Button.Click += (_, __) =>
            {
                CurrentSettings = new Settings 
                { 
                    ClientName = Settings.DefaultSettings.ClientName, 
                    Port = CurrentSettings.Port, 
                    ShutdownTimeOut = CurrentSettings.ShutdownTimeOut, 
                    RestartTimeOut = CurrentSettings.RestartTimeOut
                };
            };
            ResetPort_Button.Click += (_, __) =>
            {
                CurrentSettings = new Settings 
                { 
                    ClientName = CurrentSettings.ClientName, 
                    Port = Settings.DefaultSettings.Port, 
                    ShutdownTimeOut = CurrentSettings.ShutdownTimeOut, 
                    RestartTimeOut = CurrentSettings.RestartTimeOut 
                };
            };
            ResetShutdownTimeOut_Button.Click += (_, __) =>
            {
                CurrentSettings = new Settings 
                { 
                    ClientName = CurrentSettings.ClientName, 
                    Port = CurrentSettings.Port, 
                    ShutdownTimeOut = Settings.DefaultSettings.ShutdownTimeOut, 
                    RestartTimeOut = CurrentSettings.RestartTimeOut 
                };
            };
            ResetRestartTimeOut_Button.Click += (_, __) =>
            {
                CurrentSettings = new Settings 
                { 
                    ClientName = CurrentSettings.ClientName, 
                    Port = CurrentSettings.Port, 
                    ShutdownTimeOut = CurrentSettings.ShutdownTimeOut, 
                    RestartTimeOut = Settings.DefaultSettings.RestartTimeOut 
                };
            };
            ResetSettings_Button.Click += async (_, __) =>
            {
                if (await MessageBox.Show(this, "Reset all settings to default?", MessageBox.MessageBoxButtons.YesNo, "Confirm request") == MessageBox.MessageBoxResult.Yes)
                {
                    CurrentSettings = Settings.DefaultSettings;
                }
            };

            #endregion

            #region TextBoxPropertyChanged
            ConnectionPort_TextBox.PropertyChanged += TextBox_OnlyNumberFilter;
            ShutDownTimeOut_TextBox.PropertyChanged += TextBox_OnlyNumberFilter;
            RestartTimeOut_TextBox.PropertyChanged += TextBox_OnlyNumberFilter;
            #endregion

            ApplySettings_Button.Click += (_, __) =>
            {
                SaveCurrentSettingsToConfig();
            };

            //If settings not saved
            MainTabControl.SelectionChanged += async (_, __) =>
            {
                if (!isSettingsSaved && MainTabControl.SelectedItem != SettingsTab)
                {
                    SettingsTab.IsSelected = true;
                    if (await MessageBox.Show(this, $"Save unsaved settings?", MessageBox.MessageBoxButtons.YesNo, "Confirm request") == MessageBox.MessageBoxResult.Yes)
                    {
                        SaveCurrentSettingsToConfig();
                    }
                    else
                    {
                        CurrentSettings = Settings.Load();
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

    private void SaveCurrentSettingsToConfig()
    {
        SettingsTabElementsIsEnableState = false;

        CurrentSettings.Save();

        SettingsTabElementsIsEnableState = true;
    }

    private static void TextBox_OnlyNumberFilter(object? sender, EventArgs e)
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

    private async void WindowClosing(object? sender, CancelEventArgs e)
    {
        e.Cancel = true;
        async void Close(bool Dialog)
        {
            Window window = (Window)sender;

            if(Dialog)
            {

                if (await MessageBox.Show(window, "Disconnect and close program?", MessageBox.MessageBoxButtons.OkCancel) == MessageBox.MessageBoxResult.Ok)
                {
                    Close();
                }
            }
            else
            {
                Close();
            }

            void Close()
            {
                window.Closing -= WindowClosing;
                window.Close();
            }
        }

        if(isSettingsSaved)
        {
            Close(true);
        }
        else
        {
            if (await MessageBox.Show(this, $"Save unsaved settings?", MessageBox.MessageBoxButtons.YesNo, "Confirm request") == MessageBox.MessageBoxResult.Yes)
            {
                SaveCurrentSettingsToConfig();
                Close(false);
            }
            else
            {
                CurrentSettings = Settings.Load();
                Close(true);
            }
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