using System;
using CommandLine;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{

    public class AddUserCommand : BaseCommand<UserArguments>
    {
        public AddUserCommand(): base(new string [0]){
            
        }
        public AddUserCommand(object[] args) : base(args)
        {
            Description = "Metoda dodaje użytkownika.";
        }

        protected override CommandResult Execute(ContextApplication context, UserArguments arguments)
        {
            Console.WriteLine($"Dodaje usera. Oto jego dane: {arguments.Name} {arguments.Surname}");
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

}