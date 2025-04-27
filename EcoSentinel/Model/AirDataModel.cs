using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EcoSentinel.Model
{
    public class AirDataModel
    {
        public DatabaseService db;
        public required int dataID { get; set; } 
        public required int sensorID { get; set; } 
        public required string sensorType { get; set; } 
        public required string zone { get; set; }  
		public required string agglomeration { get; set; } 
        public required string localAuthority { get; set; } 
		public required DateOnly date { get; set; } 
        public required TimeSpan time { get; set; } 
        public required float nitrogenDioxide { get; set; } 
        public required float sulfurDioxide { get; set; } 
        public required float pmTwoPointFive { get; set; } 
        public required float pmTen { get; set; }

        public AirDataModel()
        {
            db = new DatabaseService();
        }
           
    }
}
