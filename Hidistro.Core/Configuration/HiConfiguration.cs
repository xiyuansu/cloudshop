using Hidistro.Core.Enums;
using System;
using System.Globalization;
using System.IO;
using System.Xml;

namespace Hidistro.Core.Configuration
{
	public class HiConfiguration
	{
		private const string ConfigCacheKey = "FileCache-Configuragion";

		private short smtpServerConnectionLimit = -1;

		private SSLSettings ssl = SSLSettings.Ignore;

		private int usernameMinLength = 3;

		private int usernameMaxLength = 20;

		private string usernameRegex = "[一-龥a-zA-Z0-9]+[一-龥_a-zA-Z0-9]*";

		private string emailEncoding = "utf-8";

		private int shippingAddressQuantity = 5;

		private int passwordMaxLength = 16;

		private string emailRegex = "([a-zA-Z\\.0-9_-])+@([a-zA-Z0-9_-])+((\\.[a-zA-Z0-9_-]{2,3,4}){1,2})";

		private string adminFolder = "admin";

		private bool useUniversalCode = false;

		private string version = "2.1";

		private string cachePolicy = "aspnetcache";

		private string imageServerUrl = string.Empty;

		public static readonly int[,] ThumbnailSizes = new int[6, 2]
		{
			{
				10,
				10
			},
			{
				22,
				22
			},
			{
				40,
				40
			},
			{
				100,
				100
			},
			{
				160,
				160
			},
			{
				310,
				310
			}
		};

		public int QueuedThreads
		{
			get
			{
				return 2;
			}
		}

		public SSLSettings SSL
		{
			get
			{
				return this.ssl;
			}
		}

		public short SmtpServerConnectionLimit
		{
			get
			{
				return this.smtpServerConnectionLimit;
			}
		}

		public int UsernameMinLength
		{
			get
			{
				return this.usernameMinLength;
			}
		}

		public int UsernameMaxLength
		{
			get
			{
				return this.usernameMaxLength;
			}
		}

		public string UsernameRegex
		{
			get
			{
				return this.usernameRegex;
			}
		}

		public string EmailEncoding
		{
			get
			{
				return this.emailEncoding;
			}
		}

		public int ShippingAddressQuantity
		{
			get
			{
				return this.shippingAddressQuantity;
			}
		}

		public int PasswordMaxLength
		{
			get
			{
				return this.passwordMaxLength;
			}
		}

		public string EmailRegex
		{
			get
			{
				return this.emailRegex;
			}
		}

		public string AdminFolder
		{
			get
			{
				return this.adminFolder;
			}
		}

		public string Version
		{
			get
			{
				return this.version;
			}
		}

		public bool UseUniversalCode
		{
			get
			{
				return this.useUniversalCode;
			}
		}

		public string CachePolicy
		{
			get
			{
				return this.cachePolicy;
			}
		}

		public string ImageServerUrl
		{
			get
			{
				return this.imageServerUrl;
			}
		}

		public HiConfiguration(XmlDocument doc)
		{
			XmlNode xmlNode = doc.SelectSingleNode("Hishop/Core");
			XmlAttributeCollection attributes = xmlNode.Attributes;
			this.GetAttributes(attributes);
		}

		public XmlNode GetConfigSection(string nodePath)
		{
			return HiConfiguration.GetXmlDocument().SelectSingleNode(nodePath);
		}

		public static HiConfiguration GetConfig()
		{
			return new HiConfiguration(HiConfiguration.GetXmlDocument());
		}

		private static XmlDocument GetXmlDocument()
		{
			XmlDocument xmlDocument = new AspNetCache().Get<XmlDocument>("FileCache-Configuragion");
			if (xmlDocument == null)
			{
				string filename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config\\Hishop.config");
				xmlDocument = new XmlDocument();
				xmlDocument.Load(filename);
				new AspNetCache().Insert("FileCache-Configuragion", xmlDocument, 36000, false);
			}
			return xmlDocument;
		}

		internal void GetAttributes(XmlAttributeCollection attributeCollection)
		{
			XmlAttribute xmlAttribute = attributeCollection["smtpServerConnectionLimit"];
			if (xmlAttribute != null)
			{
				this.smtpServerConnectionLimit = short.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
			}
			else
			{
				this.smtpServerConnectionLimit = -1;
			}
			xmlAttribute = attributeCollection["ssl"];
			if (xmlAttribute != null)
			{
				this.ssl = (SSLSettings)Enum.Parse(typeof(SSLSettings), xmlAttribute.Value, true);
			}
			xmlAttribute = attributeCollection["usernameMinLength"];
			if (xmlAttribute != null)
			{
				this.usernameMinLength = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["usernameMaxLength"];
			if (xmlAttribute != null)
			{
				this.usernameMaxLength = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["usernameRegex"];
			if (xmlAttribute != null)
			{
				this.usernameRegex = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["emailEncoding"];
			if (xmlAttribute != null)
			{
				this.emailEncoding = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["shippingAddressQuantity"];
			if (xmlAttribute != null)
			{
				this.shippingAddressQuantity = int.Parse(xmlAttribute.Value);
			}
			xmlAttribute = attributeCollection["passwordMaxLength"];
			if (xmlAttribute != null)
			{
				this.passwordMaxLength = int.Parse(xmlAttribute.Value);
			}
			if (this.passwordMaxLength < 6)
			{
				this.passwordMaxLength = 16;
			}
			xmlAttribute = attributeCollection["emailRegex"];
			if (xmlAttribute != null)
			{
				this.emailRegex = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["adminFolder"];
			if (xmlAttribute != null)
			{
				this.adminFolder = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["version"];
			if (xmlAttribute != null)
			{
				this.version = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["useUniversalCode"];
			if (xmlAttribute?.Value.Equals("true") ?? false)
			{
				this.useUniversalCode = true;
			}
			xmlAttribute = attributeCollection["cachePolicy"];
			if (xmlAttribute != null)
			{
				this.cachePolicy = xmlAttribute.Value;
			}
			xmlAttribute = attributeCollection["imageServerUrl"];
			if (xmlAttribute != null)
			{
				this.imageServerUrl = xmlAttribute.Value;
			}
		}
	}
}
