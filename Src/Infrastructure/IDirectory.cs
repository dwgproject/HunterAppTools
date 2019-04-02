using System.Collections.Generic;

namespace HuntingAppSupport.Infrastructure{

    public interface IDirectory{
        string Name { get; }
        IDictionary<string, ICommand> Commands { get; }
        IDictionary<string, IDirectory> Directories { get; }
    }
}