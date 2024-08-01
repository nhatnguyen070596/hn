using System;
namespace ApplicationCore.Exceptions
{
	public class AutoMapperException : Exception
	{
        public AutoMapperException()
        {
        }

        public AutoMapperException(string message)
            : base(message)
        {
        }

        public AutoMapperException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}

