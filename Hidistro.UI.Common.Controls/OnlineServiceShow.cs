using Hidistro.Context;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Common.Controls
{
	public class OnlineServiceShow : Literal
	{
		private const string QQServiceUrl = "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\"><img border=\"0\" SRC=\"http://wpa.qq.com/pa?p=2:[account]:51\" alt=\"点击这里联系客服[nickname]\"></a>";

		private const string WWServiceUrl = "<a target=\"_blank\" href=\"http://www.taobao.com/webww/ww.php?ver=3&touid=[account]&siteid=cntaobao&status=1&charset=utf-8\" ><img border=\"0\" src=\"http://amos.alicdn.com/realonline.aw?v=2&uid=[account]&site=cntaobao&s=1&charset=utf-8\" alt=\"点击这里联系客服[nickname]\" /></a>";

		private const string QQServiceUrl_noImage = "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\">[nickname]</a>";

		private const string WWServiceUrl_noImage = "<a target=\"_blank\" href=\"http://www.taobao.com/webww/ww.php?ver=3&touid=[account]&siteid=cntaobao&status=1&charset=utf-8\" >[nickname]</a>";

		private bool _ShowImage = true;

		private int iServiceType = 1;

		private string Accounts = "";

		private int iImageType = 1;

		private string _NickName = "";

		private string SiteName = HiContext.Current.SiteSettings.SiteName;

		public object ServiceType
		{
			get;
			set;
		}

		public object Account
		{
			get;
			set;
		}

		public object NickName
		{
			get
			{
				return this._NickName;
			}
			set
			{
				if (value != DBNull.Value && value != null)
				{
					this._NickName = value.ToString();
				}
			}
		}

		public object ImageType
		{
			get;
			set;
		}

		public bool ShowImage
		{
			get
			{
				return this._ShowImage;
			}
			set
			{
				this._ShowImage = value;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Text = "";
			if (this.Account != DBNull.Value && this.Account != null)
			{
				this.Accounts = this.Account.ToString();
			}
			if (this.ServiceType != DBNull.Value && this.ServiceType != null && this.ServiceType != "")
			{
				int.TryParse(this.ServiceType.ToString(), out this.iServiceType);
			}
			if (this.ImageType != DBNull.Value && this.ImageType != null && this.ImageType != "")
			{
				int.TryParse(this.ImageType.ToString(), out this.iImageType);
			}
			if ((this.iServiceType == 1 || this.iServiceType == 2) && this.Account != "")
			{
				if (this.iImageType < 1)
				{
					this.iImageType = 1;
				}
				string text = "";
				switch (this.iServiceType)
				{
				case 1:
					text = (this.ShowImage ? "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\"><img border=\"0\" SRC=\"http://wpa.qq.com/pa?p=2:[account]:51\" alt=\"点击这里联系客服[nickname]\"></a>" : "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\">[nickname]</a>");
					break;
				case 2:
					text = (this.ShowImage ? "<a target=\"_blank\" href=\"http://www.taobao.com/webww/ww.php?ver=3&touid=[account]&siteid=cntaobao&status=1&charset=utf-8\" ><img border=\"0\" src=\"http://amos.alicdn.com/realonline.aw?v=2&uid=[account]&site=cntaobao&s=1&charset=utf-8\" alt=\"点击这里联系客服[nickname]\" /></a>" : "<a target=\"_blank\" href=\"http://www.taobao.com/webww/ww.php?ver=3&touid=[account]&siteid=cntaobao&status=1&charset=utf-8\" >[nickname]</a>");
					break;
				default:
					text = (this.ShowImage ? "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\"><img border=\"0\" SRC=\"http://wpa.qq.com/pa?p=2:[account]:51\" alt=\"点击这里联系客服[nickname]\"></a>" : "<a target=\"blank\" href=\"http://wpa.qq.com/msgrd?v=3&uin=[account]&site=qq&menu=yes\">[nickname]</a>");
					break;
				}
				base.Text = text.Replace("[account]", this.Accounts).Replace("[imagetype]", this.iImageType.ToString()).Replace("[nickname]", this.NickName.ToString())
					.Replace("[sitename]", this.SiteName);
				base.Render(writer);
			}
		}
	}
}
