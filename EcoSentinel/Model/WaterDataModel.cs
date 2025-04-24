using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class WaterDataModel
    {
		public int dataID { get; }
		public int sensorID { get; }
		public string sensorType { get; }
		public DateOnly date { get; }
		public TimeSpan time { get; }
		public float nitrateMgl1 { get; }
		public float nitrateLessThanMgL1 { get; }
		public float phosphateMgl1 { get; }
		public float ecCfu100ml { get; }

    }
}
