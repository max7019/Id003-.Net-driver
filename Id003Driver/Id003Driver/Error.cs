namespace Id003Driver
{
    public enum Error
    {
        NoError = 0,
        NoHardware = -1,
        NoPort = -2,
        PortBusy = -3,
        PortClosed = -4,
        CrcError = -5,
        ProtocolError = -6,
        InvalidResponseCode = -7,
        Unknown = -8,
        InvalidBaudRate = -9,
        InvalidCountryIndex = -10,
        InvalidCurrencyIndex = -11,
        FileNotExists = -12,
        InvalidFile = -13,
        InvalidCountryCode = -14,
        InvalidCurrencyName = -15,
        InvalidCommand = 0x4B
    }
}
