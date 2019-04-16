using System;

namespace Id003Driver
{
    public static class ErrorUtils
    {
        public static Error IntToError(int value)
        {
            try
            {
                return (Error) value;
            }
            catch (InvalidCastException)
            {
                return Error.Unknown;
            }
        }

        public static int ErrorToInt(Error error)
        {
            return (int) error;
        }
    }
}
