using System;
using System.Collections.Generic;
using HuntingAppSupport;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Directories;
using HuntingAppSupport.Infrastructure;

namespace Src
{
    class HuntingAppSupport
    {
        static void Main(string[] args)
        {       
            ContextApplication contextApplication = new ContextApplication();
            Console.WriteLine("Program wspomagający testowanie HuntingApp");
            IDictionary<string, ICommand> commandDictionary = new Dictionary<string, ICommand>();
            commandDictionary.Add("exit", new ExitCommand());
            commandDictionary.Add("help", new HelpCommand());
            commandDictionary.Add("user", new UserDirectory());

            while (contextApplication.ShouldWork){
                
                Console.Write(contextApplication.GetPath());
                string currentInput = Console.ReadLine();

                if (!string.IsNullOrEmpty(currentInput)){
                    if (commandDictionary.ContainsKey(currentInput))
                        commandDictionary[currentInput].Execute(contextApplication);
                    else
                        Console.WriteLine("Uknown command.");
                }
            }
        }
    }
}
