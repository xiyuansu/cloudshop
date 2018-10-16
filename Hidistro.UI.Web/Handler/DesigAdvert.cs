using Hidistro.Context;
using Hidistro.Core;
using Hidistro.UI.Common.Controls;
using Hidistro.UI.SaleSystem.Tags;
using Hishop.Components.Validation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hidistro.UI.Web.Handler
{
	public class DesigAdvert : AdminPage, IHttpHandler
	{
		private string message = "";

		private string modeId = "";

		private string elementId = "";

		private string resultformat = "{{\"success\":{0},\"Result\":{1}}}";

		public new bool IsReusable
		{
			get
			{
				return false;
			}
		}

		public new void ProcessRequest(HttpContext context)
		{
			try
			{
				this.modeId = context.Request.Form["ModelId"];
				string text = this.modeId;
				switch (text)
				{
				default:
					if (text == "editeadvertcustom")
					{
						string text4 = context.Request.Form["Param"];
						if (text4 != "")
						{
							JObject advertcustomobject = (JObject)JsonConvert.DeserializeObject(text4);
							if (this.CheckAdvertCustom(advertcustomobject) && this.UpdateAdvertCustom(advertcustomobject))
							{
								Common_CustomAd common_CustomAd = new Common_CustomAd();
								common_CustomAd.AdId = Convert.ToInt32(this.elementId);
								var value3 = new
								{
									AdCustom = common_CustomAd.RendHtml()
								};
								this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value3));
							}
						}
					}
					break;
				case "editeadvertslide":
				{
					string text5 = context.Request.Form["Param"];
					if (text5 != "")
					{
						JObject avdvertobject = (JObject)JsonConvert.DeserializeObject(text5);
						if (this.CheckAdvertSlide(avdvertobject) && this.UpdateAdvertSlide(avdvertobject))
						{
							Common_SlideAd common_SlideAd = new Common_SlideAd();
							common_SlideAd.AdId = Convert.ToInt32(this.elementId);
							var value4 = new
							{
								AdSlide = common_SlideAd.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value4));
						}
					}
					break;
				}
				case "editeadvertimage":
				{
					string text3 = context.Request.Form["Param"];
					if (text3 != "")
					{
						JObject advertimageobject = (JObject)JsonConvert.DeserializeObject(text3);
						if (this.CheckAdvertImage(advertimageobject) && this.UpdateAdvertImage(advertimageobject))
						{
							Common_ImageAd common_ImageAd = new Common_ImageAd();
							common_ImageAd.AdId = Convert.ToInt32(this.elementId);
							var value2 = new
							{
								AdImage = common_ImageAd.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value2));
						}
					}
					break;
				}
				case "editelogo":
				{
					string text2 = context.Request.Form["Param"];
					if (text2 != "")
					{
						JObject jObject = (JObject)JsonConvert.DeserializeObject(text2);
						if (this.CheckLogo(jObject) && this.UpdateLogo(jObject))
						{
							Common_Logo common_Logo = new Common_Logo();
							var value = new
							{
								LogoUrl = common_Logo.RendHtml()
							};
							this.message = string.Format(this.resultformat, "true", JsonConvert.SerializeObject(value));
						}
					}
					break;
				}
				}
			}
			catch (Exception ex)
			{
				this.message = "{\"success\":false,\"Result\":\"未知错误:" + ex.Message + "\"}";
			}
			context.Response.ContentType = "text/json";
			context.Response.Write(this.message);
		}

		private bool CheckAdvertSlide(JObject avdvertobject)
		{
			if (string.IsNullOrEmpty(avdvertobject["Image1"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image2"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image3"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image4"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image5"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image6"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image7"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image8"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image9"].ToString()) && string.IsNullOrEmpty(avdvertobject["Image10"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
				return false;
			}
			int num = 0;
			for (int i = 1; i <= 10; i++)
			{
				if (!string.IsNullOrEmpty(avdvertobject["Image" + i].ToString()))
				{
					num++;
				}
			}
			if (num < 3)
			{
				this.message = string.Format(this.resultformat, "false", "\"请至少上传3张广告图片！\"");
				return false;
			}
			if (string.IsNullOrEmpty(avdvertobject["Id"].ToString()) || avdvertobject["Id"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			return true;
		}

		private bool UpdateAdvertSlide(JObject avdvertobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改轮播广告失败\"");
			this.elementId = avdvertobject["Id"].ToString().Split('_')[1];
			avdvertobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(avdvertobject);
			return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "slide", xmlNodeString);
		}

		private bool CheckAdvertImage(JObject advertimageobject)
		{
			if (string.IsNullOrEmpty(advertimageobject["Image"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择广告图片！\"");
				return false;
			}
			if (string.IsNullOrEmpty(advertimageobject["Id"].ToString()) || advertimageobject["Id"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			return true;
		}

		private bool UpdateAdvertImage(JObject advertimageobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改单张广告图片失败\"");
			this.elementId = advertimageobject["Id"].ToString().Split('_')[1];
			advertimageobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertimageobject);
			return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "image", xmlNodeString);
		}

		private bool CheckAdvertCustom(JObject advertcustomobject)
		{
			if (string.IsNullOrEmpty(advertcustomobject["Id"].ToString()) || advertcustomobject["Id"].ToString().Split('_').Length != 2)
			{
				this.message = string.Format(this.resultformat, "false", "\"请选择要编辑的对象\"");
				return false;
			}
			return true;
		}

		private bool UpdateAdvertCustom(JObject advertcustomobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"自定义编辑失败\"");
			this.elementId = advertcustomobject["Id"].ToString().Split('_')[1];
			advertcustomobject["Id"] = this.elementId;
			Dictionary<string, string> xmlNodeString = this.GetXmlNodeString(advertcustomobject);
			if (xmlNodeString.Keys.Contains("Html"))
			{
				xmlNodeString["Html"] = Globals.HtmlDecode(xmlNodeString["Html"]);
			}
			return TagsHelper.UpdateAdNode(Convert.ToInt16(this.elementId), "custom", xmlNodeString);
		}

		private bool CheckLogo(JObject logoobject)
		{
			if (string.IsNullOrEmpty(logoobject["LogoUrl"].ToString()))
			{
				this.message = string.Format(this.resultformat, "false", "\"请上传Logo图片！\"");
				return false;
			}
			return true;
		}

		private bool UpdateLogo(JObject advertimageobject)
		{
			this.message = string.Format(this.resultformat, "false", "\"修改Logo图片失败\"");
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			masterSettings.LogoUrl = advertimageobject["LogoUrl"].ToString();
			string text = "";
			SettingsManager.Save(masterSettings);
			return true;
		}

		private bool ValidationSettings(SiteSettings setting, ref string errors)
		{
			ValidationResults validationResults = Validation.Validate(setting, "ValMasterSettings");
			if (!validationResults.IsValid)
			{
				foreach (ValidationResult item in (IEnumerable<ValidationResult>)validationResults)
				{
					errors += Formatter.FormatErrorMessage(item.Message);
				}
			}
			return validationResults.IsValid;
		}

		public Dictionary<string, string> GetXmlNodeString(JObject scriptobject)
		{
			Dictionary<string, string> dictionary = scriptobject.ToObject<Dictionary<string, string>>();
			Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				dictionary2.Add(item.Key, Globals.HtmlEncode(item.Value.ToString()));
			}
			return dictionary2;
		}
	}
}
