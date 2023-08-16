using InfluxDB.Client.Core;
using System;

namespace influxdb_client_csharp
{
    [Measurement("Book2")]
    class Book2
    {
        [Column("Serie", IsTag = true)]
        public string Serie { get; set; }


        [Column("Name")]
        public string Name { get; set; }


        [Column("Price")]
        public double? Price { get; set; }


        [Column("SpecialOffer")]
        public bool? SpecialOffer { get; set; }


        [Column("CreateTime", IsTimestamp = true)]
        public DateTime CreateTime { get; set; }
    }
}
