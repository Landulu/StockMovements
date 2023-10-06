using System;

namespace FnacDarty.JobInterview.Stock.DomainExceptions
{
    public class InvalidMovementException : Exception
    {
        public InvalidMovementException()
        {
        }

        public InvalidMovementException(string message)
            : base(message)
        {
        }
    }
}