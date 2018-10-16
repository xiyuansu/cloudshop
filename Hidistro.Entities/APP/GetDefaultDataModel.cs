using System.Collections.Generic;

namespace Hidistro.Entities.APP
{
	public class GetDefaultDataModel
	{
		public List<TagProducts> tagProducts
		{
			get;
			set;
		}

		public bool IsOpenMeiQiaService
		{
			get;
			set;
		}

		public bool IsSuportPhoneRegister
		{
			get;
			set;
		}

		public bool IsSuportEmailRegister
		{
			get;
			set;
		}

		public bool IsValidEmail
		{
			get;
			set;
		}

		public string RegisterExtendInfo
		{
			get;
			set;
		}

		public string HomeTopicVersion
		{
			get;
			set;
		}

		public string HomeTopicPath
		{
			get;
			set;
		}

		public bool IsOpenSupplier
		{
			get;
			set;
		}

		public int AppCategoryTemplateStatus
		{
			get;
			set;
		}

		public bool IsOpenAppPromoteCoupons
		{
			get;
			set;
		}

		public decimal AppPromoteCouponsAmount
		{
			get;
			set;
		}

		public bool IsOpenMultStore
		{
			get;
			set;
		}

		public bool IsOpenReferral
		{
			get;
			set;
		}
	}
}
