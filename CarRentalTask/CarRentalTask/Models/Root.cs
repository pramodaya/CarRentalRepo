using System;
using System.Numerics;

namespace CarRentalTask.Models
{
    public class Root
    {
        public string offerUId { get; set; }
        public Vehicle? vehicle { get; set; }
        public Price? price { get; set; }
        public Vendor? vendor { get; set; }
    }

}

