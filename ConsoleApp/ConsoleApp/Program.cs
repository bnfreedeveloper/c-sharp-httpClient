using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

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
        }
    }
}
