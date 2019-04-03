using System;
using System.Collections.Generic;
using System.Text;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Directories;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport{
    public class ContextApplication{
        public Stack<IDirectory> directoryStack;
        IDictionary<string, Type> generalCommands = new Dictionary<string, Type>();

        public bool ShouldWork { get; set; } = true;

        public ContextApplication()
        {
            directoryStack = new Stack<IDirectory>();
            generalCommands.Add("exit", typeof(ExitCommand));
            generalCommands.Add("help", typeof(HelpCommand));
            generalCommands.Add("user", typeof(UserDirectory));
            generalCommands.Add("up", typeof(BackCommand));
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
        
            return string.Concat(builder.ToString(), "()-> ");
        }

        public ICommand GetCommandIfExist(string name, string [] args){
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
                    return (ICommand)Activator.CreateInstance(type, args);
            }catch(Exception ex){
                Console.WriteLine(ex.Message);
                return new DummyCommand();
            }
            return new DummyCommand();
        }
    }
}