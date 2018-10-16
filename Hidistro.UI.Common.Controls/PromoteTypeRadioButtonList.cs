using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class PromoteTypeRadioButtonList : RadioButtonList
	{
		private bool isCanEdit = true;

		private bool isMobileExclusive = false;

		public bool IsProductPromote
		{
			get;
			set;
		}

		public bool IsWholesale
		{
			get;
			set;
		}

		public bool IsSubSite
		{
			get;
			set;
		}

		public bool IsCanEdit
		{
			get
			{
				return this.isCanEdit;
			}
			set
			{
				this.isCanEdit = value;
			}
		}

		public bool IsMobileExclusive
		{
			get
			{
				return this.isMobileExclusive;
			}
			set
			{
				this.isMobileExclusive = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.IsCanEdit)
			{
				if (this.IsProductPromote)
				{
					if (this.IsWholesale)
					{
						stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\"/><label>批发打折</label></span>", 4);
					}
					else if (this.IsMobileExclusive)
					{
						stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_MobileExclusive\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>手机专享价</label></span>", 7);
					}
					else
					{
						stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_SentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>买商品赠送礼品</label></span>", 5);
						stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_SentProduct\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>买几送几</label></span>", 6);
						if (this.IsSubSite)
						{
							stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>批发打折</label></span>", 4);
						}
					}
				}
				else if (this.IsWholesale)
				{
					stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>混合批发打折</label></span>", 13);
					stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>混合批发优惠金额</label></span>", 14);
				}
				else
				{
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>满额优惠金额</label></span>", 12);
					if (this.IsSubSite)
					{
						stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>混合批发打折</label></span>", 13);
						stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>混合批发优惠金额</label></span>", 14);
					}
					stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullAmountSentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>满额送礼品</label></span>", 15);
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountSentTimesPoint\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>满额送倍数积分</label></span>", 16);
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountSentFreight\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" /><label>满额免运费</label></span>", 17);
				}
			}
			else if (this.IsProductPromote)
			{
				if (this.IsWholesale)
				{
					stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\"/><label>批发打折</label></span>", 4);
				}
				else if (this.IsMobileExclusive)
				{
					stringBuilder.AppendFormat("<span><input class='icheck' id=\"radPromoteType_MobileExclusive\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\"/><label>手机专享价</label></span>", 7);
				}
				else
				{
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_SentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>买商品赠送礼品</label></span>", 5);
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_SentProduct\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>买几送几</label></span>", 6);
					if (this.IsSubSite)
					{
						stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_QuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>批发打折</label></span>", 4);
					}
				}
			}
			else if (this.IsWholesale)
			{
				stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>混合批发打折</label></span>", 13);
				stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>混合批发优惠金额</label></span>", 14);
			}
			else
			{
				stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>满额优惠金额</label></span>", 12);
				if (this.IsSubSite)
				{
					stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullQuantityDiscount\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>混合批发打折</label></span>", 13);
					stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullQuantityReduced\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>混合批发优惠金额</label></span>", 14);
				}
				stringBuilder.AppendFormat("<span><input class='icheck'  id=\"radPromoteType_FullAmountSentGift\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>满额送礼品</label></span>", 15);
				stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountSentTimesPoint\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>满额送倍数积分</label></span>", 16);
				stringBuilder.AppendFormat("<span><input  class='icheck' id=\"radPromoteType_FullAmountSentFreight\" type=\"radio\" name=\"radPromoteType\" value=\"{0}\" disabled=\"disabled\" /><label>满额免运费</label></span>", 17);
			}
			writer.Write(stringBuilder.ToString());
		}
	}
}
