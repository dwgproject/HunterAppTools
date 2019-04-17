using System;
using System.Collections.Generic;

namespace Gravityzero.Console.Utility.Infrastructure
{
    public class DummyDirectory : IDirectory
    {
        public DummyDirectory()
        {
        }

        public string Name => string.Empty;

        public IDictionary<string, Type> Commands => new Dictionary<string, Type>();

        public IDictionary<string, IDirectory> Directories => new Dictionary<string, IDirectory>();
    }

}