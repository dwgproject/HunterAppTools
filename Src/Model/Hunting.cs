using System;
using System.Collections.Generic;

namespace Gravityzero.Console.Utility.Model
{
    public class Hunting
    {
        public Guid Identifier { get; set; }
        public DateTime Issued { get; set; }
        public virtual User Leader { get; set; }
        public virtual Status Status {get;set;}
        public string Description { get; set; }
        public virtual ICollection<UserHunting> Users { get; set; }
        public virtual ICollection<Quarry> Quarries { get; set; }
        //wszystkie mioty na polowanie
        public virtual ICollection<PartialHunting> PartialHuntings {get; set;}
    }
}