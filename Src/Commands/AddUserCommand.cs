using System;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport.Commands{

    public class AddUserCommand : ICommand
    {
        public string Description { get; set; }

        public CommandResult Execute(ContextApplication context)
        {
            Console.WriteLine("Dodaje usera");
            return new CommandResult();
        }
    }

}