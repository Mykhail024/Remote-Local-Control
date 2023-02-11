using System.Reflection;
using Avalonia.Media.Imaging;
using ReactiveUI;

namespace RLCClient
{
    public class Client : ReactiveObject
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string OS { get; set; }
        private int _Ping;
        public int Ping {
            get => _Ping;
            set
            {
                if (Ping < 0) this.RaiseAndSetIfChanged(ref _Ping, 9999);
                else this.RaiseAndSetIfChanged(ref _Ping, value);

                if (_Ping <= 70) PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-4.png"));
                else if (_Ping <= 150) PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-3.png"));
                else if (_Ping <= 250) PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-2.png"));
                else if (_Ping <= 350) PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-1.png"));
                else PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-0.png"));
            }
        }

        private Bitmap _PingImage = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("RLCClient.Resources.Icons.Network.network-strength-0.png"));
        public Bitmap PingImage 
        {
            get => _PingImage;
            private set => this.RaiseAndSetIfChanged(ref _PingImage, value);
        }

        public Client(string Name, string Ip, int Port, string OS, int Ping)
        {
            this.Name = Name;
            this.Ip = Ip;
            this.Port = Port;
            this.OS = OS;
            this.Ping = Ping;
        }
    }
}
