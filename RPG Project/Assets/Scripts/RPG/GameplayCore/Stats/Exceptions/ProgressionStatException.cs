using System;

namespace RPG.GameplayCore.Stats.Exceptions
{
    public class ProgressionStatException : Exception
    {
        public ProgressionStatException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}