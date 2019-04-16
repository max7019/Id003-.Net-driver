using System;

namespace Id003Driver
{
    public static class DeviceErrorUtils
    {
        public static DeviceError IntToDeviceError(int value)
        {
            try
            {
                return (DeviceError) value;
            }
            catch (InvalidCastException)
            {
                return DeviceError.Unknown;
            }
        }

        public static int DeviceErrorToInt(DeviceError error)
        {
            return (int) error;
        }
    }
}
