﻿using System;
using System.IO;
using Path = System.IO.Path;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;

namespace RLCClient.ViewModels
{

    public class Settings
    {
        public static readonly Settings DefaultSettings = new Settings() { ClientName = "Name", Port = 25565, ShutdownTimeOut = 1, RestartTimeOut = 1, FilterIPAdresses = new List<IPAddress>() {IPAddress.Any} };
        private static readonly string ConfigPath = $@"{Path.GetDirectoryName(Environment.ProcessPath)}\Configs";
        private static readonly string ConfigFile = @"Settings.json";
        private static readonly string ConfigFullPath = ConfigPath + @"\" + ConfigFile;

        /// <summary>Client name</summary>
        public string? ClientName { get; set; }

        /// <summary>Connection port (0-65535)</summary>
        public int Port { get; set; }

        /// <summary>Shutdown time-out in minutes</summary>
        public int ShutdownTimeOut { get; set; }

        /// <summary>Restart time-out in minutes</summary>
        public int RestartTimeOut { get; set; }

        public List<IPAddress>? FilterIPAdresses { get; set; }

        /// <summary>Load Settings from .config or create new .config</summary>
        public static Settings Load()
        {
            Settings? settings;

            if (!File.Exists(ConfigFullPath))
            {
                Console.WriteLine($"No Config \"{ConfigFile}\" in directory {ConfigPath}. Generate...");

                if (!Directory.Exists(ConfigPath))
                {
                    Directory.CreateDirectory(ConfigPath);
                }

                settings = DefaultSettings;
                using (FileStream fs = new FileStream(ConfigFullPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    JsonSerializer.Serialize<Settings>(fs, settings, new JsonSerializerOptions { WriteIndented = true });
                }

                Console.WriteLine("Successfully.");
            }
            else
            {
                using (FileStream fs = new FileStream(ConfigFullPath, FileMode.Open, FileAccess.Read))
                {
                    settings = JsonSerializer.Deserialize<Settings>(fs);
                }
            }

            return settings;
        }

        public void Save()
        {
            Settings settings = new Settings() {
                ClientName = this.ClientName,
                Port = this.Port,
                ShutdownTimeOut = this.ShutdownTimeOut,
                RestartTimeOut = this.RestartTimeOut,
                FilterIPAdresses = this.FilterIPAdresses
            };

            File.Delete(ConfigFullPath);
            using (FileStream fs = new FileStream(ConfigFullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                JsonSerializer.Serialize<Settings>(fs, settings, new JsonSerializerOptions { WriteIndented = true });
            }
        }

        public void Reset()
        {
            ClientName = DefaultSettings.ClientName;
            Port = DefaultSettings.Port;
            ShutdownTimeOut = DefaultSettings.ShutdownTimeOut;
            RestartTimeOut = DefaultSettings.RestartTimeOut;
            FilterIPAdresses = DefaultSettings.FilterIPAdresses;
            File.Delete(ConfigFullPath);
            Save();
        }

        public override bool Equals(object? obj)
        {
            Settings settings = (Settings)obj;

            if(ClientName == settings.ClientName && Port == settings.Port && ShutdownTimeOut == settings.ShutdownTimeOut && RestartTimeOut == settings.RestartTimeOut && FilterIPAdresses == settings.FilterIPAdresses)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return this.GetHashCode();
        }
    }
}
