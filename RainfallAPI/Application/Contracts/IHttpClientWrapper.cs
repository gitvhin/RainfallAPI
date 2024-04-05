using System.Net.Http;
using System.Threading.Tasks;

namespace RainfallAPI.Application.Contracts
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> GetAsync(string requestUri);
    }
}
