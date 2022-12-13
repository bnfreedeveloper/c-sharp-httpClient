using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi;

namespace ConsoleApp
{
    public class GetRequestWithHeadercs
    {
        private string _url = "https://localhost:5001/weatherForecast";

        public async Task getRequestWithSpecificHeader()
        {
            using (var httpClient = new HttpClient())
            {
                //with httprequest message we will send http header only once, for this request only
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, _url))
                {
                    requestMessage.Headers.Add("weatherAmount", "10");
                    var response = await httpClient.SendAsync(requestMessage);
                    var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    Console.WriteLine($"the amout of data returned is {weatherForecast.Count}");
                    Console.ReadLine();
                }

                //here because we didnt any general header for other request, the amout return will be the one by default
                var weatherForecastBis = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(_url), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"amount of data returned is {weatherForecastBis.Count}");
                Console.ReadLine();
            }
        }
        public async Task getRequestWithGeneralHeader()
        {
            using (var httpClient = new HttpClient())
            {
                //exemple for modifying the header for all request
                httpClient.DefaultRequestHeaders.Add("weatherAmount", "20");
                var weatherForecastBis2 = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(_url), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"amount of data returned is {weatherForecastBis2.Count}");
                Console.ReadLine();
                //here as we defined the default header all amount of data returned will be the same of each
                // new request
                var weatherForecastBis3 = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(_url), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"amount of data returned is {weatherForecastBis3.Count}");
                Console.ReadLine();
            }
        }
    }
}
