namespace InsuranceAPI.Domain.Enums;

public enum PaymentType
{
    Cash = 1,
    Cheque = 2,
    BankTransfer = 3,
    OnAccount = 4,
    BankCard = 5,
    UnderCollection = 6
}

public enum PolicyStatus
{
    Draft = 0,
    Active = 1,
    Cancelled = 2,
    Expired = 3,
    Suspended = 4
}

public enum ClaimStatus
{
    Open = 0,
    UnderEstimation = 1,
    Estimated = 2,
    Settled = 3,
    Closed = 4,
    Reopened = 5
}

public enum EndorsementType
{
    New = 0,
    Endorsement = 1,
    Cancellation = 2,
    Renewal = 3
}

public enum UserPermission
{
    None = 0,
    View = 1,
    Edit = 2,
    Full = 3
}
