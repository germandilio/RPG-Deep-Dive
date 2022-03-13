using System;

namespace RPG.Stats.Exceptions
{
    public class ProgressionStatException : Exception
    {
        public ProgressionStatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}