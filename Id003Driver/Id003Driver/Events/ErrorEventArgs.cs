using System;

namespace Id003Driver.Events
{
    public class ErrorEventArgs : EventArgs
    {
        public Error Error { get; }

        public ErrorEventArgs(Error error)
        {
            Error = error;
        }
    }
}
