using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Gravityzero.Console.Utility.Logger;
using log4net;

namespace Gravityzero.Console.Utility.Tools
{
    public class CsvReader<TData>: IReader<TData>
    {
        private readonly ILog log = LogManager.GetLogger(typeof(CsvReader<TData>));

        public CsvReader()
        {
            LoggerConfiguration.LoadConfiguration();
        }

        public IList<TData> LoadFile(string path)
        {
            var result = Activator.CreateInstance<List<TData>>();           
            try{
                var readFile = File.ReadAllLines(path);
                readFile = SplitData(readFile,';');
                readFile=readFile.Where(x=>!string.IsNullOrEmpty(x)).ToArray();                
                result = readFile.Select(d=>Load(d,';')).ToList();
                return result;
            }
            catch(Exception ex){
                log.Error(ex);
                System.Console.WriteLine($"Error{ex}");
                return result;
            }
        }

        private TData Load(string line, char delimiter)
        {
            string[] values = line.Split(delimiter);
            //values = values.Where(x=>!string.IsNullOrEmpty(x)).ToArray();
            var instance = Activator.CreateInstance<TData>();
            var properties = instance.GetType().GetProperties();
            int i = 0;
            if(string.IsNullOrEmpty(values[i]))
                values = values.Where(x=>x!=values[i]).ToArray();
            foreach(var property in properties){
                //if(property.PropertyType!=typeof(Guid) && !string.IsNullOrEmpty(values[i])){
                    try{
                        if(property.PropertyType.IsClass && !property.PropertyType.FullName.StartsWith("System.")){
                            Type type = Type.GetType(property.PropertyType.FullName,true);
                            var x = Activator.CreateInstance(type);
                            x.GetType().GetProperty("Identifier").SetValue(x,Guid.Parse(values[i]));
                            property.SetValue(instance,x);
                        }
                        else if(property.PropertyType.IsEnum){
                            property.SetValue(instance, Enum.Parse(property.PropertyType,values[i]));
                        }
                        else if(property.PropertyType == typeof(Guid)){
                            property.SetValue(instance,Guid.Parse(values[i]));
                        }
                        else{
                            property.SetValue(instance, Convert.ChangeType(values[i],property.PropertyType));
                        }
                    }
                    catch(Exception ex){
                        log.Error(ex);
                        System.Console.WriteLine($"{ex}");
                        return instance;
                    }
                //}
                i++;                
            }
            return instance;
        }

        private string[] SplitData(string[] tab, char delimiter)
        {
            var index =0;
            foreach (var item in tab)
            {
                var tempItem = item.Split(delimiter);
                var indexRow = 0;
                foreach (var row in tempItem)
                {                   
                    if(string.IsNullOrEmpty(row))
                        indexRow++;
                }
                if(indexRow==tempItem.Length)
                    tab[index]="";
                index++;
            }
            return tab;
        }
    }
}