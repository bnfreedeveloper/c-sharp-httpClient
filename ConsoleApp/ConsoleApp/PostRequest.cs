using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class PostRequest
    {
        public async Task<int> PostAsync()
        {
            var url = "https://localhost:5001/api/people";
            int id = 0;
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            using (var httpCLient = new HttpClient())
            {
                var newPerson = new Person()
                {
                    Name = "kendrick",
                    Email="testMeantTofail"
                };

                //we need to specify to the string content constructor the encoding and mediatype
                //the mediatype can be application/json or application/xml for ex
                var content = new StringContent(JsonSerializer.Serialize(newPerson),Encoding.UTF8, "application/json");

               var response = await httpCLient.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    id = Convert.ToInt32(await response.Content.ReadAsStringAsync());
                    Console.WriteLine($"id of the new inserted person : {id}");
                }
                if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorsFromApi = Utils.GetErrosFromApiResponse(await response.Content.ReadAsStringAsync());
                    foreach(var error in errorsFromApi)
                    {
                        Console.WriteLine(error.Key);
                        foreach(var errorValue in error.Value)
                        {
                            Console.WriteLine(errorValue);
                        }
                    }
                }
                var people = JsonSerializer.Deserialize<List<Person>>(await httpCLient.GetStringAsync(url), jsonSerializerOptions);
                foreach (var p in people) Console.WriteLine(p.Name);
                

             }
            return id;
        }
    }
}
