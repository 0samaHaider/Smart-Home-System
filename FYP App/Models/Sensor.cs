using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_App.Models
{
    public class Sensor
    {
        //public string ID { get; set; }
        public int Humidity {get; set; }
        public string Smoke { get; set; }
        public string Temperature { get; set; }
        public string Light { get; set; }
        public string Motion { get; set; }
        public string MoistureAvailable { get; set; }
        public string HumidityOutput { get; set; }
        public string LightOUTPUT { get; set; }
        public string SmokeOUTPUT { get; set; }
        public string TempOutput { get; set; }
        public string MoisOUTPUT { get; set; }
        public string MotionOutput { get; set; }
        public string WaterAvailable { get; set; }

        public string WaterOUTPUT { get; set; }
        public string Body_Detected { get; set; }
        public string Door { get; set; }



    }
}