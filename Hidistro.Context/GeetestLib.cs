using Hidistro.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.Context
{
	public class GeetestLib
	{
		public const string version = "3.2.0";

		public const string sdkLang = "csharp";

		protected const string apiUrl = "http://api.geetest.com";

		protected const string registerUrl = "/register.php";

		protected const string validateUrl = "/validate.php";

		public const string gtServerStatusSessionKey = "gt_server_status";

		public const string fnGeetestChallenge = "geetest_challenge";

		public const string fnGeetestValidate = "geetest_validate";

		public const string fnGeetestSeccode = "geetest_seccode";

		private string userID = "";

		private string responseStr = "";

		private string captchaID = "";

		private string privateKey = "";

		public const int successResult = 1;

		public const int failResult = 0;

		public const string forbiddenResult = "forbidden";

		public GeetestLib(string publicKey, string privateKey)
		{
			this.privateKey = privateKey;
			this.captchaID = publicKey;
		}

		private int getRandomNum()
		{
			Random random = new Random();
			return random.Next(100);
		}

		public byte preProcess()
		{
			if (this.captchaID == null)
			{
				Globals.AppendLog("publicKey is null!", "", "", "geetestlog.txt");
			}
			else
			{
				string text = this.registerChallenge();
				if (text.Length == 32)
				{
					this.getSuccessPreProcessRes(text);
					return 1;
				}
				this.getFailPreProcessRes();
				Globals.AppendLog("Server regist challenge failed!", "", "", "geetestlog.txt");
			}
			return 0;
		}

		public byte preProcess(string userID)
		{
			if (this.captchaID == null)
			{
				Globals.AppendLog("publicKey is null!", "", "", "geetestlog.txt");
			}
			else
			{
				this.userID = userID;
				string text = this.registerChallenge();
				if (text.Length == 32)
				{
					this.getSuccessPreProcessRes(text);
					return 1;
				}
				this.getFailPreProcessRes();
				Globals.AppendLog("Server regist challenge failed!", "", "", "geetestlog.txt");
			}
			return 0;
		}

		public string getResponseStr()
		{
			return this.responseStr;
		}

		private void getFailPreProcessRes()
		{
			int randomNum = this.getRandomNum();
			int randomNum2 = this.getRandomNum();
			string str = this.md5Encode(string.Concat(randomNum));
			string text = this.md5Encode(string.Concat(randomNum2));
			string arg = str + text.Substring(0, 2);
			this.responseStr = "{" + $"\"success\":{0},\"gt\":\"{this.captchaID}\",\"challenge\":\"{arg}\"" + "}";
		}

		private void getSuccessPreProcessRes(string challenge)
		{
			challenge = this.md5Encode(challenge + this.privateKey);
			this.responseStr = "{" + $"\"success\":{1},\"gt\":\"{this.captchaID}\",\"challenge\":\"{challenge}\"" + "}";
		}

		public int failbackValidateRequest(string challenge, string validate, string seccode)
		{
			if (!this.requestIsLegal(challenge, validate, seccode))
			{
				return 0;
			}
			string[] array = validate.Split('_');
			string str = array[0];
			string str2 = array[1];
			string str3 = array[2];
			int ans = this.decodeResponse(challenge, str);
			int full_bg_index = this.decodeResponse(challenge, str2);
			int img_grp_index = this.decodeResponse(challenge, str3);
			return this.validateFailImage(ans, full_bg_index, img_grp_index);
		}

		private int validateFailImage(int ans, int full_bg_index, int img_grp_index)
		{
			string source = this.md5Encode(string.Concat(full_bg_index)).Substring(0, 10);
			string source2 = this.md5Encode(string.Concat(img_grp_index)).Substring(10, 10);
			string text = "";
			for (int i = 0; i < 9; i++)
			{
				char c;
				if (i % 2 == 0)
				{
					string str = text;
					c = source.ElementAt(i);
					text = str + c.ToString();
				}
				else if (i % 2 == 1)
				{
					string str2 = text;
					c = source2.ElementAt(i);
					text = str2 + c.ToString();
				}
			}
			string value = text.Substring(4);
			int num = Convert.ToInt32(value, 16);
			int num2 = num % 200;
			if (num2 < 40)
			{
				num2 = 40;
			}
			if (Math.Abs(ans - num2) < 3)
			{
				return 1;
			}
			return 0;
		}

		private bool requestIsLegal(string challenge, string validate, string seccode)
		{
			if (challenge.Equals(string.Empty) || validate.Equals(string.Empty) || seccode.Equals(string.Empty))
			{
				return false;
			}
			return true;
		}

		public int enhencedValidateRequest(string challenge, string validate, string seccode)
		{
			if (!this.requestIsLegal(challenge, validate, seccode))
			{
				return 0;
			}
			if (validate.Length > 0 && this.checkResultByPrivate(challenge, validate))
			{
				string data = "seccode=" + seccode + "&sdk=csharp_3.2.0";
				string text = "";
				try
				{
					text = this.postValidate(data);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
				if (text.Equals(this.md5Encode(seccode)))
				{
					return 1;
				}
			}
			return 0;
		}

		public int enhencedValidateRequest(string challenge, string validate, string seccode, string userID)
		{
			if (!this.requestIsLegal(challenge, validate, seccode))
			{
				return 0;
			}
			if (validate.Length > 0 && this.checkResultByPrivate(challenge, validate))
			{
				string data = "seccode=" + seccode + "&user_id=" + userID + "&sdk=csharp_3.2.0";
				string text = "";
				try
				{
					text = this.postValidate(data);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
				}
				if (text.Equals(this.md5Encode(seccode)))
				{
					return 1;
				}
			}
			return 0;
		}

		private string readContentFromGet(string url)
		{
			try
			{
				HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
				httpWebRequest.Timeout = 20000;
				HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				Stream responseStream = httpWebResponse.GetResponseStream();
				StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				responseStream.Close();
				return result;
			}
			catch
			{
				return "";
			}
		}

		private string registerChallenge()
		{
			string text = "";
			text = ((!string.Empty.Equals(this.userID)) ? string.Format("{0}{1}?gt={2}&user_id={3}", "http://api.geetest.com", "/register.php", this.captchaID, this.userID) : string.Format("{0}{1}?gt={2}", "http://api.geetest.com", "/register.php", this.captchaID));
			return this.readContentFromGet(text);
		}

		private bool checkResultByPrivate(string origin, string validate)
		{
			string value = this.md5Encode(this.privateKey + "geetest" + origin);
			return validate.Equals(value);
		}

		private string postValidate(string data)
		{
			string requestUriString = string.Format("{0}{1}", "http://api.geetest.com", "/validate.php");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
			httpWebRequest.Method = "POST";
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.ContentLength = Encoding.UTF8.GetByteCount(data);
			Stream requestStream = httpWebRequest.GetRequestStream();
			byte[] bytes = Encoding.ASCII.GetBytes(data);
			requestStream.Write(bytes, 0, bytes.Length);
			requestStream.Close();
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			Stream responseStream = httpWebResponse.GetResponseStream();
			StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
			string result = streamReader.ReadToEnd();
			streamReader.Close();
			responseStream.Close();
			return result;
		}

		private int decodeRandBase(string challenge)
		{
			string text = challenge.Substring(32, 2);
			List<int> list = new List<int>();
			foreach (int num in text)
			{
				list.Add((num > 57) ? (num - 87) : (num - 48));
			}
			return list.ElementAt(0) * 36 + list.ElementAt(1);
		}

		private int decodeResponse(string challenge, string str)
		{
			if (str.Length > 100)
			{
				return 0;
			}
			int[] array = new int[5]
			{
				1,
				2,
				5,
				10,
				50
			};
			string text = "";
			Hashtable hashtable = new Hashtable();
			int num = 0;
			char c;
			for (int i = 0; i < challenge.Length; i++)
			{
				c = challenge.ElementAt(i);
				string text2 = c.ToString() ?? "";
				if (!text.Contains(text2))
				{
					int num2 = array[num % 5];
					text += text2;
					num++;
					hashtable.Add(text2, num2);
				}
			}
			int num3 = 0;
			for (int j = 0; j < str.Length; j++)
			{
				int num4 = num3;
				Hashtable hashtable2 = hashtable;
				c = str[j];
				num3 = num4 + (int)hashtable2[c.ToString() ?? ""];
			}
			return num3 - this.decodeRandBase(challenge);
		}

		private string md5Encode(string plainText)
		{
			MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
			string text = BitConverter.ToString(mD5CryptoServiceProvider.ComputeHash(Encoding.Default.GetBytes(plainText)));
			text = text.Replace("-", "");
			return text.ToLower();
		}
	}
}
