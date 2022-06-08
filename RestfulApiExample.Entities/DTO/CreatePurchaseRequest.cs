using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities.DTO
{
    public class CreatePurchaseRequest
    {
        public CreatePurchaseRequest()
        {

        }
        public Guid CustomerId { get; set; }
    }
}
