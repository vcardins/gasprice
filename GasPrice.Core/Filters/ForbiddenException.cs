using System;
using GasPrice.Core.Constants;

namespace GasPrice.Core.Filters
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException()
            : this(UserAccountConstants.InformationMessages.AccessNotAllowed)
        {
        }

        public ForbiddenException(string message)
            : base(message)
        {

        }

        public ForbiddenException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}