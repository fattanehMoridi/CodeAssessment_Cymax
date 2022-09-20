using Newtonsoft.Json.Linq;
using RequestingAPIs.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RequestingAPIs
{
    public partial class Program
    {
        public static async Task Main(string[] args)
        {
            InputRequest request = new InputRequest ("ContactAddress", "WarehouseAddress", new double[] { 1.0, 2.0, 3.0 });
            try
            {
                IHttpClientService httpClientService = new HttpClientService();
                var getOffer = new GetOffer(httpClientService, request);
                var taskA = getOffer.FirstOffer();
                var taskB = getOffer.SecondOffer();
                var taskC = getOffer.ThirdOffer();
                decimal[] results = await Task.WhenAll(taskA, taskB, taskC);
                decimal min = results[0];
                int offer = 1;
                for (int i = 1; i < 3; i++)
                {
                    decimal price = results[i];
                    if (price < min)
                    {
                        min = price;
                        offer = i + 1;
                    }
                }
                Console.WriteLine($"Lowest offer is {offer} offer with price of ${min}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }

        }
      
        
    }
}
