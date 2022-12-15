using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp.HttpClients
{
    public class PeopleHttpClient : IPeopleHttpClient
    {
        private readonly HttpClient _httpClient;
        private const string _url = "https://localhost:5001/api/people";

        //here is a solution when we don't add a typed client
        //public PeopleHttpClient(IHttpClientFactory httpFactory)
        //{
        //    _httpClient = httpFactory.CreateClient(); ;
        //}
        public PeopleHttpClient(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<List<Person>> getPeopleAsync()
        {
            var responsePeople = await _httpClient.GetAsync(_url);
            return JsonSerializer.Deserialize<List<Person>>(await responsePeople.Content.ReadAsStringAsync(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
