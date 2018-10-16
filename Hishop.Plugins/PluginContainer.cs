using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;

namespace Hishop.Plugins
{
	public abstract class PluginContainer
	{
		protected static volatile Cache pluginCache = HttpRuntime.Cache;

		protected abstract string PluginLocalPath
		{
			get;
		}

		protected abstract string PluginVirtualPath
		{
			get;
		}

		protected abstract string IndexCacheKey
		{
			get;
		}

		protected abstract string TypeCacheKey
		{
			get;
		}

		protected PluginContainer()
		{
			PluginContainer.pluginCache.Remove(this.IndexCacheKey);
			PluginContainer.pluginCache.Remove(this.TypeCacheKey);
		}

		protected void VerifyIndex()
		{
			if (PluginContainer.pluginCache.Get(this.IndexCacheKey) == null)
			{
				XmlDocument xmlDocument = new XmlDocument();
				XmlNode xmlNode = xmlDocument.CreateElement("Plugins");
				this.BuildIndex(xmlDocument, xmlNode);
				xmlDocument.AppendChild(xmlNode);
				PluginContainer.pluginCache.Insert(this.IndexCacheKey, xmlDocument, new CacheDependency(this.PluginLocalPath));
			}
		}

		private void BuildIndex(XmlDocument catalog, XmlNode mapNode)
		{
			if (Directory.Exists(this.PluginLocalPath))
			{
				string[] files = Directory.GetFiles(this.PluginLocalPath, "*.dll", SearchOption.AllDirectories);
				string fullName = typeof(IPlugin).FullName;
				string[] array = files;
				foreach (string filename in array)
				{
					Assembly assembly = Assembly.Load(PluginContainer.LoadPlugin(filename));
					Type[] exportedTypes = assembly.GetExportedTypes();
					foreach (Type t in exportedTypes)
					{
						if (PluginContainer.CheckIsPlugin(t, fullName))
						{
							this.AddPlugin(t, filename, catalog, mapNode);
						}
					}
				}
			}
		}

		private Type GetPlugin(string baseName, string name, string attname)
		{
			Hashtable hashtable = this.GetPluginCache();
			name = name.ToLower();
			Type type = hashtable[name] as Type;
			if (type == null)
			{
				if (PluginContainer.pluginCache.Get(this.IndexCacheKey) == null)
				{
					return null;
				}
				XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
				XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("//" + baseName + "/item[@" + attname + "='" + name + "']");
				if (xmlNode == null || !File.Exists(xmlNode.Attributes["file"].Value))
				{
					return null;
				}
				Assembly assembly = Assembly.Load(PluginContainer.LoadPlugin(xmlNode.Attributes["file"].Value));
				type = assembly.GetType(xmlNode.Attributes["identity"].Value, false, true);
				if (type != null)
				{
					hashtable[name] = type;
				}
			}
			return type;
		}

		internal virtual Type GetPlugin(string baseName, string name)
		{
			return this.GetPlugin(baseName, name, "identity");
		}

		internal virtual Type GetPluginWithNamespace(string baseName, string name)
		{
			return this.GetPlugin(baseName, name, "namespace");
		}

		private Hashtable GetPluginCache()
		{
			Hashtable hashtable = PluginContainer.pluginCache.Get(this.TypeCacheKey) as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				PluginContainer.pluginCache.Insert(this.TypeCacheKey, hashtable, new CacheDependency(this.PluginLocalPath));
			}
			return hashtable;
		}

