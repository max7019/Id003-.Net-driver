using System;

namespace Id003Driver.Events
{
    public class DeviceErrorEventArgs : EventArgs
    {
        public DeviceError DeviceError { get; }

        public DeviceErrorEventArgs(DeviceError deviceError)
        {
            DeviceError = deviceError;
        }
    }
}
