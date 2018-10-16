using Hidistro.Core;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Hidistro.UI.Web.wdgj_api
{
	public class test : Page
	{
		protected HtmlForm form1;

		protected TextBox uCodeTextBox;

		protected TextBox secretTextBox;

		protected TextBox actionTextBox;

		protected TextBox timeStampTextBox;

		protected TextBox signTextBox;

		protected HyperLink getProducts;

		protected HyperLink getOrders;

		protected Button saveButton;

		protected void Page_Load(object sender, EventArgs e)
		{
		}

		protected void saveButton_Click(object sender, EventArgs e)
		{
			string text = Globals.DateTimeToUnixTimestamp(DateTime.UtcNow).ToString();
			this.timeStampTextBox.Text = text;
			string text2 = this.sign(this.uCodeTextBox.Text, this.actionTextBox.Text, this.secretTextBox.Text, text);
			this.signTextBox.Text = text2;
			this.getProducts.Text = $"/wdgj_api/API.ashx?mType={this.actionTextBox.Text}&uCode={this.uCodeTextBox.Text}&TimeStamp={text}&Sign={text2}&PageSize=10&Page=1&GoodsType=Onsale";
			this.getProducts.NavigateUrl = $"/wdgj_api/API.ashx?mType={this.actionTextBox.Text}&uCode={this.uCodeTextBox.Text}&TimeStamp={text}&Sign={text2}&PageSize=10&Page=1&GoodsType=Onsale";
			this.getOrders.Text = $"/wdgj_api/API.ashx?mType={this.actionTextBox.Text}&uCode={this.uCodeTextBox.Text}&TimeStamp={text}&Sign={text2}&PageSize=10&Page=1&OrderStatus=0";
			this.getOrders.NavigateUrl = $"/wdgj_api/API.ashx?mType={this.actionTextBox.Text}&uCode={this.uCodeTextBox.Text}&TimeStamp={text}&Sign={text2}&PageSize=10&Page=1&OrderStatus=0";
		}

		private string sign(string uCode, string mType, string Secret, string TimeStamp)
		{
			string s = string.Format("{0}mType{1}TimeStamp{2}uCode{3}{0}", Secret, mType, TimeStamp, uCode);
			StringBuilder stringBuilder = new StringBuilder(32);
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(Encoding.GetEncoding("utf-8").GetBytes(s));
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString().ToUpper();
		}
	}
}
