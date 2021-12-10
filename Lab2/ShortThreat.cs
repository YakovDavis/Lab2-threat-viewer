using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2
{
    public class ShortThreat
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ShortThreat(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
