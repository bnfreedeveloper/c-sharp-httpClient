using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi;

namespace ConsoleApp
{
    public class GetRequest
    {
        private string _url = "https://localhost:5001/api/people";
        public async Task GetAsync()
        {

            using (var httpClient = new HttpClient())
            {
                //getasync implement also idisposable so better to go for using 
                using (var response = await httpClient.GetAsync(_url))
                {
                    Console.WriteLine(response);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        //here is the stringified representation
                        Console.WriteLine(responseContent);
                        //if we want the specific type
                        var people = JsonSerializer.Deserialize<List<Person>>(responseContent,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                        Console.ReadLine();
                    }
                }

            }
        }

        public async Task GetStringAsync()
        {
            using (var http = new HttpClient())
            {
                //if we don't care about http status code we can use this other method
                var responseWithNoStatusCode = await http.GetStringAsync(_url);
                var persons = JsonSerializer.Deserialize<List<Person>>(responseWithNoStatusCode,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var person in persons)
                {
                    Console.WriteLine(person.Name);
                }

                await new PostRequest().PostAsync();



                Console.ReadLine();

            }
        
        }
        public async Task GetRequestWithHeaderAuth(HttpClient httpClient)
        {
            var url = "https://localhost:5001/weatherForecast";
            var responseWithNoStatusCode = await httpClient.GetStringAsync(url);
            var weathers = JsonSerializer.Deserialize<List<WeatherForecast>>(responseWithNoStatusCode,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            foreach (var weather in weathers)
            {
                Console.WriteLine(weather);
            }

           // await new PostRequest().PostAsync();
            Console.ReadLine();
        }
    }
}
