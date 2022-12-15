using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Data;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileManagement _fileManagement;
        public FilesController(FileManagement fileManagement)
        {
            _fileManagement = fileManagement;
        }
        [HttpPost]
        public async Task<IActionResult> SaveAsync([FromForm] IFormFile file,[FromHeader] string folderCategory)
        {
            await _fileManagement.SaveAsync(file, folderCategory);
            return Ok();
        }
    }
}
