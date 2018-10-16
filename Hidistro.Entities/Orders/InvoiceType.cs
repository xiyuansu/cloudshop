using System.ComponentModel;

namespace Hidistro.Entities.Orders
{
	public enum InvoiceType
	{
		[Description("个人普通发票")]
		Personal,
		[Description("企业普通发票")]
		Enterprise,
		[Description("个人电子发票")]
		Personal_Electronic,
		[Description("企业电子发票")]
		Enterprise_Electronic,
		[Description("增值税发票")]
		VATInvoice
	}
}
