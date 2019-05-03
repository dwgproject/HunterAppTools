using System;

namespace Gravityzero.Console.Utility.Model
{
    public class Quarry
    {
        public Guid Identifier { get; set; }
        public virtual Animal Animal { get; set; }
        public int Amount { get; set; }
    }
}