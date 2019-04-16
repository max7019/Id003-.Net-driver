namespace Id003Driver
{
    public enum Status
    {
        None = 0x00,
        Unknown = 0xFF,
        Idling = 0x11,
        Accepting = 0x12,
        Escrow = 0x13,
        Stacking = 0x14,
        VendValid = 0x15,
        Stacked = 0x16,
        Rejecting = 0x17,
        Returning = 0x18,
        Holding = 0x19,
        Inhibit = 0x1A,
        Initialize = 0x1B,
        PowerUp = 0x40,
        PowerUpWithBillInAcceptor = 0x41,
        PowerUpWithBillInStacker = 0x42,
        StackerFull = 0x43,
        StackerOpen = 0x44,
        JamInAcceptor = 0x45,
        JamInStacker = 0x46,
        Pause = 0x47,
        Cheated = 0x48,
        Failure = 0x49,
        CommunicationError = 0x4A
    }
}
