using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ManagePutAndDeleteRequest
    {
        private readonly HttpClient _http;
        private string _url = "https://localhost:5001/api/people/";
        //i inject the http object with the authorization header defined
        //coz the route for put and delete are protected
        public ManagePutAndDeleteRequest(HttpClient http)
        {
            _http = http;
        }

        public async Task UpdateAsync(Person person)
        {
            var body = new StringContent(JsonSerializer.Serialize<Person>(person), Encoding.UTF8, "application/json");
            var response = await _http.PutAsync(_url + $"{person.Id}", body);
            if(response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("the person was updated!");
            }
            else
            {
                Console.WriteLine("couldn't update the person");
            }
            
        }
        public async Task DeleteAsync(Person person)
        {
            var response = await _http.DeleteAsync(_url +$"{ person.Id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine("the person was deleted!");
            }
            else
            {
                Console.WriteLine("couldn't delete the person");
            }
        }
    }
}
