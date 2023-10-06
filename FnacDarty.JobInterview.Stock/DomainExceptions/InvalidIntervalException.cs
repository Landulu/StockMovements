using System;

namespace FnacDarty.JobInterview.Stock.DomainExceptions
{
    public class InvalidIntervalException: Exception
    {
        public InvalidIntervalException()
        {
        }

        public InvalidIntervalException(string message)
            : base(message)
        {
        }
        
    }
}