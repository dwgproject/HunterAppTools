using System.Collections.Generic;

namespace HuntingAppSupport.Infrastructure{

    public interface IDirectory{
        string Name { get; }
        IEnumerable<ICommand> Commands { get; }
        IEnumerable<IDirectory> Directories { get; }
    }
}