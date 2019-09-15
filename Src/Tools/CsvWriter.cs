using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using Gravityzero.Console.Utility.Model;
using log4net;
using Gravityzero.Console.Utility.Logger;

namespace Gravityzero.Console.Utility.Tools
{
    public class CsvWriter<TData>
    {

        private readonly ILog log = LogManager.GetLogger(typeof(CsvWriter<TData>));
        public CsvWriter()
        {
            LoggerConfiguration.LoadConfiguration();
        }
        public bool WriteFile(string path, List<TData> data)
        {
            var tmp = new StringBuilder();
            try{
                foreach(var item in data){
                    var newLine = new StringBuilder();
                    foreach(var oneObject in item.GetType().GetProperties()){
                        var propertyValue = oneObject.GetValue(item, null);
                        if(oneObject.PropertyType.IsClass && !oneObject.PropertyType.FullName.StartsWith("System.")){
                            // var x = oneObject.PropertyType.GetProperty("Identifier").PropertyType;
                            propertyValue = (Guid?)oneObject.PropertyType.GetProperty("Identifier" ).GetValue(oneObject.GetValue(item,null),null);
                        }
                        else if(oneObject.PropertyType.IsGenericType && typeof(ICollection<>).IsAssignableFrom(oneObject.PropertyType.GetGenericTypeDefinition()) ||
                            oneObject.PropertyType.GetInterfaces().Any(x=>x.IsGenericType && x.GetGenericTypeDefinition()== typeof(ICollection<>))){
                            propertyValue = null;
                        }
                        else{
                            propertyValue = oneObject.GetValue(item, null);
                        }                    
                        newLine.Append($"{propertyValue};");
                    }
                    tmp.AppendLine(newLine.ToString());
                }
                File.WriteAllText(path,tmp.ToString());
                return true;
            }
            catch(Exception ex){
                log.Error(ex);
                return false; 
            }
        }
    }
}