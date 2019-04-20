using System;
using System.Collections.Generic;

namespace Gravityzero.Console.Utility.Model
{
    public class User
    {
        public Guid Identifier { get; set; }
        public DateTime Issued { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
 
        public string Email { get; set; }

        public string Password { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<UserHunting> Huntings { get; set; }
    }
}