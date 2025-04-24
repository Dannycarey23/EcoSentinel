using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model;

    public class SensorModel
    {
        public DatabaseService db;
        public int sensorId { get; set; } required
        public string sensorType { get; set; } required
        public string sensorStatus { get; set; } required
        public double latitude { get; set; } required
        public double longitude { get; set; } required
        public string siteName { get; set; } required
        public string siteType { get; set; } 

        public SensorModel()
        {
            db = new DatabaseService();
        }

    }
