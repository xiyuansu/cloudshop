using System;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace Hidistro.Core.Jobs
{
	[Serializable]
	[XmlRoot("job")]
	public class Job : IDisposable
	{
		private IJob _ijob;

		private bool _enabled = true;

		private Type _jobType;

		private string _name;

		private bool _enableShutDown = false;

		private int _minutes = 15;

		[NonSerialized]
		private Timer _timer = null;

		private bool disposed = false;

		[NonSerialized]
		private XmlNode _node = null;

		private bool _singleThread = true;

		private DateTime _lastStart;

		private DateTime _lastSucess;

		private DateTime _lastEnd;

		private bool _isRunning;

		private int _seconds = -1;

		protected int Interval
		{
			get
			{
				if (this._seconds > 0)
				{
					return this._seconds * 1000;
				}
				return this.Minutes * 60000;
			}
		}

		public bool IsRunning
		{
			get
			{
				return this._isRunning;
			}
		}

		public DateTime LastStarted
		{
			get
			{
				return this._lastStart;
			}
		}

		public DateTime LastEnd
		{
			get
			{
				return this._lastEnd;
			}
		}

		public DateTime LastSuccess
		{
			get
			{
				return this._lastSucess;
			}
		}

		public bool SingleThreaded
		{
			get
			{
				return this._singleThread;
			}
		}

		public Type JobType
		{
			get
			{
				return this._jobType;
			}
		}

		public int Minutes
		{
			get
			{
				return this._minutes;
			}
			set
			{
				this._minutes = value;
			}
		}

		public bool EnableShutDown
		{
			get
			{
				return this._enableShutDown;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
		}

		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
		}

		public Job(Type ijob, XmlNode node)
		{
			if (node != null)
			{
				this._node = node;
				this._jobType = ijob;
				XmlAttribute xmlAttribute = node.Attributes["enabled"];
				if (xmlAttribute != null)
				{
					this._enabled = bool.Parse(xmlAttribute.Value);
				}
				xmlAttribute = node.Attributes["enableShutDown"];
				if (xmlAttribute != null)
				{
					this._enableShutDown = bool.Parse(xmlAttribute.Value);
				}
				xmlAttribute = node.Attributes["name"];
				if (xmlAttribute != null)
				{
					this._name = xmlAttribute.Value;
				}
				xmlAttribute = node.Attributes["seconds"];
				if (xmlAttribute != null)
				{
					this._seconds = int.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
				}
				xmlAttribute = node.Attributes["minutes"];
				if (xmlAttribute != null)
				{
					try
					{
						this._minutes = int.Parse(xmlAttribute.Value, CultureInfo.InvariantCulture);
					}
					catch
					{
						this._minutes = 15;
					}
				}
				xmlAttribute = node.Attributes["singleThread"];
				if (xmlAttribute != null && !string.IsNullOrEmpty(xmlAttribute.Value) && string.Compare(xmlAttribute.Value, "false", false, CultureInfo.InvariantCulture) == 0)
				{
					this._singleThread = false;
				}
			}
		}

		public void InitializeTimer()
		{
			if (this._timer == null && this.Enabled)
			{
				this._timer = new Timer(this.timer_Callback, null, this.Interval, this.Interval);
			}
		}

		private void timer_Callback(object state)
		{
			if (this.Enabled)
			{
				this._timer.Change(-1, -1);
				this.ExecuteJob();
				if (this.Enabled)
				{
					this._timer.Change(this.Interval, this.Interval);
				}
				else
				{
					this.Dispose();
				}
			}
		}

		public void ExecuteJob()
		{
			this._isRunning = true;
			IJob job = this.CreateJobInstance();
			if (job != null)
			{
				this._lastStart = DateTime.Now;
				try
				{
					job.Execute(this._node);
					this._lastEnd = (this._lastSucess = DateTime.Now);
				}
				catch (Exception)
				{
					this._enabled = !this.EnableShutDown;
					this._lastEnd = DateTime.Now;
				}
			}
			this._isRunning = false;
		}

		public IJob CreateJobInstance()
		{
			if (this.Enabled && this._ijob == null)
			{
				if (this._jobType != (Type)null)
				{
					this._ijob = (Activator.CreateInstance(this._jobType) as IJob);
				}
				this._enabled = (this._ijob != null);
				if (!this._enabled)
				{
					this.Dispose();
				}
			}
			return this._ijob;
		}

		public void Dispose()
		{
			if (this._timer != null && !this.disposed)
			{
				lock (this)
				{
					this._timer.Dispose();
					this._timer = null;
					this.disposed = true;
				}
			}
		}
	}
}
