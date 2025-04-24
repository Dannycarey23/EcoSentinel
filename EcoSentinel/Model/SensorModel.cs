using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class SensorModel
    {        
        public int sensorId { get; }
        public string sensorType { get; set; }
        public string sensorStatus { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string siteName { get; set; }
        public string siteType { get; set; }

    }
}
