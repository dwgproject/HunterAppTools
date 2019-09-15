using System;
using System.Collections.Generic;
using Gravityzero.Console.Utility.Infrastructure;

namespace Gravityzero.Console.Utility.Directories
{
    public class RootDirectory : IDirectory
    {
        public RootDirectory()
        {
            Commands = new Dictionary<string, Type>();
            Directories = new Dictionary<string, IDirectory>();
            Commands.Add("configuration", typeof(ConfigurationDirectory));
            Commands.Add("user", typeof(UserDirectory));
            Commands.Add("hunting", typeof(HuntingDirectory));
            Commands.Add("resources", typeof(ResourcesDirectory));
        }
        public string Name => string.Empty;
        public IDictionary<string, Type> Commands { get; set;}
        public IDictionary<string, IDirectory> Directories { get; set;}
    }
}