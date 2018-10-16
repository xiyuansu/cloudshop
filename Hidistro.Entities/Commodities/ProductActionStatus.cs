namespace Hidistro.Entities.Commodities
{
	public enum ProductActionStatus
	{
		Success,
		DuplicateName,
		DuplicateSKU,
		SKUError,
		AttributeError,
		ProductTagEroor = 6,
		ProductAttrImgsError,
		UnknowError = 99
	}
}
