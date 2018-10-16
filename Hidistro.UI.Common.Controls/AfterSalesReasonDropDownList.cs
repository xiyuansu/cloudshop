using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class AfterSalesReasonDropDownList : DropDownList
	{
		private bool _allowNull = true;

		private string _nullToDisplay = "请选择原因";

		private string _RefundReason = "拍错/多拍/不想要|缺货|未按约定时间发货";

		private string _Reason = "收到商品破损|买错商品|商品错发/漏发|商品需要维修|发票问题|收到商品与描述不符|商品质量问题|未按约定时间发货|收到假货|七天无理由退换货";

		private bool _IsInsert = true;

		private int _InsertIndex = -1;

		private bool _IsRefund = false;

		public bool AllowNull
		{
			get
			{
				return this._allowNull;
			}
			set
			{
				this._allowNull = value;
			}
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

		public string ReasonList
		{
			get;
			set;
		}

		public override void DataBind()
		{
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
			if (this.AllowNull)
			{
				base.Items.Add(new ListItem(this.NullToDisplay, string.Empty));
			}
			string[] array3 = text2.Split('|');
			foreach (string text3 in array3)
			{
				base.Items.Add(new ListItem(text3, text3));
			}
			base.DataBind();
		}

		public AfterSalesReasonDropDownList()
		{
			this.Items.Clear();
		}
	}
}
