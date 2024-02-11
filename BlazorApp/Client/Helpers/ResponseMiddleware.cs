using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;
using System;
using Newtonsoft.Json;
using BlazorApp.Client.Shared;

namespace BlazorApp.Client.Helpers
{
    public class ResponseMiddleware : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            // Проверяем статус код ответа
            if (!response.IsSuccessStatusCode)
            {
                var errorModel = JsonConvert.DeserializeObject<ErrorResponseModel>(await response.Content.ReadAsStringAsync());
                switch (errorModel.Code)
                {
                    case "404":
                    default:
                        break;
                }
            }

            return response;
        }
    }
}
