using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Common.Exceptions
{
    public class UpdateConflictException : Exception
    {
        public UpdateConflictException() : base()
        {
        }

        public UpdateConflictException(string message)
            : base(message)
        {
        }

        public UpdateConflictException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
