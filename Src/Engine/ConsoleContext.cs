using System;
using System.Collections.Generic;
using System.Text;
using Gravityzero.Console.Utility.Commands;
using Gravityzero.Console.Utility.Directories;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Context
{
    public class ConsoleContext : IDisposable{
        public Stack<IDirectory> directoryStack;
        private IDictionary<string, Type> generalCommands = new Dictionary<string, Type>();
        public bool ShouldWork { get; set; } = true;

        public ConsoleContext()
        {
            directoryStack = new Stack<IDirectory>();
            generalCommands.Add("exit", typeof(ExitCommand));
            generalCommands.Add("help", typeof(HelpCommand));
            generalCommands.Add("user", typeof(UserDirectory));
            generalCommands.Add("up", typeof(BackCommand));
            generalCommands.Add("configuration", typeof(ConfigurationDirectory));
        }

        public void PushDirectory(IDirectory directory){
            directoryStack.Push(directory);
        }

        public void PopDirectory(){
            directoryStack.Pop();
        }

        public string GetPath(){
            StringBuilder builder = new StringBuilder();
            var stack = directoryStack.ToArray();

            foreach(var item in stack){
                builder.Append("/");
                builder.Append(item.Name);
            }
        
            return string.Concat(builder.ToString(), ">>> ");
        }

        public ICommand GetCommandIfExist(string name, IList<string> args){
            Type type = null;
            if (generalCommands.ContainsKey(name)){
                type = generalCommands[name];
            }
            else
            {
                IDirectory current = null;
                if (directoryStack.TryPeek(out current) && current.Commands.ContainsKey(name)){
                    type = current.Commands[name];
                }
            }

            try{
                if (type != null)
                    return args.Count == 0 ? (ICommand)Activator.CreateInstance(type) : (ICommand)Activator.CreateInstance(type, args);
            }catch(Exception ex){
                System.Console.WriteLine(ex.Message);
                return new DummyCommand();
            }
            return new DummyCommand();
        }

        public void Dispose()
        {
            
        }
    }
}