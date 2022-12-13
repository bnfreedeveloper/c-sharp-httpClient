using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebApi.Data;

namespace ConsoleApp
{
    public class UserManagement
    {
        private string _baseUrl = "https://localhost:5001/api/user";
        public async Task RegisterUser(User user)
        {
            using(var http = new HttpClient())
            {
              var body = new StringContent(JsonSerializer.Serialize<User>(user),Encoding.UTF8,"application/json");
              var response = await http.PostAsync(_baseUrl +"/register",body);
                
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        Console.Write(result);
                    }
                    else Console.WriteLine("something went wrong during registration!");
                
            }
        }
        public async Task<string> LoginUser(UserLogin user)
        {
            using (var http = new HttpClient())
            {
                var body = new StringContent(JsonSerializer.Serialize<UserLogin>(user), Encoding.UTF8, "application/json");
                var response = await http.PostAsync(_baseUrl+"/login", body);
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("login failed");
                    return "error";
                }
            }
        }
    }
}