		private void AddPlugin(Type t, string filename, XmlDocument catalog, XmlNode mapNode)
		{
			XmlNode xmlNode = mapNode.SelectSingleNode(t.BaseType.Name);
			if (xmlNode == null)
			{
				xmlNode = catalog.CreateElement(t.BaseType.Name);
				mapNode.AppendChild(xmlNode);
			}
			XmlNode xmlNode2 = catalog.CreateElement("item");
			XmlAttribute xmlAttribute = catalog.CreateAttribute("identity");
			xmlAttribute.Value = t.FullName.ToLower();
			xmlNode2.Attributes.Append(xmlAttribute);
			XmlAttribute xmlAttribute2 = catalog.CreateAttribute("file");
			xmlAttribute2.Value = filename;
			xmlNode2.Attributes.Append(xmlAttribute2);
			PluginAttribute pluginAttribute = (PluginAttribute)Attribute.GetCustomAttribute(t, typeof(PluginAttribute));
			if (pluginAttribute != null)
			{
				XmlAttribute xmlAttribute3 = catalog.CreateAttribute("name");
				xmlAttribute3.Value = pluginAttribute.Name;
				xmlNode2.Attributes.Append(xmlAttribute3);
				XmlAttribute xmlAttribute4 = catalog.CreateAttribute("seq");
				xmlAttribute4.Value = ((pluginAttribute.Sequence > 0) ? pluginAttribute.Sequence.ToString(CultureInfo.InvariantCulture) : "0");
				xmlNode2.Attributes.Append(xmlAttribute4);
				ConfigablePlugin configablePlugin = Activator.CreateInstance(t) as ConfigablePlugin;
				XmlAttribute xmlAttribute5 = catalog.CreateAttribute("logo");
				if (string.IsNullOrEmpty(configablePlugin.Logo) || configablePlugin.Logo.Trim().Length == 0)
				{
					xmlAttribute5.Value = "";
				}
				else
				{
					xmlAttribute5.Value = this.PluginVirtualPath + "/images/" + configablePlugin.Logo.Trim();
				}
				xmlNode2.Attributes.Append(xmlAttribute5);
				XmlAttribute xmlAttribute6 = catalog.CreateAttribute("shortDescription");
				xmlAttribute6.Value = configablePlugin.ShortDescription;
				xmlNode2.Attributes.Append(xmlAttribute6);
				XmlAttribute xmlAttribute7 = catalog.CreateAttribute("description");
				xmlAttribute7.Value = configablePlugin.Description;
				xmlNode2.Attributes.Append(xmlAttribute7);
			}
			XmlAttribute xmlAttribute8 = catalog.CreateAttribute("namespace");
			xmlAttribute8.Value = t.Namespace.ToLower();
			xmlNode2.Attributes.Append(xmlAttribute8);
			if (pluginAttribute != null && pluginAttribute.Sequence > 0)
			{
				XmlNode xmlNode3 = PluginContainer.FindNode(xmlNode.ChildNodes, pluginAttribute.Sequence);
				if (xmlNode3 == null)
				{
					xmlNode.AppendChild(xmlNode2);
				}
				else
				{
					xmlNode.InsertBefore(xmlNode2, xmlNode3);
				}
			}
			else
			{
				xmlNode.AppendChild(xmlNode2);
			}
		}

		private static XmlNode FindNode(XmlNodeList nodeList, int sequence)
		{
			if (nodeList == null || nodeList.Count == 0 || sequence <= 0)
			{
				return null;
			}
			for (int i = 0; i < nodeList.Count; i++)
			{
				if (int.Parse(nodeList[i].Attributes["seq"].Value) > sequence)
				{
					return nodeList[i];
				}
			}
			return null;
		}

		private static byte[] LoadPlugin(string filename)
		{
			byte[] array = default(byte[]);
			using (FileStream fileStream = new FileStream(filename, FileMode.Open))
			{
				array = new byte[(int)fileStream.Length];
				fileStream.Read(array, 0, array.Length);
			}
			return array;
		}

		private static bool CheckIsPlugin(Type t, string interfaceName)
		{
			try
			{
				if (t == null || !t.IsClass || !t.IsPublic || t.IsAbstract || t.GetInterface(interfaceName) == null)
				{
					return false;
				}
				return true;
			}
			catch
			{
				return false;
			}
		}

		public abstract PluginItemCollection GetPlugins();

		public abstract PluginItem GetPluginItem(string fullName);

		protected PluginItem GetPluginItem(string baseName, string fullName)
		{
			PluginItem result = null;
			XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
			XmlNode xmlNode = xmlDocument.SelectSingleNode("//" + baseName + "/item[@identity='" + fullName + "']");
			if (xmlNode != null)
			{
				PluginItem pluginItem = new PluginItem();
				pluginItem.FullName = xmlNode.Attributes["identity"].Value;
				pluginItem.DisplayName = xmlNode.Attributes["name"].Value;
				pluginItem.Logo = xmlNode.Attributes["logo"].Value;
				pluginItem.ShortDescription = xmlNode.Attributes["shortDescription"].Value;
				pluginItem.Description = xmlNode.Attributes["description"].Value;
				result = pluginItem;
			}
			return result;
		}

		protected PluginItemCollection GetPlugins(string baseName)
		{
			PluginItemCollection pluginItemCollection = new PluginItemCollection();
			XmlDocument xmlDocument = PluginContainer.pluginCache.Get(this.IndexCacheKey) as XmlDocument;
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("//" + baseName + "/item");
			if (xmlNodeList != null && xmlNodeList.Count > 0)
			{
				foreach (XmlNode item2 in xmlNodeList)
				{
					PluginItem pluginItem = new PluginItem();
					pluginItem.FullName = item2.Attributes["identity"].Value;
					pluginItem.DisplayName = item2.Attributes["name"].Value;
					pluginItem.Logo = item2.Attributes["logo"].Value;
					pluginItem.ShortDescription = item2.Attributes["shortDescription"].Value;
					pluginItem.Description = item2.Attributes["description"].Value;
					PluginItem item = pluginItem;
					pluginItemCollection.Add(item);
				}
			}
			return pluginItemCollection;
		}
	}
}
