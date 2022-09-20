using System.Threading.Tasks;

namespace RequestingAPIs.Services
{
    public interface IHttpClientService
    {
        Task<string> CallApiAsync(RequestParam req);
    }
}