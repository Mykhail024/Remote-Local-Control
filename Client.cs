namespace RLCClient
{
    public class Client
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string OS { get; set; }
        public int Ping { get; set; }

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
