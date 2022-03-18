using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SOTA.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SOTA.Controllers
{
    public class DownloadController : Controller
    {

        [HttpGet]
        public async Task<IActionResult> File(string path)
        { int cg=4;
            if (path == "NotData")
            {
                return RedirectToAction("ErroreData");
            }
            path = path.Replace("\"", "").Replace("\'", "").Replace("?", "").Replace("*", "").Replace("|", "")
                .Replace("<", "").Replace(">", "");
            var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }

                memory.Position = 0;
                var ext = Path.GetExtension(path).ToLowerInvariant();
                return File(memory, GetMimeTypes()[ext], Path.GetFileName(path));


        }

        public  IActionResult ErroreData()
        {ErrorViewModel err=new ErrorViewModel();
            err.RequestId="Нет данных по прошедшим эту работу ученикам";
            
            return View("Error", err);
        }


        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
        {
            {".txt", "text/plain"},
            {".pdf", "application/pdf"},
            {".doc", "application/vnd.ms-word"},
            {".docx", "application/vnd.ms-word"},
            {".xlsx", "application/vnd.ms-excel"},
            {".png", "image/png"},
            {".jpg", "image/jpeg"}
        };
        }
    }
}
