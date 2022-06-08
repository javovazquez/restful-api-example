using RestfulApiExample.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestfulApiExample.Common
{
    public static class Errors
    {
        public static NotFoundException NotFoundException(string entityName, string data, Exception? e = null)
        {
            return new NotFoundException($"{entityName} not found - {data}", e);
        }

        public static ArgumentNullException ArgumentNullException(string argumentName, Exception? e = null)
        {
            return new ArgumentNullException(argumentName, e);
        }

        public static ArgumentOutOfRangeException ArgumentOutOfRangeException(string argumentName, string? message = null)
        {
            return new ArgumentOutOfRangeException(argumentName, message);
        }

        public static UpdateConflictException UpdateConflictException(string entityName, string data, Exception? e = null)
        {
            return new UpdateConflictException($"Conflict update of {entityName} - {data}", e);
        }

        public static ValidationException ValidationException(string msg, Exception? e = null)
        {
            return new ValidationException(msg, e);
        }

    }
}
