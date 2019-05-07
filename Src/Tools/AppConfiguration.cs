using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Gravityzero.Console.Utility.Tools
{
    public class AppConfiguration
    {
        private IDictionary<string, object> configuration;

        public AppConfiguration(IDictionary<string, object> configuration)
        {
            this.configuration = configuration;
        }

        public void Push<T>(string key, T value)
        {
            if(!configuration.ContainsKey(key)){
                configuration.Add(key, value);
            }
            else
            {
                System.Console.WriteLine($"Key -{key}- already exist");
            }

        }

        public void Remove( string key)
        {
            if(configuration.ContainsKey(key)){
                configuration.Remove(key);
            }
            else{
                System.Console.WriteLine($"Key -{key}- doesn't exist");    
            }
        }

        public void SaveConfiguration(string path)
        {
            var newConfiguration = JsonConvert.SerializeObject(configuration, Formatting.Indented);
            try{
                File.WriteAllText(path, newConfiguration);
            }
            catch(Exception ex){
                System.Console.WriteLine($"Can't write configuration - {ex}");
            }
        }

        public IDictionary<string, object> LoadConfiguration(string path)
        {
            IDictionary<string,object> result = new Dictionary<string, object>();
            if(File.Exists(path)){
                try{
                    var currentConfiguration = File.ReadAllText(path);
                    result = JsonConvert.DeserializeObject<IDictionary<string,object>>(currentConfiguration);
                    return result;
                }
                catch(Exception ex){
                    System.Console.WriteLine($"{ex}");
                    return result;
                }
            }
            else{
                System.Console.WriteLine($"File doesn't exist");
                return result;
            }           
        }

        public void UpdateConfiguration(IDictionary<string, object> dictionary)
        {
            configuration = LoadConfiguration(PathProvider.SettingsPath());
            configuration["address"] = dictionary["address"];
            configuration["port"] = dictionary["port"];
            SaveConfiguration(PathProvider.SettingsPath());
        }
    }
}