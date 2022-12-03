using System;
namespace CarRentalTask.Models
{
	public class ReservationRequest
	{
        public string offerUId { get; set; }
        public Customer customer { get; set; }
    }
}

