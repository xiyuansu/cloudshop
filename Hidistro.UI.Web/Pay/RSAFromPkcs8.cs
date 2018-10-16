using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Hidistro.UI.Web.pay
{
	public sealed class RSAFromPkcs8
	{
		public static string sign(string content, string privateKey, string input_charset)
		{
			Encoding encoding = Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			SHA1 halg = new SHA1CryptoServiceProvider();
			byte[] inArray = rSACryptoServiceProvider.SignData(bytes, halg);
			return Convert.ToBase64String(inArray);
		}

		public static bool verify(string content, string signedString, string publicKey, string input_charset)
		{
			bool flag = false;
			Encoding encoding = Encoding.GetEncoding(input_charset);
			byte[] bytes = encoding.GetBytes(content);
			byte[] signature = Convert.FromBase64String(signedString);
			RSAParameters parameters = RSAFromPkcs8.ConvertFromPublicKey(publicKey);
			RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
			rSACryptoServiceProvider.ImportParameters(parameters);
			SHA1 halg = new SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.VerifyData(bytes, halg, signature);
		}

		public static string decryptData(string resData, string privateKey, string input_charset)
		{
			byte[] array = Convert.FromBase64String(resData);
			List<byte> list = new List<byte>();
			for (int i = 0; i < array.Length / 128; i++)
			{
				byte[] array2 = new byte[128];
				for (int j = 0; j < 128; j++)
				{
					array2[j] = array[j + 128 * i];
				}
				list.AddRange(RSAFromPkcs8.decrypt(array2, privateKey, input_charset));
			}
			byte[] array3 = list.ToArray();
			char[] array4 = new char[Encoding.GetEncoding(input_charset).GetCharCount(array3, 0, array3.Length)];
			Encoding.GetEncoding(input_charset).GetChars(array3, 0, array3.Length, array4, 0);
			return new string(array4);
		}

		private static byte[] decrypt(byte[] data, string privateKey, string input_charset)
		{
			RSACryptoServiceProvider rSACryptoServiceProvider = RSAFromPkcs8.DecodePemPrivateKey(privateKey);
			SHA1 sHA = new SHA1CryptoServiceProvider();
			return rSACryptoServiceProvider.Decrypt(data, false);
		}

		private static RSACryptoServiceProvider DecodePemPrivateKey(string pemstr)
		{
			byte[] array = Convert.FromBase64String(pemstr);
			if (array != null)
			{
				return RSAFromPkcs8.DecodePrivateKeyInfo(array);
			}
			return null;
		}

		private static RSACryptoServiceProvider DecodePrivateKeyInfo(byte[] pkcs8)
		{
			byte[] b = new byte[15]
			{
				48,
				13,
				6,
				9,
				42,
				134,
				72,
				134,
				247,
				13,
				1,
				1,
				1,
				5,
				0
			};
			byte[] array = new byte[15];
			MemoryStream memoryStream = new MemoryStream(pkcs8);
			int num = (int)memoryStream.Length;
			BinaryReader binaryReader = new BinaryReader(memoryStream);
			byte b2 = 0;
			ushort num2 = 0;
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
				b2 = binaryReader.ReadByte();
				if (b2 != 2)
				{
					return null;
				}
				num2 = binaryReader.ReadUInt16();
				if (num2 != 1)
				{
					return null;
				}
				array = binaryReader.ReadBytes(15);
				if (!RSAFromPkcs8.CompareBytearrays(array, b))
				{
					return null;
				}
				b2 = binaryReader.ReadByte();
				if (b2 != 4)
				{
					return null;
				}
				switch (binaryReader.ReadByte())
				{
				case 129:
					binaryReader.ReadByte();
					break;
				case 130:
					binaryReader.ReadUInt16();
					break;
				}
				byte[] privkey = binaryReader.ReadBytes((int)(num - memoryStream.Position));
				return RSAFromPkcs8.DecodeRSAPrivateKey(privkey);
			}
			catch (Exception)
			{
				return null;
			}
			finally
			{
				binaryReader.Close();
			}
		}

		private static bool CompareBytearrays(byte[] a, byte[] b)
		{
			if (a.Length != b.Length)
			{
				return false;
			}
			int num = 0;
			foreach (byte b2 in a)
			{
				if (b2 != b[num])
				{
					return false;
				}
				num++;
			}
			return true;
		}

		private static RSACryptoServiceProvider DecodeRSAPrivateKey(byte[] privkey)
		{
			MemoryStream input = new MemoryStream(privkey);
			BinaryReader binaryReader = new BinaryReader(input);
			byte b = 0;
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
				num = binaryReader.ReadUInt16();
				if (num != 258)
				{
					return null;
				}
				if (binaryReader.ReadByte() != 0)
				{
					return null;
				}
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] modulus = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] exponent = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] d = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] p = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] q = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] dP = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] dQ = binaryReader.ReadBytes(num2);
				num2 = RSAFromPkcs8.GetIntegerSize(binaryReader);
				byte[] inverseQ = binaryReader.ReadBytes(num2);
				RSACryptoServiceProvider rSACryptoServiceProvider = new RSACryptoServiceProvider();
				RSAParameters parameters = default(RSAParameters);
				parameters.Modulus = modulus;
				parameters.Exponent = exponent;
				parameters.D = d;
				parameters.P = p;
				parameters.Q = q;
				parameters.DP = dP;
				parameters.DQ = dQ;
				parameters.InverseQ = inverseQ;
				rSACryptoServiceProvider.ImportParameters(parameters);
				return rSACryptoServiceProvider;
			}
			catch (Exception)
			{
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
			b = binr.ReadByte();
			if (b != 2)
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

		private static RSAParameters ConvertFromPublicKey(string pemFileConent)
		{
			byte[] array = Convert.FromBase64String(pemFileConent);
			if (array.Length < 162)
			{
				throw new ArgumentException("pem file content is incorrect.");
			}
			byte[] array2 = new byte[128];
			byte[] array3 = new byte[3];
			Array.Copy(array, 29, array2, 0, 128);
			Array.Copy(array, 159, array3, 0, 3);
			RSAParameters result = default(RSAParameters);
			result.Modulus = array2;
			result.Exponent = array3;
			return result;
		}

		private static RSAParameters ConvertFromPrivateKey(string pemFileConent)
		{
			byte[] array = Convert.FromBase64String(pemFileConent);
			if (array.Length < 609)
			{
				throw new ArgumentException("pem file content is incorrect.");
			}
			int num = 11;
			byte[] array2 = new byte[128];
			Array.Copy(array, num, array2, 0, 128);
			num += 128;
			num += 2;
			byte[] array3 = new byte[3];
			Array.Copy(array, num, array3, 0, 3);
			num += 3;
			num += 4;
			byte[] array4 = new byte[128];
			Array.Copy(array, num, array4, 0, 128);
			num += 128;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array5 = new byte[64];
			Array.Copy(array, num, array5, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array6 = new byte[64];
			Array.Copy(array, num, array6, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array7 = new byte[64];
			Array.Copy(array, num, array7, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array8 = new byte[64];
			Array.Copy(array, num, array8, 0, 64);
			num += 64;
			num += ((array[num + 1] == 64) ? 2 : 3);
			byte[] array9 = new byte[64];
			Array.Copy(array, num, array9, 0, 64);
			RSAParameters result = default(RSAParameters);
			result.Modulus = array2;
			result.Exponent = array3;
			result.D = array4;
			result.P = array5;
			result.Q = array6;
			result.DP = array7;
			result.DQ = array8;
			result.InverseQ = array9;
			return result;
		}
	}
}
