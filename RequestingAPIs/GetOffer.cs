using Newtonsoft.Json.Linq;
using RequestingAPIs.Services;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RequestingAPIs
{
        public class GetOffer {
            public InputRequest RequestQuery;
            private IHttpClientService HttpClientService;
            public GetOffer(IHttpClientService httpClientService, InputRequest requestQuery) {
                HttpClientService = httpClientService;
                RequestQuery = requestQuery;
            }
            
            public async Task<decimal> FirstOffer()
            {
                RequestParam requestParam = new RequestParam
                {
                    Uri = "uri1",
                    Query = $@"{{'Contact Address':{RequestQuery.SurceAddress},
                            'Warehouse Address':{RequestQuery.DestinationAddress},
                            'Package Dimensions':[{RequestQuery.CartonDimensions[0]},{RequestQuery.CartonDimensions[1]},{RequestQuery.CartonDimensions[2]}]}}",
                    Type = "application/json",
                    UserName = "UserName1",
                    PassWord = "PassWord1"
                };
                var result = await HttpClientService.CallApiAsync(requestParam);
                var obj = JObject.Parse(result);
                return obj["total"].Value<decimal>();
            }
            public  async Task<decimal> SecondOffer()
            {
                RequestParam requestParam = new RequestParam
                {
                    Uri = "uri2",
                    Query = $"{{'consignee':{RequestQuery.SurceAddress},'consignor':{RequestQuery.DestinationAddress},'cartons':[{RequestQuery.CartonDimensions[0]},{RequestQuery.CartonDimensions[1]},{RequestQuery.CartonDimensions[2]}]}}",
                    Type = "application/json",
                    UserName = "UserName2",
                    PassWord = "PassWord2"
                };
                var result = await HttpClientService.CallApiAsync(requestParam);
                var obj = JObject.Parse(result);
                return obj["amount"].Value<decimal>();
            }
            public  async Task<decimal> ThirdOffer()
            {
                RequestParam requestParam = new RequestParam
                {
                    Uri = "uri3",
                    Query = $@"<xml>
                            < source > {RequestQuery.SurceAddress} </ source >
                            < destination > {RequestQuery.DestinationAddress} </ destination >
                            < packages >
                                < package > {RequestQuery.CartonDimensions[0]} </ package >
                                < package > {RequestQuery.CartonDimensions[1]} </ package >
                                < package > {RequestQuery.CartonDimensions[2]} </ package >
                            </ packages >
                            </ xml > ",
                    Type = "text / xml",
                    UserName = "UserName3",
                    PassWord = "PassWord3"
                };
                var result = await HttpClientService.CallApiAsync(requestParam);
                var doc = XDocument.Parse(result);
                return decimal.Parse(doc.Descendants("quote").First().Value);
            }

        public async Task<string> LowestOffer() {
            var taskA = FirstOffer();
            var taskB = SecondOffer();
            var taskC = ThirdOffer();
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
            return $"Lowest offer is {offer} offer with price of ${min}";

        }


        }
        
}
