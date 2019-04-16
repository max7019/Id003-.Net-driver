using System;

namespace Id003Driver
{
    public static class RejectingErrorUtils
    {
        public static RejectingError IntToRejectingError(int value)
        {
            try
            {
                return (RejectingError) value;
            }
            catch (InvalidCastException)
            {
                return RejectingError.Unknown;
            }
        }

        public static int RejectingErrorToInt(RejectingError error)
        {
            return (int) error;
        }
    }
}
