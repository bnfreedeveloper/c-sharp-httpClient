using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DatabaseContext _dbContext;
        public PeopleController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet]   
        public async Task<ActionResult<List<Person>>> GetPeople()
        {
            //await _dbContext.Persons.AddRangeAsync(new Person[]
            //{
            //    new Person() { Name ="jean valjeant"},
            //    new Person() { Name ="dexter"}
            //});
            //await _dbContext.SaveChangesAsync();
            return await _dbContext.Persons.ToListAsync();  
        }
        [HttpPost]  
        public async Task<ActionResult<int>> PostPeople(Person person)
        {
            await _dbContext.AddAsync(person);
            await _dbContext.SaveChangesAsync();
            return person.Id;
        }
    }
}
