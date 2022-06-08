using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities.DTO
{
    public class UpdateCustomerRequest
    {
        public UpdateCustomerRequest()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
