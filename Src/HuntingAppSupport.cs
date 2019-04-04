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

                IList<string> commandArgs = new List<string>();
                Queue<string> commandsQueue = new Queue<string>();
                string [] inputCommands = currentInput.Split(' ');
                foreach(string command in inputCommands){
                    if (command.Contains("-") || command.Contains("/"))
                    {
                        int index = Array.IndexOf(inputCommands, command);
                        int paremetersLength = inputCommands.Length - index;
                        for (int i = index; i <= paremetersLength; i++){
                            commandArgs.Add(inputCommands[i]);
                        }
                        break;
                    }
                    else
                    {
                        commandsQueue.Enqueue(command);
                    }
                }

                foreach (string call in commandsQueue)
                {}
                    ICommand currentCommand = contextApplication.GetCommandIfExist(call, commandArgs);
                    CommandResult result = currentCommand.Execute(contextApplication);
                    if (!string.IsNullOrEmpty(result.Message))
                        Console.WriteLine(result.Message);
                    if (currentCommand is DummyCommand)
                        break;
                }
            }
        }
    }
}


                // // if (commandsQueue.Count == 1)
                // // {
                // //     currentCommand = contextApplication.GetCommandIfExist(commandsQueue.Dequeue());
                    
                // // }
                // // else if (commandsQueue.Count > 1)
                // // {
                // //     foreach (string call in commandsQueue)
                // //     {
                // //         currentCommand = contextApplication.GetCommandIfExist(commandsQueue.Dequeue());
                // //     }
                // // }

                // try{
                //     // foreach (ICommand commandQueue

                //     // CommandResult result = currentCommand.Execute(contextApplication);
                //     // Console.WriteLine(result.Message);
                // }catch(Exception ex){
                //     Console.WriteLine(ex.Message);
                // }