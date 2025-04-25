using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class WeatherDataModel
    {
        public DatabaseService db;
        public required int dataID { get; set; }
        public required int sensorID { get; set; }
        public required string sensorType { get; set; }
        public required float elevation { get; set; }
        public required int utcOffsetSeconds { get; set; }
        public required string timezone { get; set; }
        public required string timezoneAbr { get; set; }
        public required DateOnly date { get; set; }
        public required TimeSpan time { get; set; }
        public required float temp2m { get; set; }
        public required float relativeHumidity2m { get; set; }
        public required float windSpeed { get; set; }
        public required int windDirection { get; set; } 
    
    public WeatherDataModel()
        {
            db = new DatabaseService();
        }
    }

}