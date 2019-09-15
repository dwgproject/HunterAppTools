using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Model;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class CreateCsvCommand<TData> : BaseCommand<CsvArguments>
    {
        public CreateCsvCommand(IList<string> args) : base(args)
        {
        }

        protected override CommandResult Execute(ConsoleContext context, CsvArguments arguments)
        {
            var list = new List<TData>();
            
            Task<ConnectorResult<Response<IEnumerable<TData>>>> request = WinApiConnector.RequestGet<Response<IEnumerable<TData>>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/{arguments.Source}/");
            ConnectorResult<Response<IEnumerable<TData>>> response = request.Result;
            if(!response.IsSuccess)
                return new CommandResult(response.Message, false);
            if(!response.Response.IsSuccess)
                return new CommandResult(response.Response.Code, false);
            if(!response.Response.Payload.Any())
                return new CommandResult("The payload od response is null or empty");
            
            foreach(var item in response.Response.Payload)
                list.Add(item);
            
            bool isSuccess = false;
            CsvWriter<TData> writer = new CsvWriter<TData>();
            if(!string.IsNullOrEmpty(arguments.Amount)){
                int count=0;
                bool isParsed = int.TryParse(arguments.Amount, out count);

                Random rand = new Random();
                int index = rand.Next(0,list.Count);
                list = list.GetRange(index,count);
                isSuccess = writer.WriteFile(arguments.Destination,list);
                return new CommandResult("OK",true);
            }           
            isSuccess = writer.WriteFile(arguments.Destination,list);
            if(!isSuccess)
                return new CommandResult("Write file error", false);
            return new CommandResult("OK",true);
        }
    }

    public class CsvArguments
    {
        [Option('s',"sourcePath", HelpText="path to url")]
        public string Source { get; set; }

        [Option('d',"destinationPath", HelpText="path to localization of new csv file")]
        public string Destination { get; set; }

        [Option('a',"amount", Required=false, HelpText="How many rows?")]
        public string Amount { get; set; }
    }
}