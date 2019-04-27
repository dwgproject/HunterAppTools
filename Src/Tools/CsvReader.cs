using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gravityzero.Console.Utility.Logger;
using log4net;

namespace Gravityzero.Console.Utility.Tools
{
    public class CsvReader<TData>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CsvReader<TData>));

        public CsvReader()
        {
            LoggerConfiguration.LoadConfiguration();
        }

        public List<TData> LoadFile(string path)
        {
            var result = Activator.CreateInstance<List<TData>>();           
            try{
                var readFile = File.ReadAllLines(path);
                result = readFile.Skip(1).Select(d=>Load(d)).ToList();
                return result;
            }
            catch(Exception ex){
                log.Error(ex);
                System.Console.WriteLine($"Error{ex}");
                return result;
            }
        }

        private TData Load(string line)
        {
            string[] values = line.Split(";");
            var instance = Activator.CreateInstance<TData>();
            var properties = instance.GetType().GetProperties();
            int i = 0;
            foreach(var property in properties){
                if(property.PropertyType!=typeof(Guid)){
                    try{
                        property.SetValue(instance, Convert.ChangeType(values[i],property.PropertyType));
                        return instance;
                    }
                    catch(Exception ex){
                        log.Error(ex);
                        System.Console.WriteLine($"{ex}");
                        return instance;
                    }
                }
                i++;                
            }
            return instance;
        }
    }
}