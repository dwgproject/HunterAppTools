using System.Collections.Generic;
using System.Text;
using HuntingAppSupport.Commands;
using HuntingAppSupport.Directories;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport{
    public class ContextApplication{
        public Stack<IDirectory> directoryStack;
        IDictionary<string, ICommand> commandDictionary = new Dictionary<string, ICommand>();

        public bool ShouldWork { get; set; } = true;

        public ContextApplication()
        {
            directoryStack = new Stack<IDirectory>();
            commandDictionary.Add("exit", new ExitCommand());
            commandDictionary.Add("help", new HelpCommand());
            commandDictionary.Add("user", new UserDirectory());
            commandDictionary.Add("up", new BackCommand());
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
        
            return string.Concat(builder.ToString(), "> ");
        }

        public ICommand GetCommandIfExist(string name){
            if (commandDictionary.ContainsKey(name))
                return commandDictionary[name];
            if (directoryStack.Count > 0 && directoryStack.Peek().Commands.ContainsKey(name))
                return directoryStack.Peek().Commands[name];
            return null;
        }
    }
}