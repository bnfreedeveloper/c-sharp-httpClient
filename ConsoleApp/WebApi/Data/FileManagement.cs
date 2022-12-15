using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebApi.Data
{
    public class FileManagement
    {
        private readonly IWebHostEnvironment _env;
        public FileManagement(IWebHostEnvironment env)
        {
            _env = env;   
        }
        public async Task SaveAsync(IFormFile file,string nameFolder)
        {
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string routeFolder = Path.Combine(_env.WebRootPath, nameFolder);
            if (!Directory.Exists(routeFolder))
            {
                Directory.CreateDirectory(routeFolder); 
            }
            string routerForFileToSave = Path.Combine(routeFolder, fileName);
            using (FileStream fs = new FileStream(routerForFileToSave, FileMode.Create))
            {
                await file.CopyToAsync(fs); 
            }  
        }
    }
}
