using System;

namespace Doe.Ls.EntityBase.Exceptions
{
    public class LsException : Exception
    {
        public ExceptionCategory ExceptionCategory { get; set; }

        public LsException(string message) : base(message)
        {
            
        }

        public LsException(string message,Exception innerException)
            : base(message, innerException) {

        }
    
    }
}