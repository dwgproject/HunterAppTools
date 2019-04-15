using System;
using System.Collections.Generic;
using System.Diagnostics;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Context;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Engine{

    public class ConsoleEngine : IDisposable
    {
        private readonly ConsoleContext consoleContext;

        public ConsoleEngine()
        {
            consoleContext = new ConsoleContext();
        }
        public void Run(string[] args){
            System.Console.WriteLine();
            System.Console.WriteLine($"{GetFileInfo().ProductName} ({GetFileInfo().FileVersion})");
            System.Console.WriteLine($"Type \"help\" or \"credits\" for more information."); 
            System.Console.WriteLine();
            while (consoleContext.ShouldWork){              
                System.Console.Write(consoleContext.GetPath());
                string currentInput = System.Console.ReadLine();

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
                {
                    ICommand currentCommand = consoleContext.GetCommandIfExist(call, commandArgs);
                    CommandResult result = currentCommand.Execute(consoleContext);
                    if (!string.IsNullOrEmpty(result.Message))
                        System.Console.WriteLine(result.Message);
                    if (currentCommand is DummyCommand)
                        break;
                }
            }
        }
        private FileVersionInfo GetFileInfo(){
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            return fvi;
        }
        public void Dispose()
        {
            consoleContext?.Dispose();
        }
    }
}