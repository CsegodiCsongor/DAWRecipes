using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesDAW.Models
{
    public class Instruction
    {
        public Guid Id { get; set; }

        public string instruction { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
