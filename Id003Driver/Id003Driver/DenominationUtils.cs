using System;

namespace Id003Driver
{
    public static class DenominationUtils
    {
        private const int FirstDenominationIndex = 0x61;
        private const int LastDenominationIndex = 0x68;

        public static int GetDenominationPureIndex(int deviceIndex)
        {
            if (deviceIndex < FirstDenominationIndex || deviceIndex > LastDenominationIndex)
                throw new ArgumentOutOfRangeException(nameof(deviceIndex));

            return deviceIndex - FirstDenominationIndex;
        }
    }
}
