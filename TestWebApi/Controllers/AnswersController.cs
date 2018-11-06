using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TestWebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        static HttpClient client = new HttpClient();
        
        // GET api/values
        [HttpGet]
        [ActionName("user")]
        public ActionResult<User> Get()
        {
            return new User { name = "Vimal Patel", token = "b45a9b26-58ca-4107-abdb-1b9439d97ce3" };
        }


        [HttpGet]
        [ActionName("sort")]
        public async Task<ActionResult<List<Product>>> Sort(string sortOption)
        {
            var sortedProds = new List<Product>();
            HttpResponseMessage response = new HttpResponseMessage();
            var token = "";
            response = await client.GetAsync("http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/products/" + token);
            if (response.IsSuccessStatusCode)
            {
                sortedProds = await response.Content.ReadAsAsync<List<Product>>();
            }
            else {
                return NotFound(MessageForStatusCode((int)response.StatusCode));
            }
        
            if (sortedProds.Count > 0)
            {
                switch (sortOption)
                {
                    case "Low":
                        sortedProds = sortedProds.OrderBy(p => p.Price).ToList();
                        break;

                    case "High":
                        sortedProds = sortedProds.OrderByDescending(p => p.Price).ToList();
                        break;
                    case "Ascending":
                        sortedProds = sortedProds.OrderBy(p => p.Name).ToList();
                        break;

                    case "Descending":
                        sortedProds = sortedProds.OrderByDescending(p => p.Name).ToList();
                        break;

                    case "Recommended":
                        var ShopperHistory = new List<ShopperHistory>();

                        response = await client.GetAsync("http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/shopperHistory/" + token);
                        if (response.IsSuccessStatusCode)
                        {
                            ShopperHistory = await response.Content.ReadAsAsync<List<ShopperHistory>>();
                        }

                        sortedProds = ShopperHistory.SelectMany(p => p.Products).OrderByDescending(p => p.Quantity).ThenByDescending(p => p.Quantity).ThenBy(p => p.Name).ToList();
                        break;
                }
            }
            return sortedProds;
        }

        [HttpPost]
        [ActionName("trolleyCalculator")]
        public async Task<ActionResult<int>> Post() {
            var trolley = new Trolley();
            HttpResponseMessage response = new HttpResponseMessage();
            int lowestTotal = 0;
            var token = "";
            response = await client.GetAsync("http://dev-wooliesx-recruitment.azurewebsites.net/api/resource/trolleyCalculator/" + token);
            if (response.IsSuccessStatusCode)
            {
                trolley = await response.Content.ReadAsAsync<Trolley>();
            }
            else
            {
                return NotFound(MessageForStatusCode((int)response.StatusCode));
            }

            if (trolley.Specials.Count() > 0)
            {
                lowestTotal = trolley.Specials.Min(p => p.Total);
            }
            return lowestTotal;
        }

        private static string MessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                    return "Resource not found";
                case 500:
                    return "Something went wrong";
                default:
                    return null;
            }
        }


    }
}
