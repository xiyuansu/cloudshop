using Hidistro.Context;
using Hidistro.Core;
using Hidistro.Core.Configuration;
using Hidistro.Core.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace Hidistro.UI.Web.Installer
{
	public class Install : Page
	{
		private string action;

		private string dbServer;

		private string dbName;

		private string dbUsername;

		private string dbPassword;

		private string username;

		private string email;

		private string password;

		private string password2;

		private bool isAddDemo;

		private string siteName;

		private string siteDescription;

		private bool testSuccessed;

		private IList<string> errorMsgs = null;

		protected HtmlForm form1;

		protected Label lblErrMessage;

		protected Label Label1;

		protected TextBox txtDbServer;

		protected TextBox txtDbName;

		protected TextBox txtDbUsername;

		protected TextBox txtDbPassword;

		protected TextBox txtUsername;

		protected TextBox txtEmail;

		protected TextBox txtPassword;

		protected TextBox txtPassword2;

		protected TextBox txtSiteName;

		protected TextBox txtSiteDescription;

		protected CheckBox chkIsAddDemo;

		protected Button btnInstall;

		private void LoadParameters()
		{
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				this.action = base.Request["action"];
				this.dbServer = base.Request["DBServer"];
				this.dbName = base.Request["DBName"];
				this.dbUsername = base.Request["DBUsername"];
				this.dbPassword = base.Request["DBPassword"];
				this.username = base.Request["Username"];
				this.email = base.Request["Email"];
				this.password = base.Request["Password"];
				this.password2 = base.Request["Password2"];
				this.isAddDemo = (!string.IsNullOrEmpty(base.Request["IsAddDemo"]) && base.Request["IsAddDemo"] == "true");
				this.testSuccessed = (!string.IsNullOrEmpty(base.Request["TestSuccessed"]) && base.Request["TestSuccessed"] == "true");
				this.siteName = ((string.IsNullOrEmpty(base.Request["SiteName"]) || base.Request["SiteName"].Trim().Length == 0) ? "Hishop" : base.Request["SiteName"]);
				this.siteDescription = ((string.IsNullOrEmpty(base.Request["SiteDescription"]) || base.Request["SiteDescription"].Trim().Length == 0) ? "最安全，最专业的网上商店系统" : base.Request["SiteDescription"]);
			}
			else
			{
				this.dbServer = this.txtDbServer.Text;
				this.dbName = this.txtDbName.Text;
				this.dbUsername = this.txtDbUsername.Text;
				this.dbPassword = this.txtDbPassword.Text;
				this.username = this.txtUsername.Text;
				this.email = this.txtEmail.Text;
				this.password = this.txtPassword.Text;
				this.password2 = this.txtPassword2.Text;
				this.isAddDemo = this.chkIsAddDemo.Checked;
				this.siteName = ((this.txtSiteName.Text.Trim().Length == 0) ? "Hishop" : this.txtSiteName.Text);
				this.siteDescription = ((this.txtSiteDescription.Text.Trim().Length == 0) ? "最安全，最专业的网上商店系统" : this.txtSiteDescription.Text);
			}
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			this.LoadParameters();
			this.btnInstall.Click += this.btnInstall_Click;
			if (!string.IsNullOrEmpty(base.Request["isCallback"]) && base.Request["isCallback"] == "true")
			{
				string str = "无效的操作类型：" + this.action;
				bool flag = false;
				if (this.action == "Test")
				{
					flag = this.ExecuteTest();
				}
				base.Response.Clear();
				base.Response.ContentType = "application/json";
				if (flag)
				{
					base.Response.Write("{\"Status\":\"OK\"}");
				}
				else
				{
					string text = "";
					if (this.errorMsgs != null && this.errorMsgs.Count > 0)
					{
						foreach (string errorMsg in this.errorMsgs)
						{
							text = text + "{\"Text\":\"" + errorMsg + "\"},";
						}
						text = text.Substring(0, text.Length - 1);
						this.errorMsgs.Clear();
					}
					else
					{
						text = "{\"Text\":\"" + str + "\"}";
					}
					base.Response.Write("{\"Status\":\"Fail\",\"ErrorMsgs\":[" + text + "]}");
				}
				base.Response.End();
			}
		}

		private void btnInstall_Click(object sender, EventArgs e)
		{
			string empty = string.Empty;
			int num = default(int);
			if (!this.ValidateUser(out empty))
			{
				this.ShowMsg(empty, false);
			}
			else if (!this.testSuccessed && !this.ExecuteTest())
			{
				this.ShowMsg("数据库链接信息有误", false);
			}
			else if (!this.CreateDataSchema(out empty))
			{
				this.ShowMsg(empty, false);
			}
			else if (!this.AddBuiltInRoles(out empty))
			{
				this.ShowMsg(empty, false);
			}
			else if (!this.CreateAdministrator(out num, out empty))
			{
				this.ShowMsg(empty, false);
			}
			else if (!this.AddInitData(out empty))
			{
				this.ShowMsg(empty, false);
			}
			else if (!this.isAddDemo || this.AddDemoData(out empty))
			{
				if (!this.SaveSiteSettings(out empty))
				{
					this.ShowMsg(empty, false);
				}
				else if (!this.SaveConfig(out empty))
				{
					this.ShowMsg(empty, false);
				}
				else
				{
					this.Context.Response.Redirect("/", true);
				}
			}
		}

		private void ShowMsg(string errorMsg, bool seccess)
		{
			this.lblErrMessage.Text = errorMsg;
		}

		private bool CreateDataSchema(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/Schema.sql");
			if (!File.Exists(text))
			{
				errorMsg = "没有找到数据库架构文件-Schema.sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool AddBuiltInRoles(out string errorMsg)
		{
			DbConnection dbConnection = null;
			DbTransaction dbTransaction = null;
			try
			{
				using (dbConnection = new SqlConnection(this.GetConnectionString()))
				{
					dbConnection.Open();
					DbCommand dbCommand = dbConnection.CreateCommand();
					dbTransaction = dbConnection.BeginTransaction();
					dbCommand.Connection = dbConnection;
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "SET IDENTITY_INSERT [dbo].[aspnet_Roles] ON " + Environment.NewLine + "INSERT INTO [dbo].[aspnet_Roles]([RoleId],[RoleName],[Description])VALUES(" + 0 + ",'" + SystemRoles.SystemAdministrator + " ','系统管理员') " + Environment.NewLine + "INSERT INTO [dbo].[aspnet_Roles]([RoleId],[RoleName],[Description])VALUES(" + -1 + ",'" + SystemRoles.StoreAdmin + "','门店管理员') " + Environment.NewLine + "INSERT INTO [dbo].[aspnet_Roles]([RoleId],[RoleName],[Description])VALUES(" + -2 + ",'" + SystemRoles.SupplierAdmin + "','供应商管理员') " + Environment.NewLine + "INSERT INTO [dbo].[aspnet_Roles]([RoleId],[RoleName],[Description])VALUES(" + -3 + ",'" + SystemRoles.ShoppingGuider + "','门店导购') " + Environment.NewLine + "SET IDENTITY_INSERT [dbo].[aspnet_Roles] OFF ";
					dbCommand.ExecuteNonQuery();
					dbTransaction.Commit();
					dbConnection.Close();
				}
				errorMsg = null;
				return true;
			}
			catch (SqlException ex)
			{
				errorMsg = ex.Message;
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (Exception ex2)
					{
						errorMsg = ex2.Message;
					}
				}
				if (dbConnection != null && dbConnection.State != 0)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				return false;
			}
		}

		private bool CreateAdministrator(out int managerId, out string errorMsg)
		{
			DbConnection dbConnection = null;
			DbTransaction dbTransaction = null;
			try
			{
				using (dbConnection = new SqlConnection(this.GetConnectionString()))
				{
					dbConnection.Open();
					DbCommand dbCommand = dbConnection.CreateCommand();
					dbTransaction = dbConnection.BeginTransaction();
					dbCommand.Connection = dbConnection;
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = "INSERT INTO [dbo].[aspnet_Managers]([RoleId],[UserName],[Password],[PasswordSalt],[CreateDate])VALUES(@RoleId,@UserName,@Password,@PasswordSalt,@CreateDate);SELECT @@IDENTITY";
					string text = Globals.RndStr(128, true);
					this.password = Users.EncodePassword(this.password, text);
					int num = 0;
					dbCommand.Parameters.Add(new SqlParameter("@RoleId", num));
					dbCommand.Parameters.Add(new SqlParameter("@UserName", this.username));
					dbCommand.Parameters.Add(new SqlParameter("@Password", this.password));
					dbCommand.Parameters.Add(new SqlParameter("@PasswordSalt", text));
					dbCommand.Parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
					managerId = Convert.ToInt32(dbCommand.ExecuteScalar());
					dbTransaction.Commit();
					dbConnection.Close();
				}
				errorMsg = null;
				return true;
			}
			catch (SqlException ex)
			{
				errorMsg = ex.Message;
				managerId = 0;
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (Exception ex2)
					{
						errorMsg = ex2.Message;
					}
				}
				if (dbConnection != null && dbConnection.State != 0)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
				return false;
			}
		}

		private bool AddInitData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteInitData.zh-CN.Sql");
			if (!File.Exists(text))
			{
				errorMsg = "没有找到初始化数据文件-SiteInitData.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool AddDemoData(out string errorMsg)
		{
			string text = base.Request.MapPath("SqlScripts/SiteDemo.zh-CN.sql");
			if (!File.Exists(text))
			{
				errorMsg = "没有找到演示数据文件-SiteDemo.Sql";
				return false;
			}
			return this.ExecuteScriptFile(text, out errorMsg);
		}

		private bool SaveSiteSettings(out string errorMsg)
		{
			errorMsg = null;
			if (this.siteName.Length > 30 || this.siteDescription.Length > 30)
			{
				errorMsg = "网店名称和简单介绍的长度不能超过30个字符";
				return false;
			}
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.XmlResolver = null;
                SiteSettings siteSettings = new SiteSettings();
				siteSettings.SiteUrl = base.Request.Url.Host;
				siteSettings.AliOHServerUrl = "https://openapi.alipay.com/gateway.do";
				siteSettings.SiteName = this.siteName;
				siteSettings.SiteDescription = this.siteDescription;
				siteSettings.CheckCode = Install.CreateKey(20);
				siteSettings.AppKey = this.CreateAppKey();
				siteSettings.InstallDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
				this.InitMasterSettings(siteSettings);
				this.RegisterERP(siteSettings.AppKey, siteSettings.CheckCode);
				return true;
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		public void InitMasterSettings(SiteSettings settings)
		{
			settings = SettingsManager.EncrySettings(settings);
			IDictionary<string, object> dictionary = DataHelper.ConvertEntityToObjDictionary(settings);
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string key in dictionary.Keys)
			{
				stringBuilder.AppendLine("INSERT INTO Hishop_SiteSettings ([Key],[Value]) Values('" + DataHelper.CleanSearchString(key.ToNullString()) + "','" + DataHelper.CleanSearchString(dictionary[key].ToNullString()) + "');");
			}
			DbConnection dbConnection = null;
			DbTransaction dbTransaction = null;
			if (stringBuilder != null && !string.IsNullOrEmpty(stringBuilder.ToNullString()))
			{
				using (dbConnection = new SqlConnection(this.GetConnectionString()))
				{
					dbConnection.Open();
					DbCommand dbCommand = dbConnection.CreateCommand();
					dbTransaction = dbConnection.BeginTransaction();
					dbCommand.Connection = dbConnection;
					dbCommand.Transaction = dbTransaction;
					dbCommand.CommandType = CommandType.Text;
					dbCommand.CommandText = stringBuilder.ToString();
					dbCommand.ExecuteNonQuery();
					dbTransaction.Commit();
					dbConnection.Close();
				}
			}
		}

		private void RegisterERP(string appkey, string appsecret)
		{
			string url = "http://hierp.huz.cn/api/commercialtenantregister";
			string postResult = Globals.GetPostResult(url, "appKey=" + appkey + "&appSecret=" + appsecret + "&routeAddress=" + Globals.HostPath(HiContext.Current.Context.Request.Url) + "/OpenAPI");
			Globals.AppendLog(postResult, "", "", "");
		}

		private bool SaveConfig(out string errorMsg)
		{
			try
			{
				Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				configuration.AppSettings.Settings.Remove("Installer");
				using (RijndaelManaged rijndaelManaged = this.GetCryptographer())
				{
					configuration.AppSettings.Settings["IV"].Value = Convert.ToBase64String(rijndaelManaged.IV);
					configuration.AppSettings.Settings["Key"].Value = Convert.ToBase64String(rijndaelManaged.Key);
				}
				MachineKeySection machineKeySection = (MachineKeySection)configuration.GetSection("system.web/machineKey");
				machineKeySection.ValidationKey = Install.CreateKey(20);
				machineKeySection.DecryptionKey = Install.CreateKey(24);
				machineKeySection.Validation = MachineKeyValidation.SHA1;
				machineKeySection.Decryption = "3DES";
				configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = this.GetConnectionString();
				configuration.ConnectionStrings.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
				configuration.Save();
				errorMsg = null;
				return true;
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}

		private RijndaelManaged GetCryptographer()
		{
			RijndaelManaged rijndaelManaged = new RijndaelManaged();
			rijndaelManaged.KeySize = 128;
			rijndaelManaged.GenerateIV();
			rijndaelManaged.GenerateKey();
			return rijndaelManaged;
		}

		private bool ExecuteTest()
		{
			this.errorMsgs = new List<string>();
			DbTransaction dbTransaction = null;
			DbConnection dbConnection = null;
			string item = default(string);
			try
			{
				if (this.ValidateConnectionStrings(out item))
				{
					using (dbConnection = new SqlConnection(this.GetConnectionString()))
					{
						try
						{
							dbConnection.Open();
							DbCommand dbCommand = dbConnection.CreateCommand();
							dbTransaction = dbConnection.BeginTransaction();
							dbCommand.Connection = dbConnection;
							dbCommand.Transaction = dbTransaction;
							dbCommand.CommandText = "CREATE TABLE installTest(Test bit NULL)";
							dbCommand.ExecuteNonQuery();
							dbCommand.CommandText = "DROP TABLE installTest";
							dbCommand.ExecuteNonQuery();
							dbTransaction.Commit();
							dbConnection.Close();
						}
						catch (Exception ex)
						{
							this.errorMsgs.Add(ex.Message);
						}
					}
				}
				else
				{
					this.errorMsgs.Add(item);
				}
			}
			catch (Exception ex2)
			{
				this.errorMsgs.Add(ex2.Message);
				if (dbTransaction != null)
				{
					try
					{
						dbTransaction.Rollback();
					}
					catch (Exception ex3)
					{
						this.errorMsgs.Add(ex3.Message);
					}
				}
				if (dbConnection != null && dbConnection.State != 0)
				{
					dbConnection.Close();
					dbConnection.Dispose();
				}
			}
			string folderPath = base.Request.MapPath("/config/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			try
			{
				Configuration configuration = WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
				if (configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString == "none")
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "required";
				}
				else
				{
					configuration.ConnectionStrings.ConnectionStrings["HidistroSqlServer"].ConnectionString = "none";
				}
				configuration.Save();
			}
			catch (Exception ex4)
			{
				this.errorMsgs.Add(ex4.Message);
			}
			folderPath = base.Request.MapPath("/storage/test.txt");
			if (!Install.TestFolder(folderPath, out item))
			{
				this.errorMsgs.Add(item);
			}
			foreach (string errorMsg in this.errorMsgs)
			{
				HttpContext.Current.Response.Write(errorMsg);
			}
			HttpContext.Current.Response.Write(this.errorMsgs.Count.ToString());
			return this.errorMsgs.Count == 0;
		}

		private bool ExecuteScriptFile(string pathToScriptFile, out string errorMsg)
		{
			StreamReader streamReader = null;
			SqlConnection sqlConnection = null;
			using (streamReader = new StreamReader(pathToScriptFile))
			{
				using (sqlConnection = new SqlConnection(this.GetConnectionString()))
				{
					DbCommand dbCommand = new SqlCommand
					{
						Connection = sqlConnection,
						CommandType = CommandType.Text,
						CommandTimeout = 60
					};
					sqlConnection.Open();
					while (!streamReader.EndOfStream)
					{
						try
						{
							string text = Install.NextSqlFromStream(streamReader);
							if (!string.IsNullOrEmpty(text))
							{
								dbCommand.CommandText = text;
								dbCommand.ExecuteNonQuery();
							}
						}
						catch (Exception ex)
						{
							errorMsg = ex.Message;
						}
					}
					sqlConnection.Close();
				}
				streamReader.Close();
			}
			errorMsg = null;
			return true;
		}

		private static string NextSqlFromStream(StreamReader reader)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = reader.ReadLine().Trim();
			while (!reader.EndOfStream && string.Compare(text, "GO", true, CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + Environment.NewLine);
				text = reader.ReadLine();
			}
			if (string.Compare(text, "GO", true, CultureInfo.InvariantCulture) != 0)
			{
				stringBuilder.Append(text + Environment.NewLine);
			}
			return stringBuilder.ToString();
		}

		private bool ValidateConnectionStrings(out string msg)
		{
			msg = null;
			if (string.IsNullOrEmpty(this.dbServer) || string.IsNullOrEmpty(this.dbName) || string.IsNullOrEmpty(this.dbUsername))
			{
				msg = "数据库连接信息不完整";
				return false;
			}
			return true;
		}

		private bool ValidateUser(out string msg)
		{
			msg = null;
			if (string.IsNullOrEmpty(this.username) || string.IsNullOrEmpty(this.email) || string.IsNullOrEmpty(this.password) || string.IsNullOrEmpty(this.password2))
			{
				msg = "管理员账号信息不完整";
				return false;
			}
			HiConfiguration config = HiConfiguration.GetConfig();
			if (this.username.Length > config.UsernameMaxLength || this.username.Length < config.UsernameMinLength)
			{
				msg = $"管理员用户名的长度只能在{config.UsernameMinLength}和{config.UsernameMaxLength}个字符之间";
				return false;
			}
			if (string.Compare(this.username, "anonymous", true) == 0)
			{
				msg = "不能使用anonymous作为管理员用户名";
				return false;
			}
			if (!Regex.IsMatch(this.username, config.UsernameRegex))
			{
				msg = "管理员用户名的格式不符合要求，用户名一般由字母、数字、下划线和汉字组成，且必须以汉字或字母开头";
				return false;
			}
			if (this.email.Length > 256)
			{
				msg = "电子邮件的长度必须小于256个字符";
				return false;
			}
			if (!Regex.IsMatch(this.email, config.EmailRegex))
			{
				msg = "电子邮件的格式错误";
				return false;
			}
			if (this.password != this.password2)
			{
				msg = "管理员登录密码两次输入不一致";
				return false;
			}
			if (this.password.Length < 6 || this.password.Length > config.PasswordMaxLength)
			{
				msg = $"管理员登录密码的长度只能在{6}和{config.PasswordMaxLength}个字符之间";
				return false;
			}
			return true;
		}

		private string GetConnectionString()
		{
			return $"server={this.dbServer};uid={this.dbUsername};pwd={this.dbPassword};Trusted_Connection=no;database={this.dbName}";
		}

		private static string CreateKey(int len)
		{
			byte[] array = new byte[len];
			new RNGCryptoServiceProvider().GetBytes(array);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append($"{array[i]:X2}");
			}
			return stringBuilder.ToString();
		}

		private string CreateAppKey()
		{
			string text = string.Empty;
			Random random = new Random();
			for (int i = 0; i < 7; i++)
			{
				int num = random.Next();
				text += ((char)(ushort)(48 + (ushort)(num % 7))).ToString();
			}
			return DateTime.Now.ToString("yyyyMMddHHmmss") + text;
		}

		private static bool TestFolder(string folderPath, out string errorMsg)
		{
			try
			{
				File.WriteAllText(folderPath, "Hi");
				File.AppendAllText(folderPath, ",This is a test file.");
				File.Delete(folderPath);
				errorMsg = null;
				return true;
			}
			catch (Exception ex)
			{
				errorMsg = ex.Message;
				return false;
			}
		}
	}
}
