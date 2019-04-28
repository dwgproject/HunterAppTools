using System.Collections.Generic;
using System.IO;

namespace Gravityzero.Console.Utility.Tools
{
    public class ConsoleSettings
    {   
        private IDictionary<string, object> settings;

        public string ServerAddress 
        {
            get 
            { 
                return settings.ContainsKey("address") ? settings["address"].ToString() : "localhost";
            }

            set 
            { 
                if (settings.ContainsKey("address"))
                {
                    settings["address"] = value;//parsowanie
                }
                else
                {
                    settings.Add("address", value);
                }
            } 
        }

        public string Port
        {
            get 
            { 
                return settings.ContainsKey("port") ? settings["port"].ToString() : "5000";
            }

            set 
            { 
                if (settings.ContainsKey("port"))
                {
                    settings["port"] = value;//parsowanie
                }
                else
                {
                    settings.Add("port", value);
                }
            } 
        }

        public ConsoleSettings()
        {
            settings = Init();
        }

        private IDictionary<string,object> Init()
        {
            string settingsFilePath = PathProvider.SettingsPath();
            if (File.Exists(settingsFilePath))
            {
                return new AppConfiguration(new Dictionary<string, object>()).
                                                LoadConfiguration(settingsFilePath);
            }
            
            settings = new Dictionary<string,object>();
            settings.Add("address", "localhost");
            settings.Add("port", "5000");    
            new AppConfiguration(settings).
                                    SaveConfiguration(settingsFilePath);
            return settings;
        }

        public void Save(){
            string settingsFilePath = PathProvider.SettingsPath();
            new AppConfiguration(settings).SaveConfiguration(settingsFilePath);
        }
    }
}