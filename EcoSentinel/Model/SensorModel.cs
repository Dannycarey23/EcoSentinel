using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model;

    public class SensorModel
    {
        public DatabaseService db;
        public required int sensorId { get; set; } 
        public required string sensorType { get; set; } 
        public required string sensorStatus { get; set; } 
        public required double latitude { get; set; } 
        public required double longitude { get; set; } 
        public required string siteName { get; set; } 
        public required string siteType { get; set; } 

        public SensorModel()
        {
            db = new DatabaseService();
        }

    }
