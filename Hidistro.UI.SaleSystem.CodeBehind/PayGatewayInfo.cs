using Hidistro.Entities.Members;
using Hidistro.Entities.Sales;

namespace Hidistro.UI.SaleSystem.CodeBehind
{
	public class PayGatewayInfo
	{
		private string wapInpourNotifyUrl;

		private string wapInpourReturnUrl;

		private InpourRequestInfo inpourRequest;

		private string orderId;

		private PaymentModeInfo paymode;

		private int gatewayTypeId;

		private string paymentName = string.Empty;

		private string gatewayTypeName = string.Empty;

		public string WapInpourNotifyUrl
		{
			get
			{
				return this.wapInpourNotifyUrl;
			}
			set
			{
				this.wapInpourNotifyUrl = value;
			}
		}

		public string WapInpourReturnUrl
		{
			get
			{
				return this.wapInpourReturnUrl;
			}
			set
			{
				this.wapInpourReturnUrl = value;
			}
		}

		public InpourRequestInfo InpourRequest
		{
			get
			{
				return this.inpourRequest;
			}
			set
			{
				this.inpourRequest = value;
			}
		}

		public string OrderId
		{
			get
			{
				return this.orderId;
			}
			set
			{
				this.orderId = value;
			}
		}

		public PaymentModeInfo Paymode
		{
			get
			{
				return this.paymode;
			}
			set
			{
				this.paymode = value;
			}
		}

		public int GatewayTypeId
		{
			get
			{
				return this.gatewayTypeId;
			}
			set
			{
				this.gatewayTypeId = value;
			}
		}

		public string PaymentName
		{
			get
			{
				return this.paymentName;
			}
			set
			{
				this.paymentName = value;
			}
		}

		public string GatewayTypeName
		{
			get
			{
				return this.gatewayTypeName;
			}
			set
			{
				this.gatewayTypeName = value;
			}
		}
	}
}
