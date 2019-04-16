using System;

namespace Id003Driver.Events
{
    public class StatusChangedEventArgs : EventArgs
    {
        public Status PreviousStatus { get; }
        public object PreviousStatusData { get; }
        public Status CurrentStatus { get; }
        public object CurrentStatusData { get; }

        public StatusChangedEventArgs(Status previousStatus, object previousStatusData, Status currentStatus, object currentStatusData)
        {
            PreviousStatus = previousStatus;
            PreviousStatusData = previousStatusData;
            CurrentStatus = currentStatus;
            CurrentStatusData = currentStatusData;
        }
    }
}
