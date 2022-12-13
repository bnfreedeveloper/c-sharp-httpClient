using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using WebApi;
using WebApi.Data;

namespace ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://localhost:5001/api/people";
            using (var httpClient = new HttpClient())
            {
                //getasync implement also idisposable so better to go for using 
                using (var response = await httpClient.GetAsync(url))
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
                
                //if we don't care about http status code we can use this other method
                var responseWithNoStatusCode = await httpClient.GetStringAsync(url);
                var persons = JsonSerializer.Deserialize<List<Person>>(responseWithNoStatusCode,
                            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach(var person in persons)
                {
                    Console.WriteLine(person.Name);
                }

               await new PostRequest().PostAsync();


               
                Console.ReadLine();

            }
            // we gonna send values through http header
            var urlWeather = "https://localhost:5001/weatherForecast";
            using ( var httpClient = new HttpClient())
            {
                //with httprequest message we will send http header only once, for this request only
                using (var requestMessage = new HttpRequestMessage(HttpMethod.Get,urlWeather))
                {
                    requestMessage.Headers.Add("weatherAmount", "10");
                    var response = await httpClient.SendAsync(requestMessage);
                    var weatherForecast = JsonSerializer.Deserialize<List<WeatherForecast>>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive=true});

                    Console.WriteLine($"the amout of data returned is {weatherForecast.Count}");
                    Console.ReadLine();
                }

                //here because we didnt any general header for other request, the amout return will be the one by default
                var weatherForecastBis = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(urlWeather), new JsonSerializerOptions { PropertyNameCaseInsensitive=true});
                Console.WriteLine($"amount of data returned is {weatherForecastBis.Count}");
                Console.ReadLine();


                //exemple for modifying the header for all request
                httpClient.DefaultRequestHeaders.Add("weatherAmount", "20");
                var weatherForecastBis2 = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(urlWeather), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"amount of data returned is {weatherForecastBis2.Count}");
                Console.ReadLine();
                //here as we defined the default header all amount of data returned will be the same of each
                // new request
                var weatherForecastBis3 = JsonSerializer.Deserialize<List<WeatherForecast>>(await httpClient.GetStringAsync(urlWeather), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                Console.WriteLine($"amount of data returned is {weatherForecastBis3.Count}");
                Console.ReadLine();

                var userManagement = new UserManagement();
                
                await userManagement.RegisterUser(new User
                {
                    UserName = "jeanvaljeant",
                    Email = "test@gmail.com",
                    Password = "test123",
                    ConfirmedPassword = "test123"
                });
                var token = await userManagement.LoginUser(new UserLogin
                {
                    UserName = "jeanvaljeant",
                    Password = "test123"
                });
                Console.WriteLine(token);
                Console.ReadLine();
            }
        }
    }
}
