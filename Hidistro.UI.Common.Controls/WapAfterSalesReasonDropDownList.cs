using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class WapAfterSalesReasonDropDownList : WebControl
	{
		private string _Reason = "收到商品破损|买错商品|商品错发/漏发|商品需要维修|发票问题|收到商品与描述不符|商品质量问题|未按约定时间发货|收到假货|七天无理由退换货";

		private string _RefundReason = "拍错/多拍/不想要|缺货|未按约定时间发货";

		private string _CssClass = "pay_list";

		private bool _IsInsert = true;

		private int _InsertIndex = -1;

		private bool _IsRefund = false;

		private string _nullToDisplay = "请选择原因";

		public override string CssClass
		{
			get
			{
				return this._CssClass;
			}
			set
			{
				this._CssClass = value;
			}
		}

		public bool IsInsert
		{
			get
			{
				return this._IsInsert;
			}
			set
			{
				this._IsInsert = value;
			}
		}

		public int InsertIndex
		{
			get
			{
				return this._InsertIndex;
			}
			set
			{
				this._InsertIndex = value;
			}
		}

		public string ReasonList
		{
			get;
			set;
		}

		public bool IsRefund
		{
			get
			{
				return this._IsRefund;
			}
			set
			{
				this._IsRefund = value;
			}
		}

		public string SelectedValue
		{
			get;
			set;
		}

		public string NullToDisplay
		{
			get
			{
				return this._nullToDisplay;
			}
			set
			{
				this._nullToDisplay = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			string format = "<li><label class=\"label-checkbox item-content\"><input type=\"radio\" name=\"chk_applyreason\" value=\"{0}\"><div class=\"item-media\"><i class=\"icon icon-form-checkbox\"></i></div><div class=\"pay_name\">{0}</li>";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("<ul class=\"" + this.CssClass + "\">");
			string text = this._Reason;
			if (this.IsRefund)
			{
				text = this._RefundReason;
			}
			string text2 = "";
			if (!string.IsNullOrEmpty(this.ReasonList))
			{
				string[] array = text.Split('|');
				if (this.IsInsert)
				{
					if (this.InsertIndex < array.Length)
					{
						for (int i = 0; i < array.Length; i++)
						{
							if (this.InsertIndex == i)
							{
								string[] array2 = this.ReasonList.Split('|');
								for (int j = 0; j < array2.Length; j++)
								{
									if (!string.IsNullOrEmpty(array2[j]))
									{
										text2 = text2 + array2[j] + "|";
									}
								}
							}
							else
							{
								text2 = text2 + array[i] + "|";
							}
						}
						text2 = text2.TrimEnd('|');
					}
					else
					{
						text2 = text + "|" + this.ReasonList;
					}
				}
				else
				{
					text2 = text + "|" + this.ReasonList;
				}
			}
			else
			{
				text2 = text;
			}
			string[] array3 = text2.Split('|');
			foreach (string arg in array3)
			{
				stringBuilder.AppendLine(string.Format(format, arg));
			}
			stringBuilder.AppendLine("</ul>");
			writer.Write(stringBuilder.ToString());
		}
	}
}
