using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Entities.Orders;
using Hidistro.Entities.Sales;
using Hidistro.SaleSystem.Sales;
using Hidistro.UI.Common.Controls;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Supplier.sales
{
	public class BatchPrintData : SupplierAdminPage
	{
		private int UserStoreId = 0;

		protected static string orderIds = string.Empty;

		protected int pringrows;

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

		protected Dictionary<string, string> SizeCustomeList = new Dictionary<string, string>();

		protected Dictionary<string, string> CustomeList = new Dictionary<string, string>();

		protected Panel pnlTask;

		protected Literal litNumber;

		protected Panel pnlTaskEmpty;

		protected Panel pnlShipper;

		protected ShippersDropDownList ddlShoperTag;

		protected TextBox txtShipTo;

		protected RegionSelector dropRegions;

		protected TextBox txtAddress;

		protected TextBox txtTelphone;

		protected TextBox txtCellphone;

		protected Button btnUpdateAddrdss;

		protected Panel pnlEmptySender;

		protected Panel pnlTemplates;

		protected DropDownList ddlTemplates;

		protected TextBox txtStartCode;

		protected Button btnPrint;

		protected Panel pnlEmptyTemplates;

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
			if (this.SizeCustomeList.Count > 0)
			{
				int num = 0;
				foreach (string key in this.SizeCustomeList.Keys)
				{
					if (!string.IsNullOrEmpty(key))
					{
						stringBuilder.Append(" var Custome" + num + "=[" + this.CustomeList[key].Substring(0, this.CustomeList[key].Length - 1) + "];var SizeCustome" + num + "=[" + this.SizeCustomeList[key].Substring(0, this.SizeCustomeList[key].Length - 1) + "];");
					}
					num++;
				}
			}
			stringBuilder.Append(" var LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));");
			stringBuilder.Append(" try{  ");
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
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeZipcode[i].split(',')[0],SizeZipcode[i].split(',')[1],SizeZipcode[i].split(',')[2],SizeZipcode[i].split(',')[3],SizeZipcode[0]);");
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
				stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeShipitemweith[i].split(',')[0],SizeShipitemweith[i].split(',')[1],SizeShipitemweith[i].split(',')[2],SizeShipitemweith[i].split(',')[3],Shipitemweith[i]);");
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
			if (this.SizeCustomeList.Count > 0)
			{
				int num2 = 0;
				foreach (string key2 in this.SizeCustomeList.Keys)
				{
					if (!string.IsNullOrEmpty(key2.Trim()))
					{
						stringBuilder.Append(" LODOP.ADD_PRINT_TEXT(SizeCustome" + num2 + "[i].split(',')[0],SizeCustome" + num2 + "[i].split(',')[1],SizeCustome" + num2 + "[i].split(',')[2],SizeCustome" + num2 + "[i].split(',')[3],Custome" + num2 + "[i]);");
					}
					num2++;
				}
			}
			stringBuilder.Append(" LODOP.PRINT();");
			stringBuilder.Append(" }");
			stringBuilder.Append(" setTimeout(\"hidediv()\",3000);");
			stringBuilder.Append("  }catch(e){ alert(\"请先安装打印控件！\"+e.name+\"-\" + e.message);hidediv();return false;}");
			stringBuilder.Append("}");
			stringBuilder.Append(" setTimeout(\"clicks()\",1000); ");
			stringBuilder.Append("</script>");
			base.ClientScript.RegisterStartupScript(base.GetType(), "myscript", stringBuilder.ToString());
		}

		private void printdata()
		{
			this.mailNo = this.txtStartCode.Text.Trim();
			int shipperId = int.Parse(this.ddlShoperTag.SelectedValue.ToString());
			ShippersInfo shipper = SalesHelper.GetShipper(shipperId);
			if (shipper == null)
			{
				this.ShowMsg("请选择一个发货人", false);
			}
			else
			{
				string text = HttpContext.Current.Request.MapPath($"../../Storage/master/flex/{this.ddlTemplates.SelectedValue}");
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
					IList<OrderInfo> printDataList = this.GetPrintDataList(BatchPrintData.orderIds);
					this.pringrows = printDataList.Count;
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					foreach (OrderInfo item in printDataList)
					{
						this.UpdateOrderIds = this.UpdateOrderIds + item.OrderId + ",";
						string[] array = item.ShippingRegion.ToString().Split('，');
						foreach (XmlNode item2 in xmlNode.SelectNodes("item"))
						{
							string text2 = string.Empty;
							string innerText3 = item2.SelectSingleNode("name").InnerText;
							string innerText4 = item2.SelectSingleNode("position").InnerText;
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
								this.ShipTo = this.ShipTo + "'" + this.ReplaceString(item.ShipTo) + "',";
								if (!string.IsNullOrEmpty(item.ShipTo.ToNullString().Trim()))
								{
									this.SizeShipTo = this.SizeShipTo + "'" + str + "',";
								}
								else
								{
									this.SizeShipTo += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "收货人-电话")
							{
								this.ShipTelPhone = this.ShipTelPhone + "'" + item.TelPhone + "',";
								if (!string.IsNullOrEmpty(item.TelPhone.ToNullString().Trim()))
								{
									this.SizeShipTelPhone = this.SizeShipTelPhone + "'" + str + "',";
								}
								else
								{
									this.SizeShipTelPhone += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "收货人-手机")
							{
								this.ShipCellPhone = this.ShipCellPhone + "'" + item.CellPhone + "',";
								if (!string.IsNullOrEmpty(item.CellPhone.ToNullString()))
								{
									this.SizeShipCellPhone = this.SizeShipCellPhone + "'" + str + "',";
								}
								else
								{
									this.SizeShipCellPhone += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "收货人-邮编")
							{
								this.ShipZipCode = this.ShipZipCode + "'" + item.ZipCode + "',";
								if (!string.IsNullOrEmpty(item.ZipCode.ToNullString().Trim()))
								{
									this.SizeShipZipCode = this.SizeShipZipCode + "'" + str + "',";
								}
								else
								{
									this.SizeShipZipCode += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "收货人-地址")
							{
								this.ShipAddress = this.ShipAddress + "'" + this.ReplaceString(item.Address.ToNullString()) + "',";
								if (!string.IsNullOrEmpty(item.Address.ToNullString().Trim()))
								{
									this.ShipSizeAddress = this.ShipSizeAddress + "'" + str + "',";
								}
								else
								{
									this.ShipSizeAddress += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "收货人-地区1级")
							{
								if (array.Length != 0)
								{
									text2 = array[0];
								}
								this.ShipProvince = this.ShipProvince + "'" + text2 + "',";
								if (!string.IsNullOrEmpty(text2.ToNullString().Trim()))
								{
									this.ShipSizeProvnce = this.ShipSizeProvnce + "'" + str + "',";
								}
								else
								{
									this.ShipSizeProvnce += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text2.ToNullString().Trim()))
								{
									this.ShipSizeCity = this.ShipSizeCity + "'" + str + "',";
								}
								else
								{
									this.ShipSizeCity += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text2.ToNullString().Trim()))
								{
									this.SizeDestination = this.SizeDestination + "'" + str + "',";
								}
								else
								{
									this.SizeDestination += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text2.ToNullString().Trim()))
								{
									this.ShipSizeDistrict = this.ShipSizeDistrict + "'" + str + "',";
								}
								else
								{
									this.ShipSizeDistrict += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "送货-上门时间")
							{
								this.ShipToDate = this.ShipToDate + "'" + item.ShipToDate + "',";
								if (!string.IsNullOrEmpty(item.ShipToDate.ToNullString().Trim()))
								{
									this.SizeShipToDate = this.SizeShipToDate + "'" + str + "',";
								}
								else
								{
									this.SizeShipToDate += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "订单-订单号")
							{
								this.OrderId = this.OrderId + "'订单号：" + item.PayOrderId + "',";
								if (!string.IsNullOrEmpty(item.OrderId.Trim()))
								{
									this.SizeOrderId = this.SizeOrderId + "'" + str + "',";
								}
								else
								{
									this.SizeOrderId += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "订单-总金额")
							{
								decimal total = item.GetTotal(false);
								if (!string.IsNullOrEmpty(total.ToString().Trim()))
								{
									this.OrderTotal = this.OrderTotal + "'" + item.GetTotal(false).F2ToString("f2") + "',";
								}
								total = item.GetTotal(false);
								if (!string.IsNullOrEmpty(total.ToString().Trim()))
								{
									this.SizeOrderTotal = this.SizeOrderTotal + "'" + str + "',";
								}
								else
								{
									this.SizeOrderTotal += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "订单-详情")
							{
								string text8 = string.Empty;
								foreach (LineItemInfo value2 in item.LineItems.Values)
								{
									if (value2.Status == LineItemStatus.Normal || value2.Status == LineItemStatus.RefundRefused || value2.Status == LineItemStatus.ReturnsRefused || value2.Status == LineItemStatus.ReplaceRefused || value2.Status == LineItemStatus.Replaced)
									{
										text8 = text8 + "规格" + value2.SKUContent + " 数量" + value2.ShipmentQuantity + "货号 :" + value2.SKU;
									}
								}
								text8 = text8.Replace(";", "");
								if (!string.IsNullOrEmpty(text8.Trim()))
								{
									this.SizeitemInfos = this.SizeitemInfos + "'" + str + "',";
								}
								else
								{
									this.SizeitemInfos += "'0,0,0,0',";
								}
								this.ShipitemInfos = this.ShipitemInfos + "'" + this.ReplaceString(text8) + "',";
							}
							if (innerText3.Split('_')[0] == "订单-物品总重量")
							{
								decimal weight = item.Weight;
								this.Shipitemweith = this.Shipitemweith + "'" + weight.F2ToString("f2") + "',";
								if (!string.IsNullOrEmpty(weight.ToNullString().Trim()))
								{
									this.SizeShipitemweith = this.SizeShipitemweith + "'" + str + "',";
								}
								else
								{
									this.SizeShipitemweith += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "订单-备注")
							{
								this.Remark = this.Remark + "'" + this.ReplaceString(item.Remark) + "',";
								if (!string.IsNullOrEmpty(item.Remark.ToNullString().Trim()))
								{
									this.SizeRemark = this.SizeRemark + "'" + str + "',";
								}
								else
								{
									this.SizeRemark += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-姓名")
							{
								this.ShipperName = this.ShipperName + "'" + this.ReplaceString(shipper.ShipperName) + "',";
								if (!string.IsNullOrEmpty(shipper.ShipperName.ToNullString().Trim()))
								{
									this.SizeShipperName = this.SizeShipperName + "'" + str + "',";
								}
								else
								{
									this.SizeShipperName += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-电话")
							{
								this.TelPhone = this.TelPhone + "'" + shipper.TelPhone + "',";
								if (!string.IsNullOrEmpty(shipper.TelPhone.ToNullString().Trim()))
								{
									this.SizeTelPhone = this.SizeTelPhone + "'" + str + "',";
								}
								else
								{
									this.SizeTelPhone += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-手机")
							{
								this.CellPhone = this.CellPhone + "'" + shipper.CellPhone + "',";
								if (!string.IsNullOrEmpty(shipper.CellPhone.ToNullString().Trim()))
								{
									this.SizeCellPhone = this.SizeCellPhone + "'" + str + "',";
								}
								else
								{
									this.SizeCellPhone += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-邮编")
							{
								this.Zipcode = this.Zipcode + "'" + shipper.Zipcode + "',";
								if (!string.IsNullOrEmpty(shipper.Zipcode.ToNullString().Trim()))
								{
									this.SizeZipcode = this.SizeZipcode + "'" + str + "',";
								}
								else
								{
									this.SizeZipcode += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-地址")
							{
								this.Address = this.Address + "'" + this.ReplaceString(shipper.Address) + "',";
								if (!string.IsNullOrEmpty(shipper.Address.ToNullString().Trim()))
								{
									this.SizeAddress = this.SizeAddress + "'" + str + "',";
								}
								else
								{
									this.SizeAddress += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "发货人-地区1级")
							{
								if (array2.Length != 0)
								{
									text7 = array2[0];
								}
								this.Province = this.Province + "'" + text7 + "',";
								if (!string.IsNullOrEmpty(text7.ToNullString().Trim()))
								{
									this.SizeProvnce = this.SizeProvnce + "'" + str + "',";
								}
								else
								{
									this.SizeProvnce += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text7.ToNullString().Trim()))
								{
									this.SizeCity = this.SizeCity + "'" + str + "',";
								}
								else
								{
									this.SizeCity += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text7.ToNullString().Trim()))
								{
									this.SizeDeparture = this.SizeDeparture + "'" + str + "',";
								}
								else
								{
									this.SizeDeparture += "'0,0,0,0',";
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
								if (!string.IsNullOrEmpty(text7.ToNullString().Trim()))
								{
									this.SizeDistrict = this.SizeDistrict + "'" + str + "',";
								}
								else
								{
									this.SizeDistrict += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('_')[0] == "网店名称")
							{
								this.SiteName = this.SiteName + "'" + this.ReplaceString(HiContext.Current.SiteSettings.SiteName) + "',";
								if (!string.IsNullOrEmpty(HiContext.Current.SiteSettings.SiteName.Trim()))
								{
									this.SizeSiteName = this.SizeSiteName + "'" + str + "',";
								}
								else
								{
									this.SizeSiteName += "'0,0,0,0',";
								}
							}
							if (innerText3.Split('-')[0] == "自定义")
							{
								string text9 = this.ReplaceString(innerText3.Split('-')[1].Split('_')[0]);
								string definedDataItem = this.GetDefinedDataItem(this.ReplaceString(text9));
								if (this.CustomeList.ContainsKey(text9))
								{
									Dictionary<string, string> sizeCustomeList;
									string key;
									if (!string.IsNullOrEmpty(definedDataItem))
									{
										sizeCustomeList = this.SizeCustomeList;
										key = text9;
										sizeCustomeList[key] = sizeCustomeList[key] + "'" + str + "',";
									}
									else
									{
										sizeCustomeList = this.SizeCustomeList;
										key = text9;
										sizeCustomeList[key] += "'0,0,0,0',";
									}
									sizeCustomeList = this.CustomeList;
									key = text9;
									sizeCustomeList[key] = sizeCustomeList[key] + "'" + definedDataItem + "',";
								}
								else
								{
									string value = "'" + definedDataItem + "',";
									string str2 = "";
									str2 = (string.IsNullOrEmpty(definedDataItem) ? (str2 + "'0,0,0,0',") : (str2 + "'" + str + "',"));
									this.SizeCustomeList.Add(text9, str2);
									this.CustomeList.Add(text9, value);
								}
							}
						}
					}
					this.UpdateOrderIds = this.UpdateOrderIds.TrimEnd(',');
					if (string.IsNullOrEmpty(this.UpdateOrderIds))
					{
						this.ShowMsg("订单当前状态不能打印！", false);
					}
					else
					{
						this.PrintPage(this.width, this.height);
					}
				}
			}
		}

		private string GetDefinedDataItem(string name)
		{
			List<string> list = new List<string>();
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(HttpContext.Current.Request.MapPath(string.Format("/Storage/master/flex/PrintDefinedData.xml")));
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("/DataItems/Item");
			foreach (XmlNode item in xmlNodeList)
			{
				if (this.ReplaceString(item.ChildNodes[0].InnerText.Trim()).Equals(name))
				{
					return this.ReplaceString(item.ChildNodes[1].InnerText.ToNullString().Trim());
				}
			}
			return string.Empty;
		}

		private string ReplaceString(string str)
		{
			if (!string.IsNullOrEmpty(str))
			{
				return str.Replace("'", "＇").Replace("/n", "");
			}
			return string.Empty;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (HiContext.Current.Manager.RoleId == -2)
			{
				this.UserStoreId = HiContext.Current.Manager.StoreId;
			}
			if (!string.IsNullOrEmpty(base.Request["OrderIds"]))
			{
				BatchPrintData.orderIds = base.Request["OrderIds"];
				this.litNumber.Text = BatchPrintData.orderIds.Trim(',').Split(',').Length.ToString();
			}
			this.ddlShoperTag.SelectedIndexChanged += this.ddlShoperTag_SelectedIndexChanged;
			this.btnUpdateAddrdss.Click += this.btnUpdateAddrdss_Click;
			this.btnPrint.Click += this.btnbtnPrint_Click;
			if (!this.Page.IsPostBack)
			{
				this.ddlShoperTag.DataBind();
				IList<ShippersInfo> shippersBysupplierId = SalesHelper.GetShippersBysupplierId(HiContext.Current.Manager.StoreId);
				if (shippersBysupplierId != null && shippersBysupplierId.Count >= 1)
				{
					foreach (ShippersInfo item in shippersBysupplierId)
					{
						if (item.IsDefault)
						{
							this.ddlShoperTag.SelectedValue = item.ShipperId;
						}
					}
				}
				this.LoadShipper();
				this.LoadTemplates();
			}
		}

		private DataSet GetPrintData(string orderIds)
		{
			orderIds = "'" + orderIds.Replace(",", "','") + "'";
			return OrderHelper.GetOrdersAndLines(orderIds);
		}

		private IList<OrderInfo> GetPrintDataList(string orderIds)
		{
			IList<OrderInfo> list = new List<OrderInfo>();
			string[] array = orderIds.Split(',');
			foreach (string orderId in array)
			{
				OrderInfo orderInfo = OrderHelper.GetOrderInfo(orderId);
				if (orderInfo != null && orderInfo.SupplierId == this.UserStoreId && orderInfo.ItemStatus == OrderItemStatus.Nomarl && (orderInfo.OrderStatus == OrderStatus.BuyerAlreadyPaid || (orderInfo.Gateway == "hishop.plugins.payment.podrequest" && orderInfo.OrderStatus == OrderStatus.WaitBuyerPay) || orderInfo.OrderStatus == OrderStatus.SellerAlreadySent))
				{
					list.Add(orderInfo);
				}
			}
			return list;
		}

		private void btnbtnPrint_Click(object sender, EventArgs e)
		{
			this.printdata();
			string[] array = this.UpdateOrderIds.Split(',');
			List<string> list = new List<string>();
			string[] array2 = array;
			foreach (string str in array2)
			{
				list.Add("'" + str + "'");
			}
			if (!string.IsNullOrEmpty(this.UpdateOrderIds))
			{
				OrderHelper.SetOrderExpressComputerpe(string.Join(",", list.ToArray()), this.templateName, this.templateName);
				OrderHelper.SetOrderShipNumber(array, this.mailNo, this.templateName);
				OrderHelper.SetOrderPrinted(array, true);
			}
			else
			{
				this.ShowMsg("订单当前状态不能打印！", false);
			}
		}

		private void btnUpdateAddrdss_Click(object sender, EventArgs e)
		{
			if (!this.dropRegions.GetSelectedRegionId().HasValue)
			{
				this.ShowMsg("请选择发货地区！", false);
			}
			else if (this.UpdateAddress())
			{
				this.ShowMsg("修改成功", true);
			}
			else
			{
				this.ShowMsg("修改失败，请确认信息填写正确或订单还未发货", false);
			}
		}

		private bool UpdateAddress()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null && shipper.SupplierId == HiContext.Current.Manager.StoreId)
			{
				shipper.Address = this.txtAddress.Text;
				shipper.CellPhone = this.txtCellphone.Text;
				shipper.RegionId = this.dropRegions.GetSelectedRegionId().Value;
				shipper.ShipperName = this.txtShipTo.Text;
				shipper.TelPhone = this.txtTelphone.Text;
				return SalesHelper.UpdateShipper(shipper);
			}
			return false;
		}

		private void ddlShoperTag_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.LoadShipper();
		}

		private void LoadShipper()
		{
			ShippersInfo shipper = SalesHelper.GetShipper(this.ddlShoperTag.SelectedValue);
			if (shipper != null)
			{
				this.txtAddress.Text = shipper.Address;
				this.txtCellphone.Text = shipper.CellPhone;
				this.txtShipTo.Text = shipper.ShipperName;
				this.txtTelphone.Text = shipper.TelPhone;
				this.dropRegions.SetSelectedRegionId(shipper.RegionId);
				this.pnlEmptySender.Visible = false;
				this.pnlShipper.Visible = true;
			}
			else
			{
				this.pnlShipper.Visible = false;
				this.pnlEmptySender.Visible = true;
			}
		}

		private void LoadTemplates()
		{
			DataTable isUserExpressTemplates = SalesHelper.GetIsUserExpressTemplates();
			if (isUserExpressTemplates != null && isUserExpressTemplates.Rows.Count > 0)
			{
				this.ddlTemplates.Items.Add(new ListItem("-请选择-", ""));
				foreach (DataRow row in isUserExpressTemplates.Rows)
				{
					this.ddlTemplates.Items.Add(new ListItem(row["ExpressName"].ToString(), row["XmlFile"].ToString()));
				}
				this.pnlEmptyTemplates.Visible = false;
				this.pnlTemplates.Visible = true;
			}
			else
			{
				this.pnlEmptyTemplates.Visible = true;
				this.pnlTemplates.Visible = false;
			}
		}
	}
}
