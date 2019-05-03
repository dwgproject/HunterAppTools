using System;
using System.Collections.Generic;

namespace Gravityzero.Console.Utility.Model
{
    public class PartialHunting
    {
        public Guid Identifier { get; set; }
        public int Number { get; set; }
        public virtual Hunting Hunting { get;set; }
        public virtual Status Status { get;set; }
        public virtual ICollection<PartialHuntersList> PartialHunters {get; set;}
    }
}