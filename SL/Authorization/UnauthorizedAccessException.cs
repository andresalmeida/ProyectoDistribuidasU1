using System;

namespace SL.Authentication
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException() : base("No autorizado.")
        {
        }

        public UnauthorizedAccessException(string message) : base(message)
        {
        }
    }
}
