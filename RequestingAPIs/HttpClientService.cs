using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RequestingAPIs.Services
{
	public class Query
	{
		public string ContactAddress;
		public string warehouseAddress;
		public double[] CartonDimensionss = new double[3];
	}
	public class RequestParam
	{
		public string Uri;
		public string Query;
		public string Type;
		public string UserName;
		public string PassWord;
	}
    public class HttpClientService : IHttpClientService
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public HttpClientService()
        {
            _httpClient.Timeout = new TimeSpan(0, 0, 30);
            _httpClient.DefaultRequestHeaders.Clear();

        }
        public async Task<string> CallApiAsync(RequestParam req)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, req.Uri);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(req.Type));
            request.Content = new StringContent(req.Query, Encoding.UTF8);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue(req.Type);
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(String.Format("{0}:{1}", req.UserName, req.PassWord))));
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();

        }
       
    }
}