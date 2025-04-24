using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class AirDataModel
    {
		public int dataID { get; }
		public int sensorID { get; }
		public string sensorType { get; }
		public string zone { get; }
		public string agglomeration { get; }
		public string localAuthority { get; }
		public DateOnly date { get; }
		public TimeSpan time { get; }
		public float nitrogenDioxide { get; }
		public float sulfurDioxide { get; }
		public float pmTwoPointFive { get; }
		public float pmTen { get; }


    }
}
