using System;

namespace Id003Driver
{
    public static class StatusUtils
    {
        public static Status IntToStatus(int value)
        {
            try
            {
                return (Status) value;
            }
            catch (InvalidCastException)
            {
                return Status.Unknown;
            }
        }

        public static int StatusToInt(Status status)
        {
            return (int) status;
        }

        public static bool IsAcceptanceStatus(Status status)
        {
            return status >= Status.Idling && status <= Status.Inhibit;
        }

        public static bool IsPowerUpStatus(Status status)
        {
            return status >= Status.PowerUp && status <= Status.PowerUpWithBillInStacker;
        }

        public static bool IsErrorStatus(Status status)
        {
            return status >= Status.StackerFull && status <= Status.CommunicationError;
        }
    }
}
