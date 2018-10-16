using Hidistro.Entities.Depot;
using Hidistro.Entities.Members;
using System;

namespace Hidistro.Entities
{
	public class StoreBaseEntity : StoreBase
	{
		public PositionInfo Position
		{
			get;
			set;
		}

		public StoreDeliveryInfo Delivery
		{
			get;
			set;
		}

		public string AddressSimply
		{
			get;
			set;
		}

		public string Address
		{
			get;
			set;
		}

		public string FullRegionPath
		{
			get;
			set;
		}

		public bool IsOpen
		{
			get;
			set;
		}

		public DateTime? CloseStartTime
		{
			get;
			set;
		}

		public DateTime? CloseEndTime
		{
			get;
			set;
		}

		public DateTime OpenStartTime
		{
			get;
			set;
		}

		public DateTime OpenEndTime
		{
			get;
			set;
		}

		private double DistanceOrgi
		{
			get;
			set;
		}

		public string Distance
		{
			get
			{
				double num;
				if (this.DistanceOrgi >= 1000.0)
				{
					num = this.DistanceOrgi / 1000.0;
					return num.ToString("F1") + "KM";
				}
				num = this.DistanceOrgi;
				return num.ToString("F0") + "M";
			}
			set
			{
				this.DistanceOrgi = double.Parse(value);
			}
		}

		public bool IsInServiceArea
		{
			get;
			set;
		}
	}
}
