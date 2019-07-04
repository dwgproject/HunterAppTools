using System.Collections.Generic;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using Gravityzero.Console.Utility.Tools;

namespace Gravityzero.Console.Utility.Commands
{
    public class DeleteQuarryCommand : BaseCommand<QuarryArguments>
    {
        //Do automatyzacji testów można będzie wykorzystać opconalny parametr -i<IDENTIFIER> klasy QuarryArguments
        public DeleteQuarryCommand(IList<string> args) : base(args)
        {
        }       
        protected override CommandResult Execute(ConsoleContext context, QuarryArguments arguments)
        {
            if(!string.IsNullOrEmpty(arguments.Identifier)){
                var result = WinApiConnector.RequestDelete<Response<string>>($"{context.ConsoleSettings.ServerAddress}:{context.ConsoleSettings.Port}/Api/"+$"/{arguments.Identifier}");
                return new CommandResult(result.Result.IsSuccess ? "OK" : result.Result.Message);
            }
            // TODO
            // Usuwanie po identyfikatorze polowania
            return new CommandResult();
        }
    }
}