using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities.DTO
{
    public class CustomerResource: Utilities.LinkResource
    {
        public CustomerResource(): base()
        {

        }

        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
