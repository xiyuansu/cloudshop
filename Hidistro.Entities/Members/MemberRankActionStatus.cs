namespace Hidistro.Entities.Members
{
	public enum MemberRankActionStatus
	{
		Success,
		DuplicateName,
		DeleteDenied = 4,
		OutofNumber,
		IsDefault,
		UnknowError = 99
	}
}
