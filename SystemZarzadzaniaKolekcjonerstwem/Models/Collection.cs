using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemZarzadzaniaKolekcjonerstwem.Models
{
    public class Collection
    {
        public string Name { get; set; }
        public List<Item> Items { get; set; }

        public Collection(string name) 
        {
            Name = name;
        }
    }
}
