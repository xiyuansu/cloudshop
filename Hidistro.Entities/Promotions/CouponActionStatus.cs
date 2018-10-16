namespace Hidistro.Entities.Promotions
{
	public enum CouponActionStatus
	{
		Success,
		DuplicateName,
		InvalidClaimCode,
		Disabled,
		OutOfTimes,
		OutOfExpiryDate,
		CreateClaimCodeSuccess,
		CreateClaimCodeError,
		UnknowError = 99,
		AmountNotBy,
		NeedPointNotBy,
		DiscountValueNotBy,
		StartTimeNotBy,
		ClosingTimeNotBy,
		StartTimeAndClosingTimeNotBy,
		NotExists,
		SameClaimCode,
		EmptyClaimCode,
		CreateCouponItemFail,
		InconsistentInformationUser,
		EmptyGenerateTime,
		InadequateInventory,
		CannotReceive,
		PointNotEnough,
		Overdue,
		CanNotGet
	}
}
