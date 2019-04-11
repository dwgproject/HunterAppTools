using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Src.Tools
{
    public class AppConfiguration
    {
        private IDictionary<string, object> configurationDictionary;

        public AppConfiguration()
        {
            configurationDictionary = new Dictionary<string, object>();
        }

        public void Push<T>(IDictionary<string,object> configuration, string key, T value)
        {
            configuration.Add(key, value);
        }

        public void Remove(IDictionary<string,object> configuration, string key)
        {
            configuration.Remove(key);
        }

        public void SaveConfiguration(string path)
        {
            var newConfiguration = JsonConvert.SerializeObject(configurationDictionary, Formatting.Indented);
            try{
                File.WriteAllText(path, newConfiguration);
            }
            catch(Exception ex){
                Console.WriteLine($"Can't write configuration - {ex}");
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
                    Console.WriteLine($"{ex}");
                    return result;
                }
            }
            else{
                Console.WriteLine($"File");
                return result;
            }
            
        }
    }
}