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
                var result = await getOffer.LowestOffer();
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }

        }
      
        
    }
}
