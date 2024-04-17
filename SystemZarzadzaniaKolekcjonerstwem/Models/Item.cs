using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemZarzadzaniaKolekcjonerstwem.Models
{
    public class Item
    {
        public String Name { get; set; }

        public Item(string name)
        {
            Name = name;
        }
    }
}
