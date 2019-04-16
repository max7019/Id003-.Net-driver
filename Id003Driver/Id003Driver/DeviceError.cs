namespace Id003Driver
{
    public enum DeviceError
    {
        Unknown = 0x00,
        StackMotorFailure = 0xA2,
        TransportMotorSpeedFailure = 0xA5,
        TransportMotorFailure = 0xA6,
        CashBoxNotReady = 0xAB,
        ValidatorHeadRemove = 0xAF,
        BootRomFailure = 0xB0,
        ExternalRomFailure = 0xB1,
        RomFailure = 0xB2,
        ExternalRomWritingFailure = 0xB3
    }
}
