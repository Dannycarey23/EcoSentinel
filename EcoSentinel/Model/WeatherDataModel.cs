using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class WeatherDataModel
    {
		public int dataID { get; }
		public int sensorID { get; }
		public string sensorType { get; }
		public float elevation { get; } 
		public int utcOffsetSeconds { get; }
		public string timezone { get; }
		public string timezoneAbr { get; }
		public DateOnly date { get; }
		public TimeSpan time { get; }
		public float temp2m { get; }
		public float relativeHumidity2m { get; }
		public float windSpeed { get; }
		public int windDirection { get; }
 
    }
}
