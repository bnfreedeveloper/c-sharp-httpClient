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
            await userManagement.RegisterUser(new User
            {
                UserName = "jeanValjeant",
                Email = "test@gmail.com",
                Password = "test123",
                ConfirmedPassword = "test123"
            });
            var token = await userManagement.LoginUser(new UserLogin
                {
                    UserName = "jeanValjeant",
                    Password = "test123"
                });
                Console.WriteLine(token);
               using (var httpClient = new HttpClient())
               {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
                await new GetRequest().GetRequestWithHeaderAuth(httpClient);

                //WE ADD ONE USER
               var personId = await new PostRequest().PostAsync(new Person
               {
                   Name="aragorn",
                   Age=42,
                   Email="conte@gmail.com"
               });
                await new PostRequest().PostAsync(new Person
                {
                    Name = "legolas",
                    Age = 36,
                    Email = "arrow@gmail.com"
                });
                var person = new Person
                {
                    Id = personId,
                    Name = "testUpdate",
                    Age = 25,
                    Email = "testUpdate@gmail.com"
                };
                //we gonna load the people using a generic function
                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);    
                await new ManagePutAndDeleteRequest(httpClient).UpdateAsync(person);
                //we check if user was updated
                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);

                // we gonna delete the user
                await new ManagePutAndDeleteRequest(httpClient).DeleteAsync(person);
                //we check if user was indeed deleted
                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);

            }
            Console.ReadLine();
        }
    }
}

