using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace RLCClient.ViewModels;

public partial class MainViewModel : ReactiveObject
{
    #region Battery
    private int _BatteryPercent = 0;
    public int BatteryPercent
    {
        get => _BatteryPercent;
        set
        {
            if (value < 0)
            {
                this.RaiseAndSetIfChanged(ref _BatteryPercent, 0);
                throw new Exception("Value < 0");
            }
            else if (value > 100)
            {
                this.RaiseAndSetIfChanged(ref _BatteryPercent, 100);
                throw new Exception("Value > 0");
            }
            else
            {
                this.RaiseAndSetIfChanged(ref _BatteryPercent, value);
            }

            this.RaisePropertyChanged(nameof(BatteryPercentString));

            #region ImagePercent
            if (_BatteryPercent == 100) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.100.png"));
            else if (_BatteryPercent >= 90) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.90.png"));
            else if (_BatteryPercent >= 80) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.80.png"));
            else if (_BatteryPercent >= 70) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.70.png"));
            else if (_BatteryPercent >= 60) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.60.png"));
            else if (_BatteryPercent >= 50) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.50.png"));
            else if (_BatteryPercent >= 40) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.40.png"));
            else if (_BatteryPercent >= 30) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.30.png"));
            else if (_BatteryPercent >= 20) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.20.png"));
            else if (_BatteryPercent >= 10) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.10.png"));
            else if (_BatteryPercent >= 0) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.0.png"));
            #endregion
        }
    }
    public string BatteryPercentString
    {
        get => _BatteryPercent + "%";
    }

    private bool _BatteryChargingState = false;
    public bool BatteryChargingState
    {
        get => _BatteryChargingState;
        set
        {
            if (value == true)
            {
                BatteryPercent_TextBox_Margin = new Thickness(4, 0);
            }
            else if (value == false)
            {
                BatteryPercent_TextBox_Margin = new Thickness(-6, 0);
            }
            this.RaiseAndSetIfChanged(ref _BatteryChargingState, value);
        }
    }

    private Thickness _BatteryPercent_TextBox_Margin = new Thickness(-6, 0);
    public Thickness BatteryPercent_TextBox_Margin
    {
        get => _BatteryPercent_TextBox_Margin;
        set
        {
            this.RaiseAndSetIfChanged(ref _BatteryPercent_TextBox_Margin, value);
        }
    }

    private Bitmap _BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.0.png"));
    public Bitmap BatteryImage
    {
        get => _BatteryImage;
        private set => this.RaiseAndSetIfChanged(ref _BatteryImage, value);
    }

    #endregion

    public List<Client> Clients { get; set; } = new()
        {
            new ("HONG", "129.123.43.1", 25565, OperatingSystem.Windows, 0),
            new ("HONG19", "172.52.32.69", 25565, OperatingSystem.Linux, 134),
            new ("Admin", "127.0.0.1", 25565, OperatingSystem.Windows, 42),
            new ("Anast", "127.0.0.1", 25565, OperatingSystem.Android, 302),
            new ("Drag", "127.0.0.1", 25565, OperatingSystem.Windows, 98),
            new ("LAstyl", "127.0.0.1", 25565, OperatingSystem.MacOS, 168),
            new ("test", "255.255.255.255", 25565, OperatingSystem.MacOS, 431),

        };

}
