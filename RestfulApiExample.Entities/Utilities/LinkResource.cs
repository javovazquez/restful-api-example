using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Entities.Utilities
{
    public class LinkResource
    {
        public List<Link> Links { get; set; }

        public LinkResource()
        {
            this.Links = new List<Link>();
        }
    }
}
