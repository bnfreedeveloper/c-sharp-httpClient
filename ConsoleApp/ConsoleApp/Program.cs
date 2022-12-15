using Common;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using WebApi;
using WebApi.Data;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using ConsoleApp.HttpClients;
using System.IO;

namespace ConsoleApp
{
    public class Program
    {
        static async Task Main(string[] args)
        { 

            //    var userManagement = new UserManagement();

            ////already done, but uncomment it for the first time (oui tout cela c'est de moi)
            //await userManagement.RegisterUser(new User
            //{
            //    UserName = "jeanValjeant",
            //    Email = "test@gmail.com",
            //    Password = "test123",
            //    ConfirmedPassword = "test123"
            //});
            //var token = await userManagement.LoginUser(new UserLogin
            //    {
            //        UserName = "jeanValjeant",
            //        Password = "test123"
            //    });
            //    Console.WriteLine(token);
            //   using (var httpClient = new HttpClient())
            //   {
            //                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            //                await new GetRequest().GetRequestWithHeaderAuth(httpClient);

            //                //WE ADD ONE USER
            //               var personId = await new PostRequest().PostAsync(new Person
            //               {
            //                   Name="aragorn",
            //                   Age=42,
            //                   Email="conte@gmail.com"
            //               });
            //                await new PostRequest().PostAsync(new Person
            //                {
            //                    Name = "legolas",
            //                    Age = 36,
            //                    Email = "arrow@gmail.com"
            //                });
            //                var person = new Person
            //                {
            //                    Id = personId,
            //                    Name = "testUpdate",
            //                    Age = 25,
            //                    Email = "testUpdate@gmail.com"
            //                };
            //                //we gonna load the people using a generic function
            //                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);    
            //                await new ManagePutAndDeleteRequest(httpClient).UpdateAsync(person);
            //                //we check if user was updated
            //                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);

            //                // we gonna delete the user
            //                await new ManagePutAndDeleteRequest(httpClient).DeleteAsync(person);
            //                //we check if user was indeed deleted
            //                await new GetRequest().GetRequestAuthHeader<Person>(httpClient);

            //   }


            //for better practice we gonna use the httpclientfactory
            var servicesCollection = new ServiceCollection();
            ConfigureServices(servicesCollection);
            var services = servicesCollection.BuildServiceProvider(); 
            var httpClientFactory = services.GetRequiredService<IHttpClientFactory>();

            //exemple 1 basic use case
            var httpClientFromFactory = httpClientFactory.CreateClient();
            var urlWeather = "https://localhost:5001/weatherForecast/";
            var response = await httpClientFromFactory.GetAsync(urlWeather);

            //this will throw exception if no success status code
            response.EnsureSuccessStatusCode();
            Console.WriteLine("success response");
            //exemple 2 Named clients
            var httpCLientPeople = httpClientFactory.CreateClient("people");
            
            var responsePeople = await httpCLientPeople.GetAsync("");
            responsePeople.EnsureSuccessStatusCode();

            var httpClientWeatherForecast = httpClientFactory.CreateClient("weatherForecast");
            var responseWeather = await httpClientWeatherForecast.GetAsync("");
            responseWeather.EnsureSuccessStatusCode();
            Console.WriteLine("success response for named clients");

            //exemple 3 typed clients
            var httpPeople = services.GetRequiredService<IPeopleHttpClient>();
            await httpPeople.getPeopleAsync();

            //if we dont add typed client, here is one solution
            //new PeopleHttpClient(services.GetRequiredService<IHttpClientFactory>());

            Console.WriteLine("success response for httpclientfactory typed client");

            //files Management
            var fileUrl = "https://localhost:5001/api/files/";
            var fileRoute = @"C:\Users\gutz9\Desktop\real\httpClientDepth\ConsoleApp\TestExempleToSaveViaApi.txt";
            var fileName = Path.GetFileName(fileRoute).Split(".")[0];
            using(var requestContent = new MultipartFormDataContent())
            {
                using (var filestream = File.OpenRead(fileRoute))
                {
                    //the variable named "file" must match the name of the one for IFormFile
                    requestContent.Add(new StreamContent(filestream),"file",fileName);
                    requestContent.Headers.Add("folderCategory", "files");
                    await services.GetRequiredService<IHttpClientFactory>()
                                  .CreateClient()
                                  .PostAsync(fileUrl, requestContent);
                }
            }
            

            Console.ReadLine();
        }
        private static void ConfigureServices(ServiceCollection services)
        {
            //basic use case
            services.AddHttpClient();

            //named clients
            services.AddHttpClient("people", options =>
             {
                 options.BaseAddress = new Uri("https://localhost:5001/api/people");
             });
            services.AddHttpClient("weatherForecast", options =>
             {
                 options.BaseAddress = new Uri("https://localhost:5001/weatherForecast/");
                 options.DefaultRequestHeaders.Add("weatherAmount", "10");
             });

            //typed client
            services.AddHttpClient<IPeopleHttpClient, PeopleHttpClient>();
        }
    }
}

