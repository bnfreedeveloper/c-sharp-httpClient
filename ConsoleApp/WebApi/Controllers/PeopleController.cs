using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]/")]
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
        [HttpPut("{id:int}")]
        [Authorize(Policy="adminOnly")]
        public async Task<IActionResult> Put(int id, Person person)
        {
            if(await _dbContext.Persons.AnyAsync(p => p.Id == id))
            {
                _dbContext.Persons.Update(person);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();  
        }

        [HttpDelete("{id:int}")]
        [Authorize(Policy="adminOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _dbContext.Persons.SingleOrDefaultAsync(_ => _.Id == id);
            if ( person != null )
            {
                _dbContext.Persons.Remove(person);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            return NotFound();
        }
    }
}
