using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class WaterDataModel
    {
        public DatabaseService db;
        public required int dataID { get; set; } 
        public required int sensorID { get; set; } 
        public required string sensorType { get; set; } 
        public required DateOnly date { get; set; } 
        public required TimeSpan time { get; set; } 
        public required float nitrateMgl1 { get; set; } 
        public required float nitrateLessThanMgL1 { get; set; } 
        public required float phosphateMgl1 { get; set; } 
        public required float ecCfu100ml { get; set; }

        public WaterDataModel()
        {
            db = new DatabaseService();
        }
    }
}
