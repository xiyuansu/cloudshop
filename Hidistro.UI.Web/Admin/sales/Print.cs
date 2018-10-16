using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Admin.sales
{
	public class Print : AdminPage
	{
		protected int pringrows;

		protected string orderIds = "";

		protected string mailNo = "";

		protected string templateName = "";

		protected string width = "";

		protected string height = "";

		protected string UpdateOrderIds = string.Empty;

		protected string ShipperName = string.Empty;

		protected string SizeShipperName = string.Empty;

		protected string CellPhone = string.Empty;

		protected string SizeCellPhone = string.Empty;

		protected string TelPhone = string.Empty;

		protected string SizeTelPhone = string.Empty;

		protected string Address = string.Empty;

		protected string SizeAddress = string.Empty;

		protected string Zipcode = string.Empty;

		protected string SizeZipcode = string.Empty;

		protected string Province = string.Empty;

		protected string SizeProvnce = string.Empty;

		protected string City = string.Empty;

		protected string SizeCity = string.Empty;

		protected string District = string.Empty;

		protected string SizeDistrict = string.Empty;

		protected string ShipToDate = string.Empty;

		protected string SizeShipToDate = string.Empty;

		protected string OrderId = string.Empty;

		protected string SizeOrderId = string.Empty;

		protected string OrderTotal = string.Empty;

		protected string SizeOrderTotal = string.Empty;

		protected string Shipitemweith = string.Empty;

		protected string SizeShipitemweith = string.Empty;

		protected string Remark = string.Empty;

		protected string SizeRemark = string.Empty;

		protected string ShipitemInfos = string.Empty;

		protected string SizeitemInfos = string.Empty;

		protected string SiteName = string.Empty;

		protected string SizeSiteName = string.Empty;

		protected string ShipTo = string.Empty;

		protected string SizeShipTo = string.Empty;

		protected string ShipTelPhone = string.Empty;

		protected string SizeShipTelPhone = string.Empty;

		protected string ShipCellPhone = string.Empty;

		protected string SizeShipCellPhone = string.Empty;

		protected string ShipZipCode = string.Empty;

		protected string SizeShipZipCode = string.Empty;

		protected string ShipAddress = string.Empty;

		protected string ShipSizeAddress = string.Empty;

		protected string ShipProvince = string.Empty;

		protected string ShipSizeProvnce = string.Empty;

		protected string ShipCity = string.Empty;

		protected string ShipSizeCity = string.Empty;

		protected string ShipDistrict = string.Empty;

		protected string ShipSizeDistrict = string.Empty;

		protected string Departure = string.Empty;

		protected string SizeDeparture = string.Empty;

		protected string Destination = string.Empty;

		protected string SizeDestination = string.Empty;

		protected HtmlGenericControl divContent;

		protected HtmlForm Form1;

		protected Button btprint;

		private void PrintPage(string pagewidth, string pageheght)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<script language='javascript'>");
			stringBuilder.Append("function clicks(){");
			if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
			{
				stringBuilder.Append(" var ShipperName=[" + this.ShipperName.Substring(0, this.ShipperName.Length - 1) + "];var SizeShipperName=[" + this.SizeShipperName.Substring(0, this.SizeShipperName.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
			{
				stringBuilder.Append(" var CellPhone=[" + this.CellPhone.Substring(0, this.CellPhone.Length - 1) + "];var SizeCellPhone=[" + this.SizeCellPhone.Substring(0, this.SizeCellPhone.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
			{
				stringBuilder.Append(" var TelPhone=[" + this.TelPhone.Substring(0, this.TelPhone.Length - 1) + "];var SizeTelPhone=[" + this.SizeTelPhone.Substring(0, this.SizeTelPhone.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
			{
				stringBuilder.Append(" var Address=[" + this.Address.Substring(0, this.Address.Length - 1) + "];var SizeAddress=[" + this.SizeAddress.Substring(0, this.SizeAddress.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
			{
				stringBuilder.Append(" var Zipcode=[" + this.Zipcode.Substring(0, this.Zipcode.Length - 1) + "];var SizeZipcode=[" + this.SizeZipcode.Substring(0, this.SizeZipcode.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
			{
				stringBuilder.Append(" var Province=[" + this.Province.Substring(0, this.Province.Length - 1) + "];var SizeProvnce=[" + this.SizeProvnce.Substring(0, this.SizeProvnce.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
			{
				stringBuilder.Append(" var City=[" + this.City.Substring(0, this.City.Length - 1) + "];var SizeCity=[" + this.SizeCity.Substring(0, this.SizeCity.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
			{
				stringBuilder.Append(" var District=[" + this.District.Substring(0, this.District.Length - 1) + "];var SizeDistrict=[" + this.SizeDistrict.Substring(0, this.SizeDistrict.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
			{
				stringBuilder.Append(" var ShipToDate=[" + this.ShipToDate.Substring(0, this.ShipToDate.Length - 1) + "];var SizeShipToDate=[" + this.SizeShipToDate.Substring(0, this.SizeShipToDate.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
			{
				stringBuilder.Append(" var OrderId=[" + this.OrderId.Substring(0, this.OrderId.Length - 1) + "];var SizeOrderId=[" + this.SizeOrderId.Substring(0, this.SizeOrderId.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
			{
				stringBuilder.Append(" var OrderTotal=[" + this.OrderTotal.Substring(0, this.OrderTotal.Length - 1) + "];var SizeOrderTotal=[" + this.SizeOrderTotal.Substring(0, this.SizeOrderTotal.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
			{
				stringBuilder.Append(" var Shipitemweith=[" + this.Shipitemweith.Substring(0, this.Shipitemweith.Length - 1) + "];var SizeShipitemweith=[" + this.SizeShipitemweith.Substring(0, this.SizeShipitemweith.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
			{
				stringBuilder.Append(" var Remark=[" + this.Remark.Substring(0, this.Remark.Length - 1) + "];var SizeRemark=[" + this.SizeRemark.Substring(0, this.SizeRemark.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
			{
				stringBuilder.Append(" var ShipitemInfos=[" + this.ShipitemInfos.Substring(0, this.ShipitemInfos.Length - 1) + "];var SizeitemInfos=[" + this.SizeitemInfos.Substring(0, this.SizeitemInfos.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
			{
				stringBuilder.Append(" var SiteName=[" + this.SiteName.Substring(0, this.SiteName.Length - 1) + "];var SizeSiteName=[" + this.SizeSiteName.Substring(0, this.SizeSiteName.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
			{
				stringBuilder.Append(" var ShipTo=[" + this.ShipTo.Substring(0, this.ShipTo.Length - 1) + "];var SizeShipTo=[" + this.SizeShipTo.Substring(0, this.SizeShipTo.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
			{
				stringBuilder.Append(" var ShipTelPhone=[" + this.ShipTelPhone.Substring(0, this.ShipTelPhone.Length - 1) + "];var SizeShipTelPhone=[" + this.SizeShipTelPhone.Substring(0, this.SizeShipTelPhone.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
			{
				stringBuilder.Append(" var ShipCellPhone=[" + this.ShipCellPhone.Substring(0, this.ShipCellPhone.Length - 1) + "];var SizeShipCellPhone=[" + this.SizeShipCellPhone.Substring(0, this.SizeShipCellPhone.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
			{
				stringBuilder.Append(" var ShipZipCode=[" + this.ShipZipCode.Substring(0, this.ShipZipCode.Length - 1) + "];var SizeShipZipCode=[" + this.SizeShipZipCode.Substring(0, this.SizeShipZipCode.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
			{
				stringBuilder.Append(" var ShipAddress=[" + this.ShipAddress.Substring(0, this.ShipAddress.Length - 1) + "];var ShipSizeAddress=[" + this.ShipSizeAddress.Substring(0, this.ShipSizeAddress.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
			{
				stringBuilder.Append(" var ShipProvince=[" + this.ShipProvince.Substring(0, this.ShipProvince.Length - 1) + "];var ShipSizeProvnce=[" + this.ShipSizeProvnce.Substring(0, this.ShipSizeProvnce.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
			{
				stringBuilder.Append(" var ShipCity=[" + this.ShipCity.Substring(0, this.ShipCity.Length - 1) + "];var ShipSizeCity=[" + this.ShipSizeCity.Substring(0, this.ShipSizeCity.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
			{
				stringBuilder.Append(" var ShipDistrict=[" + this.ShipDistrict.Substring(0, this.ShipDistrict.Length - 1) + "];var ShipSizeDistrict=[" + this.ShipSizeDistrict.Substring(0, this.ShipSizeDistrict.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
			{
				stringBuilder.Append(" var Departure=[" + this.Departure.Substring(0, this.Departure.Length - 1) + "];var SizeDeparture=[" + this.SizeDeparture.Substring(0, this.SizeDeparture.Length - 1) + "];");
			}
			if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
			{
				stringBuilder.Append(" var Destination=[" + this.Destination.Substring(0, this.Destination.Length - 1) + "];var SizeDestination=[" + this.SizeDestination.Substring(0, this.SizeDestination.Length - 1) + "];");
			}
			stringBuilder.Append(" var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));");
			stringBuilder.Append(" try{ ");
			stringBuilder.Append("  for(var i=0;i<" + this.pringrows + ";++i){ ");
			stringBuilder.Append("showdiv();");
			stringBuilder.Append(" LODOP.SET_PRINT_PAGESIZE (1," + decimal.Parse(pagewidth) * 10m + "," + decimal.Parse(pageheght) * 10m + ",\"\");");
			stringBuilder.Append(" LODOP.SET_PRINT_STYLE(\"FontSize\",12);");
			stringBuilder.Append(" LODOP.SET_PRINT_STYLE(\"Bold\",1);");
			if (!string.IsNullOrEmpty(this.SizeShipperName.Trim()))
			{
				stringBuilder.Append("LODOP.ADD_PRINT_TEXT(SizeShipperName[i].split(',')[0],SizeShipperName[i].split(',')[1],SizeShipperName[i].split(',')[2],SizeShipperName[i].split(',')[3],ShipperName[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeCellPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeCellPhone[i].split(',')[0],SizeCellPhone[i].split(',')[1],SizeCellPhone[i].split(',')[2],SizeCellPhone[i].split(',')[3],CellPhone[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeTelPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeTelPhone[i].split(',')[0],SizeTelPhone[i].split(',')[1],SizeTelPhone[i].split(',')[2],SizeTelPhone[i].split(',')[3],TelPhone[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeAddress.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeAddress[i].split(',')[0],SizeAddress[i].split(',')[1],SizeAddress[i].split(',')[2],SizeAddress[i].split(',')[3],Address[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeZipcode.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeZipcode[i].split(',')[0],Zipcode[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeProvnce.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeProvnce[i].split(',')[0],SizeProvnce[i].split(',')[1],SizeProvnce[i].split(',')[2],SizeProvnce[i].split(',')[3],Province[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeCity.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeCity[i].split(',')[0],SizeCity[i].split(',')[1],SizeCity[i].split(',')[2],SizeCity[i].split(',')[3],City[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDistrict.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDistrict[i].split(',')[0],SizeDistrict[i].split(',')[1],SizeDistrict[i].split(',')[2],SizeDistrict[i].split(',')[3],District[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipToDate.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipToDate[i].split(',')[0],SizeShipToDate[i].split(',')[1],SizeShipToDate[i].split(',')[2],SizeShipToDate[i].split(',')[3],ShipToDate[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderId.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderId[i].split(',')[0],SizeOrderId[i].split(',')[1],SizeOrderId[i].split(',')[2],SizeOrderId[i].split(',')[3],OrderId[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeOrderTotal.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeOrderTotal[i].split(',')[0],SizeOrderTotal[i].split(',')[1],SizeOrderTotal[i].split(',')[2],SizeOrderTotal[i].split(',')[3],OrderTotal[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipitemweith.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipitemweith[i].split(',')[0],SizeShipitemweith[i].split(',')[1],SizeShipitemweith[i].split(',')[2],SizeShipitemweith[i].split(',')[3]Shipitemweith[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeRemark.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeRemark[i].split(',')[0],SizeRemark[i].split(',')[1],SizeRemark[i].split(',')[2],SizeRemark[i].split(',')[3],Remark[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeitemInfos.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeitemInfos[i].split(',')[0],SizeitemInfos[i].split(',')[1],SizeitemInfos[i].split(',')[2],SizeitemInfos[i].split(',')[3],ShipitemInfos[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeSiteName.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeSiteName[i].split(',')[0],SizeSiteName[i].split(',')[1],SizeSiteName[i].split(',')[2],SizeSiteName[i].split(',')[3],SiteName[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTo.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTo[i].split(',')[0],SizeShipTo[i].split(',')[1],SizeShipTo[i].split(',')[2],SizeShipTo[i].split(',')[3],ShipTo[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipTelPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipTelPhone[i].split(',')[0],SizeShipTelPhone[i].split(',')[1],SizeShipTelPhone[i].split(',')[2],SizeShipTelPhone[i].split(',')[3],ShipTelPhone[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipCellPhone.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipCellPhone[i].split(',')[0],SizeShipCellPhone[i].split(',')[1],SizeShipCellPhone[i].split(',')[2],SizeShipCellPhone[i].split(',')[3],ShipCellPhone[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeShipZipCode.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipZipCode[i].split(',')[0],SizeShipZipCode[i].split(',')[1],SizeShipZipCode[i].split(',')[2],SizeShipZipCode[i].split(',')[3],ShipZipCode[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeAddress.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeAddress[i].split(',')[0],ShipSizeAddress[i].split(',')[1],ShipSizeAddress[i].split(',')[2],ShipSizeAddress[i].split(',')[3],ShipAddress[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeProvnce.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeProvnce[i].split(',')[0],ShipSizeProvnce[i].split(',')[1],ShipSizeProvnce[i].split(',')[2],ShipSizeProvnce[i].split(',')[3],ShipProvince[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeCity.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeCity[i].split(',')[0],ShipSizeCity[i].split(',')[1],ShipSizeCity[i].split(',')[2],ShipSizeCity[i].split(',')[3],ShipCity[i]);");
			}
			if (!string.IsNullOrEmpty(this.ShipSizeDistrict.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(ShipSizeDistrict[i].split(',')[0],ShipSizeDistrict[i].split(',')[1],ShipSizeDistrict[i].split(',')[2],ShipSizeDistrict[i].split(',')[3],ShipDistrict[i]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDeparture.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDeparture[i].split(',')[0],SizeDeparture[i].split(',')[1],SizeDeparture[i].split(',')[2],SizeDeparture[i].split(',')[3],Departure[0]);");
			}
			if (!string.IsNullOrEmpty(this.SizeDestination.Trim()))
			{
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeDestination[i].split(',')[0],SizeDestination[i].split(',')[1],SizeDestination[i].split(',')[2],SizeDestination[i].split(',')[3],Destination[i]);");
			}
			stringBuilder.Append(" LODOP.PRINT();");
			stringBuilder.Append("   }");
			stringBuilder.Append("hidediv()");
			stringBuilder.Append("  }catch(e){ alert(\"请先安装打印控件！\");return false;}");
			stringBuilder.Append("}");
			stringBuilder.Append(" setTimeout(\"clicks()\",1000);document.getElementById(\"divcloseprint\").style.display = \"\"; document.getElementById(\"divprint\").style.display = \"none\";  ");
			stringBuilder.Append("</script>");
			base.ClientScript.RegisterStartupScript(base.GetType(), "myscript", stringBuilder.ToString());
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.btprint.Click += this.ButPrint_Click;
			if (!base.IsPostBack)
			{
				this.mailNo = base.Request["mailNo"];
				int shipperId = int.Parse(base.Request["shipperId"]);
				this.orderIds = base.Request["orderIds"].Trim(',');
				string text = HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
				if (File.Exists(text))
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(text);
					XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
					this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
					string innerText = xmlNode.SelectSingleNode("pic").InnerText;
					string innerText2 = xmlNode.SelectSingleNode("size").InnerText;
					this.width = innerText2.Split(':')[0];
					this.height = innerText2.Split(':')[1];
					DataSet printData = this.GetPrintData(this.orderIds);
					int num = 0;
					foreach (DataRow row in printData.Tables[0].Rows)
					{
						this.UpdateOrderIds = this.UpdateOrderIds + row["orderid"] + ",";
						HtmlGenericControl htmlGenericControl = new HtmlGenericControl("div");
						if (!string.IsNullOrEmpty(innerText) && innerText != "noimage")
						{
							using (System.Drawing.Image image = System.Drawing.Image.FromFile(HttpContext.Current.Request.MapPath($"../../Storage/master/flex/{innerText}")))
							{
								htmlGenericControl.Attributes["style"] = $"background-image: url(../../Storage/master/flex/{innerText}); width: {image.Width}px; height: {image.Height}px;text-align: center; position: relative;";
							}
						}
						DataTable dataTable = printData.Tables[1];
						ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
						string[] array = row["shippingRegion"].ToString().Split('，');
						foreach (XmlNode item in xmlNode.SelectNodes("item"))
						{
							string innerText3 = item.SelectSingleNode("name").InnerText;
							StringBuilder stringBuilder = new StringBuilder(innerText3);
							stringBuilder.Replace("收货人-姓名", row["ShipTo"].ToString());
							stringBuilder.Replace("收货人-电话", row["TelPhone"].ToString());
							stringBuilder.Replace("收货人-手机", row["CellPhone"].ToString());
							stringBuilder.Replace("收货人-邮编", row["ZipCode"].ToString());
							stringBuilder.Replace("收货人-地址", row["Address"].ToString());
							string newValue = string.Empty;
							if (array.Length != 0)
							{
								newValue = array[0];
							}
							stringBuilder.Replace("收货人-地区1级", newValue);
							newValue = string.Empty;
							if (array.Length > 1)
							{
								newValue = array[1];
							}
							stringBuilder.Replace("收货人-地区2级", newValue);
							stringBuilder.Replace("目的地-地区", newValue);
							newValue = string.Empty;
							if (array.Length > 2)
							{
								newValue = array[2];
							}
							stringBuilder.Replace("收货人-地区3级", newValue);
							string[] array2 = new string[3]
							{
								"",
								"",
								""
							};
							if (shipper != null)
							{
								array2 = RegionHelper.GetFullRegion(shipper.RegionId, "-", true, 0).Split('-');
							}
							stringBuilder.Replace("发货人-姓名", (shipper != null) ? shipper.ShipperName : "");
							stringBuilder.Replace("发货人-手机", (shipper != null) ? shipper.CellPhone : "");
							stringBuilder.Replace("发货人-电话", (shipper != null) ? shipper.TelPhone : "");
							stringBuilder.Replace("发货人-地址", (shipper != null) ? shipper.Address : "");
							stringBuilder.Replace("发货人-邮编", (shipper != null) ? shipper.Zipcode : "");
							string newValue2 = string.Empty;
							if (array2.Length != 0)
							{
								newValue2 = array2[0];
							}
							stringBuilder.Replace("发货人-地区1级", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 1)
							{
								newValue2 = array2[1];
							}
							stringBuilder.Replace("发货人-地区2级", newValue2);
							stringBuilder.Replace("始发地-地区", newValue2);
							newValue2 = string.Empty;
							if (array2.Length > 2)
							{
								newValue2 = array2[2];
							}
							decimal num2 = default(decimal);
							decimal.TryParse(row["Weight"].ToString(), out num2);
							stringBuilder.Replace("发货人-地区3级", newValue2);
							stringBuilder.Replace("送货-上门时间", row["ShipToDate"].ToString());
							stringBuilder.Replace("订单-订单号", "订单号：" + row["OrderId"].ToString());
							stringBuilder.Replace("订单-总金额", decimal.Parse(row["OrderTotal"].ToString()).F2ToString("f2"));
							stringBuilder.Replace("订单-物品总重量", num2.F2ToString("f2"));
							stringBuilder.Replace("订单-备注", row["Remark"].ToString());
							DataRow[] array3 = dataTable.Select(" OrderId='" + row["OrderId"] + "'");
							string text2 = string.Empty;
							if (array3.Length != 0)
							{
								DataRow[] array4 = array3;
								foreach (DataRow dataRow2 in array4)
								{
									text2 = text2 + "规格" + dataRow2["SKUContent"] + " 数量" + dataRow2["ShipmentQuantity"] + "货号 :" + dataRow2["SKU"];
								}
								text2 = text2.Replace("；", "");
							}
							stringBuilder.Replace("订单-详情", text2);
							stringBuilder.Replace("订单-送货时间", "");
							stringBuilder.Replace("网店名称", HiContext.Current.SiteSettings.SiteName);
							stringBuilder.Replace("自定义内容", "");
							innerText3 = stringBuilder.ToString();
							string innerText4 = item.SelectSingleNode("font").InnerText;
							string innerText5 = item.SelectSingleNode("fontsize").InnerText;
							string innerText6 = item.SelectSingleNode("position").InnerText;
							string innerText7 = item.SelectSingleNode("align").InnerText;
							string str = innerText6.Split(':')[0];
							string str2 = innerText6.Split(':')[1];
							string str3 = innerText6.Split(':')[2];
							string str4 = innerText6.Split(':')[3];
							string innerText8 = item.SelectSingleNode("border").InnerText;
							HtmlGenericControl htmlGenericControl2 = new HtmlGenericControl("div");
							htmlGenericControl2.Visible = true;
							htmlGenericControl2.InnerText = innerText3.Split('_')[0];
							htmlGenericControl2.Style["font-family"] = innerText4;
							htmlGenericControl2.Style["font-size"] = "16px";
							htmlGenericControl2.Style["width"] = str + "px";
							htmlGenericControl2.Style["height"] = str2 + "px";
							htmlGenericControl2.Style["text-align"] = innerText7;
							htmlGenericControl2.Style["position"] = "absolute";
							htmlGenericControl2.Style["left"] = str3 + "px";
							htmlGenericControl2.Style["top"] = str4 + "px";
							htmlGenericControl2.Style["padding"] = "0";
							htmlGenericControl2.Style["margin-left"] = "0px";
							htmlGenericControl2.Style["margin-top"] = "0px";
							htmlGenericControl2.Style["font-weight"] = "bold";
							htmlGenericControl.Controls.Add(htmlGenericControl2);
						}
						this.divContent.Controls.Add(htmlGenericControl);
						num++;
						if (num < printData.Tables[0].Rows.Count)
						{
							HtmlGenericControl htmlGenericControl3 = new HtmlGenericControl("div");
							htmlGenericControl3.Attributes["class"] = "PageNext";
							this.divContent.Controls.Add(htmlGenericControl3);
						}
					}
					this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(',');
				}
			}
		}

		protected void ButPrint_Click(object sender, EventArgs e)
		{
			this.printdata();
			string[] array = this.UpdateOrderIds.Split(',');
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string str in array2)
			{
				list.Add("'" + str + "'");
			}
			OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), this.templateName, this.templateName);
			OrderHelper.SetOrderShipNumber(array, this.mailNo, this.templateName);
			OrderHelper.SetOrderPrinted(array, true);
		}

		private string ReplaceString(string str)
		{
			return str.Replace("'", "＇");
		}

		private void printdata()
		{
			this.mailNo = base.Request["mailNo"];
			int shipperId = int.Parse(base.Request["shipperId"]);
			this.orderIds = base.Request["orderIds"].Trim(',');
			string text = HttpContext.Current.Request.MapPath(string.Format("../../Storage/master/flex/{0}", base.Request["template"]));
			if (File.Exists(text))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(text);
				XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//printer");
				this.templateName = xmlNode.SelectSingleNode("kind").InnerText;
				string innerText = xmlNode.SelectSingleNode("pic").InnerText;
				string innerText2 = xmlNode.SelectSingleNode("size").InnerText;
				this.width = innerText2.Split(':')[0];
				this.height = innerText2.Split(':')[1];
				DataSet printData = this.GetPrintData(this.orderIds);
				this.pringrows = printData.Tables[0].Rows.Count;
				foreach (DataRow row in printData.Tables[0].Rows)
				{
					this.UpdateOrderIds = this.UpdateOrderIds + row["orderid"] + ",";
					DataTable dataTable = printData.Tables[1];
					ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
					string[] array = row["shippingRegion"].ToString().Split('，');
					foreach (XmlNode item in xmlNode.SelectNodes("item"))
					{
						string text2 = string.Empty;
						string innerText3 = item.SelectSingleNode("name").InnerText;
						string innerText4 = item.SelectSingleNode("position").InnerText;
						string text3 = innerText4.Split(':')[0];
						string text4 = innerText4.Split(':')[1];
						string text5 = innerText4.Split(':')[2];
						string text6 = innerText4.Split(':')[3];
						string str = text6 + "," + text5 + "," + text3 + "," + text4;
						string[] array2 = new string[3]
						{
							"",
							"",
							""
						};
						if (shipper != null)
						{
							array2 = RegionHelper.GetFullRegion(shipper.RegionId, "-", true, 0).Split('-');
						}
						string text7 = string.Empty;
						if (innerText3.Split('_')[0] == "收货人-姓名")
						{
							this.ShipTo = this.ShipTo + "'" + this.ReplaceString(row["ShipTo"].ToString()) + "',";
							if (!string.IsNullOrEmpty(row["ShipTo"].ToString().Trim()))
							{
								this.SizeShipTo = this.SizeShipTo + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-电话")
						{
							this.ShipTelPhone = this.ShipTelPhone + "'" + row["TelPhone"].ToString() + "',";
							if (!string.IsNullOrEmpty(row["TelPhone"].ToString().Trim()))
							{
								this.SizeShipTelPhone = this.SizeShipTelPhone + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-手机")
						{
							this.ShipCellPhone = this.ShipCellPhone + "'" + row["CellPhone"].ToString() + "',";
							if (!string.IsNullOrEmpty(row["CellPhone"].ToString().Trim()))
							{
								this.SizeShipCellPhone = this.SizeShipCellPhone + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-邮编")
						{
							this.ShipZipCode = this.ShipZipCode + "'" + row["ZipCode"].ToString() + "',";
							if (!string.IsNullOrEmpty(row["ZipCode"].ToString().Trim()))
							{
								this.SizeShipZipCode = this.SizeShipZipCode + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-地址")
						{
							this.ShipAddress = this.ShipAddress + "'" + this.ReplaceString(row["Address"].ToString()) + "',";
							if (!string.IsNullOrEmpty(row["Address"].ToString().Trim()))
							{
								this.ShipSizeAddress = this.ShipSizeAddress + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-地区1级")
						{
							if (array.Length != 0)
							{
								text2 = array[0];
							}
							this.ShipProvince = this.ShipProvince + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeProvnce = this.ShipSizeProvnce + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-地区2级")
						{
							text2 = string.Empty;
							if (array.Length > 1)
							{
								text2 = array[1];
							}
							this.ShipCity = this.ShipCity + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeCity = this.ShipSizeCity + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "目的地-地区")
						{
							text2 = string.Empty;
							if (array.Length > 1)
							{
								text2 = array[1];
							}
							this.Destination = this.Destination + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.SizeDestination = this.SizeDestination + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "收货人-地区3级")
						{
							text2 = string.Empty;
							if (array.Length > 2)
							{
								text2 = array[2];
							}
							this.ShipDistrict = this.ShipDistrict + "'" + text2 + "',";
							if (!string.IsNullOrEmpty(text2.Trim()))
							{
								this.ShipSizeDistrict = this.ShipSizeDistrict + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "送货-上门时间")
						{
							this.ShipToDate = this.ShipToDate + "'" + row["ShipToDate"].ToString() + "',";
							if (!string.IsNullOrEmpty(row["ShipToDate"].ToString().Trim()))
							{
								this.SizeShipToDate = this.SizeShipToDate + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "订单-订单号")
						{
							this.OrderId = this.OrderId + "'订单号：" + row["OrderId"].ToString() + "',";
							if (!string.IsNullOrEmpty(row["OrderId"].ToString().Trim()))
							{
								this.SizeOrderId = this.SizeOrderId + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "订单-总金额")
						{
							if (!string.IsNullOrEmpty(row["OrderTotal"].ToString().Trim()))
							{
								this.OrderTotal = this.OrderTotal + decimal.Parse(row["OrderTotal"].ToString()).F2ToString("f2") + "',";
							}
							if (!string.IsNullOrEmpty(row["OrderTotal"].ToString().Trim()))
							{
								this.SizeOrderTotal = this.SizeOrderTotal + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "订单-详情")
						{
							DataRow[] array3 = dataTable.Select(" OrderId='" + row["OrderId"] + "'");
							string text8 = string.Empty;
							if (array3.Length != 0)
							{
								DataRow[] array4 = array3;
								foreach (DataRow dataRow2 in array4)
								{
									text8 = text8 + "规格" + dataRow2["SKUContent"] + " 数量" + dataRow2["ShipmentQuantity"] + "货号 :" + dataRow2["SKU"];
								}
								text8 = text8.Replace(";", "");
							}
							if (!string.IsNullOrEmpty(text8.Trim()))
							{
								this.SizeitemInfos = this.SizeitemInfos + "'" + str + "',";
							}
							this.ShipitemInfos = this.ShipitemInfos + "'" + this.ReplaceString(text8) + "',";
						}
						if (innerText3.Split('_')[0] == "订单-物品总重量")
						{
							decimal num = default(decimal);
							decimal.TryParse(row["Weight"].ToString(), out num);
							this.Shipitemweith = this.Shipitemweith + "'" + num.F2ToString("f2") + "',";
							if (!string.IsNullOrEmpty(num.ToString().Trim()))
							{
								this.SizeShipitemweith = this.SizeShipitemweith + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "订单-备注")
						{
							this.Remark = this.Remark + "'" + this.ReplaceString(row["Remark"].ToString()) + "',";
							if (!string.IsNullOrEmpty(row["Remark"].ToString().Trim()))
							{
								this.SizeRemark = this.SizeRemark + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-姓名")
						{
							this.ShipperName = this.ShipperName + "'" + this.ReplaceString(shipper.ShipperName) + "',";
							if (!string.IsNullOrEmpty(shipper.ShipperName.Trim()))
							{
								this.SizeShipperName = this.SizeShipperName + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-电话")
						{
							this.TelPhone = this.TelPhone + "'" + shipper.TelPhone + "',";
							if (!string.IsNullOrEmpty(shipper.TelPhone.Trim()))
							{
								this.SizeTelPhone = this.SizeTelPhone + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-手机")
						{
							this.CellPhone = this.CellPhone + "'" + shipper.CellPhone + "',";
							if (!string.IsNullOrEmpty(shipper.CellPhone.Trim()))
							{
								this.SizeCellPhone = this.SizeCellPhone + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-邮编")
						{
							this.Zipcode = this.Zipcode + "'" + shipper.Zipcode + "',";
							if (!string.IsNullOrEmpty(shipper.Zipcode.Trim()))
							{
								this.SizeZipcode = this.SizeZipcode + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-地址")
						{
							this.Address = this.Address + "'" + this.ReplaceString(shipper.Address) + "',";
							if (!string.IsNullOrEmpty(shipper.Address.Trim()))
							{
								this.SizeAddress = this.SizeAddress + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-地区1级")
						{
							if (array2.Length != 0)
							{
								text7 = array2[0];
							}
							this.Province = this.Province + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeProvnce = this.SizeProvnce + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-地区2级")
						{
							text7 += string.Empty;
							if (array2.Length > 1)
							{
								text7 = array2[1];
							}
							this.City = this.City + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeCity = this.SizeCity + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "始发地-地区")
						{
							text7 += string.Empty;
							if (array2.Length > 1)
							{
								text7 = array2[1];
							}
							this.Departure = this.Departure + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeDeparture = this.SizeDeparture + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "发货人-地区3级")
						{
							text7 += string.Empty;
							if (array2.Length > 2)
							{
								text7 = array2[2];
							}
							this.District = this.District + "'" + text7 + "',";
							if (!string.IsNullOrEmpty(text7.Trim()))
							{
								this.SizeDistrict = this.SizeDistrict + "'" + str + "',";
							}
						}
						if (innerText3.Split('_')[0] == "网店名称")
						{
							this.SiteName = this.SiteName + "'" + this.ReplaceString(HiContext.Current.SiteSettings.SiteName) + "',";
							if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.SiteName.Trim()))
							{
								this.SizeSiteName = this.SizeSiteName + "'" + str + "',";
							}
						}
					}
				}
				this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(',');
				this.PrintPage(this.width, this.height);
			}
		}

		private DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return OrderHelper.GetOrdersAndLines(orderIds);
		}

		private decimal CalculateOrderTotal(DataRow order, DataSet ds)
		{
			decimal d = default(decimal);
			decimal d2 = default(decimal);
			decimal d3 = default(decimal);
			bool flag = false;
			decimal.TryParse(order["AdjustedFreight"].ToString(), out d);
			string value = order["CouponCode"].ToString();
			decimal.TryParse(order["CouponValue"].ToString(), out d2);
			decimal.TryParse(order["AdjustedDiscount"].ToString(), out d3);
			bool.TryParse(order["OptionPrice"].ToString(), out flag);
			DataRow[] orderGift = ds.Tables[2].Select("OrderId='" + order["orderId"] + "'");
			DataRow[] orderLine = ds.Tables[1].Select("OrderId='" + order["orderId"] + "'");
			decimal amount = this.GetAmount(orderGift, orderLine, order);
			amount += d;
			if (!string.IsNullOrEmpty(value))
			{
				amount -= d2;
			}
			return amount + d3;
		}

		public decimal GetAmount(DataRow[] orderGift, DataRow[] orderLine, DataRow order)
		{
			return this.GetGoodDiscountAmount(order, orderLine) + this.GetGiftAmount(orderGift);
		}

		public decimal GetGoodDiscountAmount(DataRow order, DataRow[] orderLine)
		{
			decimal num = default(decimal);
			decimal.TryParse(order["DiscountAmount"].ToString(), out num);
			decimal result = this.GetGoodsAmount(orderLine);
			string text = order["ReducedPromotionName"].ToString();
			if (order["ReducedPromotionAmount"] != DBNull.Value)
			{
				result = Convert.ToDecimal(order["ReducedPromotionAmount"]);
			}
			return result;
		}

		public decimal GetGoodsAmount(DataRow[] rows)
		{
			decimal num = default(decimal);
			foreach (DataRow dataRow in rows)
			{
				num += decimal.Parse(dataRow["ItemAdjustedPrice"].ToString()) * (decimal)int.Parse(dataRow["Quantity"].ToString());
			}
			return num;
		}

		public decimal GetGiftAmount(DataRow[] rows)
		{
			decimal num = default(decimal);
			foreach (DataRow dataRow in rows)
			{
				num += decimal.Parse(dataRow["CostPrice"].ToString());
			}
			return num;
		}
	}
}
