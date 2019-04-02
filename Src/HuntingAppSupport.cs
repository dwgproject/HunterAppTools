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
            
           

            while (contextApplication.ShouldWork){
                
                Console.Write(contextApplication.GetPath());
                string currentInput = Console.ReadLine();

                if (string.IsNullOrEmpty(currentInput))
                    continue;

                string[] commandArgs = new string[0];
                Queue<string> commandsQueue = new Queue<string>();
                string [] inputCommands = currentInput.Split(' ');
                foreach(string command in inputCommands){
                    if (command.Contains("-") || command.Contains("/"))
                    {
                        int index = Array.IndexOf(inputCommands, command);
                        int paremetersLength = inputCommands.Length - index;
                        commandArgs = new string[paremetersLength];
                        Array.Copy(inputCommands, index, commandArgs, 0, paremetersLength);
                        break;
                    }else
                    {
                        commandsQueue.Enqueue(command);
                    }
                }

                if (commandsQueue.Count == 1)
                {
                    ICommand command = contextApplication.GetCommandIfExist(commandsQueue.Dequeue());
                    if (command != null)
                        command.Execute(contextApplication);
                    else 
                        Console.WriteLine("Uknown command.");
                }
                else if (commandsQueue.Count > 1)
                {
                    foreach (string call in commandsQueue)
                    {
                        ICommand command = contextApplication.GetCommandIfExist(commandsQueue.Dequeue());
                        if (command != null)
                            command.Execute(contextApplication);
                        else 
                            Console.WriteLine("Uknown command.");
                            break;
                    }
                }
            }
        }
    }
}
