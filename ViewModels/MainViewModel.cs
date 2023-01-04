using System;
using System.IO;
using Avalonia.Media.Imaging;
using ReactiveUI;
using System.Reflection;

namespace RLCClient.ViewModels
{
    public class MainViewModel : ReactiveObject
    {
        private int _BatteryPercent = 0;
        public int BatteryPercent
        {
            get => _BatteryPercent;
            set
            {
                if(value < 0)
                {
                    this.RaiseAndSetIfChanged(ref _BatteryPercent, 0);
                    throw new Exception("Value < 0");
                }
                else if(value > 100)
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
                else if(_BatteryPercent >= 90) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.90.png"));
                else if(_BatteryPercent >= 80) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.80.png"));
                else if(_BatteryPercent >= 70) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.70.png"));
                else if(_BatteryPercent >= 60) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.60.png"));
                else if(_BatteryPercent >= 50) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.50.png"));
                else if(_BatteryPercent >= 40) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.40.png"));
                else if(_BatteryPercent >= 30) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.30.png"));
                else if(_BatteryPercent >= 20) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.20.png"));
                else if(_BatteryPercent >= 10) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.10.png"));
                else if(_BatteryPercent >= 0) BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.0.png"));
                #endregion
            }
        }
        public string BatteryPercentString
        {
            get
            {
                return _BatteryPercent + "%";
            }
        }

        private Bitmap _BatteryImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Battery.0.png"));
        public Bitmap BatteryImage
        {
            get =>  _BatteryImage;
            private set => this.RaiseAndSetIfChanged(ref _BatteryImage, value);
        }
    }
}
