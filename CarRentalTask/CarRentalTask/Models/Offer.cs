using System;
namespace CarRentalTask.Models
{
    public class Offer
    {
        public Price? price { get; set; }
        public Root? root { get; set; }
        public Vehicle? vehicle { get; set; }
        public Vendor? vendor { get; set; }
    }

}

