using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using CarRentalTask.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarRentalTask.Controllers
{
    public class LocationsController : Controller
    {
        HttpClientHandler _clientHandler = new HttpClientHandler();

        Location _location = new Location();
        List<Location> _locations = new List<Location>();



        public LocationsController()
        {
            _clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            Task<List<Location>> task = GetAllLocations();
            return View();
        }


        [HttpGet]
        public async Task<List<Location>> GetAllLocations()
        {
            _locations = new List<Location>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                using (var respnse = await httpClient.GetAsync("https://booking-test.dev-dch.com/api/v1/Locations/Locations"))
                {

                    string apiResponse = await respnse.Content.ReadAsStringAsync();
                    _locations = JsonConvert.DeserializeObject<List<Location>>(apiResponse);

                }

            }

            return _locations;

        }

        [HttpPost]
        public async Task<List<Root>> GetOffers(int locationId)
        {
            var url = "https://booking-test.dev-dch.com/api/v1/Availability/GetOffers";
            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            List<Root> _offer = new List<Root>();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                OfferReq ofr = new OfferReq()
                {
                    locationId = locationId
                };
                var response = await httpClient.PostAsJsonAsync(url, ofr);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    _offer = JsonConvert.DeserializeObject<List<Root>>(jsonString);
                    Console.WriteLine(_offer);
                }
                else
                {
                    Console.WriteLine("Error:" + response.StatusCode);
                }

            }

            List<Root> _resultArr = new List<Root>();
            _offer.ForEach(item => {
                Root r = new Root()
                {
                    offerUId = item.offerUId.ToString(),
                    vehicle = item.vehicle,
                    price = item.price,
                    vendor = item.vendor
                };
                _resultArr.Add(r);
            });
            return _resultArr;
        }

        [HttpPost]
        public async Task<ReservationResponse> CreateReservation(string offerUId, string name, string surname)
        {
            var url = "https://booking-test.dev-dch.com/api/v1/Reservations/CreateReservation";
            var jsonSerializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            ReservationResponse rp = new ReservationResponse();

            using (var httpClient = new HttpClient(_clientHandler))
            {
                Customer cus = new Customer()
                {
                    name = name,
                    surname = surname
                };
                ReservationRequest req = new ReservationRequest()
                {
                    offerUId = offerUId.ToString(),
                    customer = cus
                };
                var response = await httpClient.PostAsJsonAsync(url, req);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    rp = JsonConvert.DeserializeObject<ReservationResponse>(jsonString);
                    Console.WriteLine(rp);
                }
                else
                {
                    Console.WriteLine("Error: " + response.StatusCode);
                }

            }
            return rp;
        }

    }
}

