namespace Id003Driver
{
    public enum RejectingError
    {
        Unknown = 0x00,
        InsertionError = 0x71,
        MagError = 0x72,
        RejectionActionByRemainingOfBills = 0x73,
        CompensationError = 0x74,
        ConveyingError = 0x75,
        DenominationAssessingError = 0x76,
        PhotoPatternError = 0x77,
        PhotoLevelError = 0x78,
        ReturnByInhibitOrInsertionDirection = 0x79,
        UnknownError = 0x7A,
        OperationError = 0x7B,
        RejectingActionByRemainingOfBillsAndSuch = 0x7C,
        LengthError = 0x7D,
        PhotoPatternError2 = 0x7E
    }
}
