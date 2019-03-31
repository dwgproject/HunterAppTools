using System.Collections.Generic;
using System.Text;
using HuntingAppSupport.Infrastructure;

namespace HuntingAppSupport{
    public class ContextApplication{
        public Stack<IDirectory> directoryStack;
  
        public bool ShouldWork { get; set; } = true;

        public ContextApplication()
        {
            directoryStack = new Stack<IDirectory>();
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

    }
}