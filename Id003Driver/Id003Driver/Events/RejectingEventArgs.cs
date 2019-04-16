using System;

namespace Id003Driver.Events
{
    public class RejectingEventArgs : EventArgs
    {
        public RejectingError RejectingError { get; }

        public RejectingEventArgs(RejectingError rejectingError)
        {
            RejectingError = rejectingError;
        }
    }
}
