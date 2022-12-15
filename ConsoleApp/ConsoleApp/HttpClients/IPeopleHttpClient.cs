using Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApp.HttpClients
{
    public interface IPeopleHttpClient
    {
        Task<List<Person>> getPeopleAsync();
    }
}