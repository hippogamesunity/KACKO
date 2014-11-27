using System;

namespace Assets.Scripts
{
    public class ApiKeyException : Exception
    {
        public ApiKeyException(string message) : base(message)
        {
        }
    }
}