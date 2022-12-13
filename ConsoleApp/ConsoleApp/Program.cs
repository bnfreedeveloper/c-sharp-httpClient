using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using WebApi;
using WebApi.Data;
using System.Net.Http.Headers;

namespace ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        { 

                var userManagement = new UserManagement();

            //already done, but uncomment it for the first time (oui tout cela c'est de moi)
            //await userManagement.RegisterUser(new User
            //{
            //    UserName = "jeanvaljeant",
            //    Email = "test@gmail.com",
            //    Password = "test123",
            //    ConfirmedPassword = "test123"
            //});
            var token = await userManagement.LoginUser(new UserLogin
                {
                    UserName = "jeanvaljeant",
                    Password = "test123"
                });
                Console.WriteLine(token);
               using (var httpClient = new HttpClient())
               {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                await new GetRequest().GetRequestWithHeaderAuth(httpClient);
               }
                Console.ReadLine();
        }
    }
}

