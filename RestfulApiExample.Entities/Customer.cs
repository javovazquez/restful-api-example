using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities
{
    public class Customer
    {
        public Customer()
        {
        }

        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
