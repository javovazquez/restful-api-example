using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities
{
    public class Purchase
    {
        public Purchase()
        {

        }

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
    }
}
