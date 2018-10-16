using Hidistro.Context;
using Hidistro.Core;
using Hishop.Alipay.OpenHome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.UI.Web.App_Code
{
	public class AliOHClientHelper
	{
		public static AlipayOHClient Instance(string serverRootPath)
		{
			SiteSettings masterSettings = SettingsManager.GetMasterSettings();
			string aliOHServerUrl = masterSettings.AliOHServerUrl;
			string aliOHAppId = masterSettings.AliOHAppId;
			string text = serverRootPath + "/config/rsa_private_key.pem";
			string aliPubKey = serverRootPath + "/config/alipay_pubKey.pem";
			string pubKey = serverRootPath + "/config/rsa_public_key.pem";
			AliOHClientHelper.LoadCertificateFile(text);
			return new AlipayOHClient(aliOHServerUrl, aliOHAppId, aliPubKey, text, pubKey, "UTF-8");
		}

		private static RSACryptoServiceProvider LoadCertificateFile(string filename)
		{
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("priKeyFile", filename);
			using (FileStream fileStream = File.OpenRead(filename))
			{
				byte[] array = new byte[fileStream.Length];
				byte[] privkey = null;
				fileStream.Read(array, 0, array.Length);
				if (array[0] != 48)
				{
					privkey = AliOHClientHelper.GetPem("RSA PRIVATE KEY", array);
				}
				try
				{
					return AliOHClientHelper.DecodeRSAPrivateKey(privkey);
				}
				catch (Exception ex)
				{
					Globals.WriteExceptionLog(ex, dictionary, "LoadCertificateFile");
				}
				return null;
			}
		}

		private static byte[] GetPem(string type, byte[] data)
		{
			string @string = Encoding.UTF8.GetString(data);
			string text = $"-----BEGIN {type}-----\\n";
			string value = $"-----END {type}-----";
			int num = @string.IndexOf(text) + text.Length;
			int num2 = @string.IndexOf(value, num);
			return Convert.FromBase64String(@string.Substring(num, num2 - num));
		}

		private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
		{
			MemoryStream input = new MemoryStream(privkey);
			BinaryReader binaryReader = new BinaryReader(input);
			IDictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("priKeyFile", binaryReader.ReadString().ToNullString());
			ushort num = 0;
			int num2 = 0;
			try
			{
				switch (binaryReader.ReadUInt16())
				{
				case 33072:
					binaryReader.ReadByte();
					break;
				case 33328:
					binaryReader.ReadInt16();
					break;
				default:
					return null;
				}
				if (binaryReader.ReadUInt16() != 258)
				{
					return null;
				}
				if (binaryReader.ReadByte() != 0)
				{
					return null;
				}
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] modulus = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] exponent = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] d = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] p = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] q = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] dP = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] dQ = binaryReader.ReadBytes(num2);
				num2 = AliOHClientHelper.GetIntegerSize(binaryReader);
				byte[] inverseQ = binaryReader.ReadBytes(num2);
				CspParameters parameters = new CspParameters
				{
					Flags = CspProviderFlags.UseMachineKeyStore
				};
				RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider(1024, parameters);
				RSAParameters rSAParameters = default(RSAParameters);
				rSAParameters.Modulus = modulus;
				rSAParameters.Exponent = exponent;
				rSAParameters.D = d;
				rSAParameters.P = p;
				rSAParameters.Q = q;
				rSAParameters.DP = dP;
				rSAParameters.DQ = dQ;
				rSAParameters.InverseQ = inverseQ;
				RSAParameters parameters2 = rSAParameters;
				rSACryptoServiceProvider.ImportParameters(parameters2);
				return rSACryptoServiceProvider;
			}
			catch (Exception ex)
			{
				Globals.WriteExceptionLog(ex, dictionary, "DecodeRSAPrivateKey");
				return null;
			}
			finally
			{
				binaryReader.Close();
			}
		}

		private static int GetIntegerSize(BinaryReader binr)
		{
			byte b = 0;
			byte b2 = 0;
			byte b3 = 0;
			int num = 0;
			if (binr.ReadByte() != 2)
			{
				return 0;
			}
			b = binr.ReadByte();
			switch (b)
			{
			case 129:
				num = binr.ReadByte();
				break;
			case 130:
			{
				b3 = binr.ReadByte();
				b2 = binr.ReadByte();
				byte[] value = new byte[4]
				{
					b2,
					b3,
					0,
					0
				};
				num = BitConverter.ToInt32(value, 0);
				break;
			}
			default:
				num = b;
				break;
			}
			while (binr.ReadByte() == 0)
			{
				num--;
			}
			binr.BaseStream.Seek(-1L, SeekOrigin.Current);
			return num;
		}
	}
}
