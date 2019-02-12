using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace FNEMS
{
    public static partial class Upload
    {
        public class EnergyReading : TableEntity
        {
            public EnergyReading()
            {
                this.PartitionKey = "Data";
                this.Timestamp = DateTime.Now;
            }

            public double Electricity { get; set; }
            public double Gas { get; set; }
            public DateTime ReadingDate { get; set; }
            public string Comment { get; set; }
            public string Office { get; set; }
        }
    }
}
