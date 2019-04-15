using System.Collections.Generic;
using CommandLine;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;
using log4net;

namespace Gravityzero.Console.Utility.Commands
{
    public class AddUserCommand : BaseCommand<UserArguments>
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(AddUserCommand));
        public AddUserCommand(): base(new string [0]){
            
        }
        public AddUserCommand(IList<string> args) : base(args)
        {
            Description = "Metoda dodaje użytkownika.";
        }

        protected override CommandResult Execute(ConsoleContext context, UserArguments arguments)
        {
            //Task<ConnectorResult<User>> result = WinApiConnector.RequestPost<User, User>(new User());
            

            

            System.Console.WriteLine($"Dodaje usera. Oto jego dane: {arguments.Name} {arguments.Surname}");
            return new CommandResult();
        }
    }

    public class UserArguments
    {
        [Option('n', "name", Required = true, HelpText = "Imię użytkownika.")]
        public string Name {get; set;}

        [Option('s', "surname", Required = true, HelpText = "Nazwisko użytkownika.")]
        public string Surname {get; set;}
    }

    public class User
    {

    }

}


            // using(IWinApiService<User, UserResult> service = new WinApiPostService<User, UserResult>()){
            //     try
            //     {
            //         service.Send();
            //     }
            //     catch(Exception ex)
            //     {
            //         if (logger.IsErrorEnabled)
            //             logger.Error(ex);
            //     }
            // }