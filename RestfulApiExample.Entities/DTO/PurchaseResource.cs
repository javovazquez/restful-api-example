using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities.DTO
{
    public class PurchaseResource: Utilities.LinkResource
    {
        public PurchaseResource(): base()
        {

        }

        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Cost { get; set; }
        public decimal Discount { get; set; }
    }
}
