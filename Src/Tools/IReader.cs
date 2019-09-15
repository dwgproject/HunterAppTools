using System.Collections.Generic;

namespace Gravityzero.Console.Utility.Tools
{
    public interface IReader<TData>
    {
         IList<TData> LoadFile(string path);
    }
}