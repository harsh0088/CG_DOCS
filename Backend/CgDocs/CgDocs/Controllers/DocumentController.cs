using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CgDocs.Models;
using CgDocs.RequestModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace cgDocs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly CgDocsContext __cgDocsContext;
        private IHostingEnvironment _environment;
        public DocumentController(CgDocsContext documentInfo, IHostingEnvironment env)
        {
            __cgDocsContext = documentInfo;
            _environment = env;
        }

        // Get all files api
        [HttpGet]
        public IActionResult Get()
        {
            var getDocmument = __cgDocsContext.Documents.ToList();
            return Ok(getDocmument);
        }

        //Get files by id api
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                var response = __cgDocsContext.Documents.Where(obj => obj.FolderId == id);
                if (response == null)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
        }

        // Create a new file
        [HttpPost]
        public void Post([FromBody] DocumentRequest value)
        {
            Documents obj = new Documents();
            obj.DocumentName = value.DocumentName;
            obj.DocumentContentType = value.DocumentContentType;
            obj.DocumentSize = value.DocumentSize;
            obj.DocumentCreatedBy = value.DocumentCreatedBy;
            obj.DocumentCreatedAt = value.DocumentCreatedAt;
            obj.FolderId = value.FolderId;
            obj.DocumentIsDeleted = value.DocumentIsDeleted;
            __cgDocsContext.Documents.Add(obj);
            __cgDocsContext.SaveChanges();
        }

        //Get a file by id api
        [HttpGet("{id}/{value}")]
        public IActionResult Get(int id, string value)
        {
            var result = __cgDocsContext.Documents.Where(e => e.FolderId == id).Where(o => o.DocumentName.Contains(value));
            return Ok(result);
        }

        //Hard delete a file api
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var delete = __cgDocsContext.Documents.Where(res => res.DocumentId == id).ToList();
            delete.ForEach(res => __cgDocsContext.Documents.Remove(res));
            var del = __cgDocsContext.Documents.Where(res => res.DocumentId == id).ToList();
            del.ForEach(res => __cgDocsContext.Documents.Remove(res));
            __cgDocsContext.SaveChanges();
        }

        //Upload a new file inside a folder
        [HttpPost]
        [Route("upload/{folderId}/{createdBy}/{createdAt}")]
        public async Task<ActionResult> Upload(List<IFormFile> files, int folderId, int createdBy, DateTime createdAt)
        {
            long size = files.Sum(f => f.Length);


            var rootPath = Path.Combine(_environment.ContentRootPath, "Resources", "Documents");


            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            foreach (var file in files)
            {
                var filePath = Path.Combine(rootPath, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    var Documents = new Documents
                    {
                        DocumentName = file.FileName,
                        DocumentContentType = file.ContentType,
                        DocumentSize = (int)file.Length,
                        FolderId = folderId,
                        DocumentCreatedAt = DateTime.Now,
                        DocumentCreatedBy = createdBy,
                        DocumentIsDeleted = false
                    };

                    await file.CopyToAsync(stream);
                    __cgDocsContext.Documents.Add(Documents);
                    await __cgDocsContext.SaveChangesAsync();
                }
            }
            return Ok(new { count = files.Count, size });
        }

        private int? parseint(long length)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("download/{id}")]
        public async Task<ActionResult> Download(int id)
        {
            var provider = new FileExtensionContentTypeProvider();
            var document = __cgDocsContext.Documents.FirstOrDefault(o => o.DocumentId == id);

            if (document == null)
                return NotFound();

            var file = Path.Combine(_environment.ContentRootPath, "Resources", "Documents", document.DocumentName);
            string contentType;
            if (!provider.TryGetContentType(file, out contentType))
            {
                contentType = "application/octet-stream";
            }

            byte[] fileBytes;
            if (System.IO.File.Exists(file))
            {
                fileBytes = System.IO.File.ReadAllBytes(file);
            }
            else
            {
                return NotFound();
            }

            return File(fileBytes, contentType, document.DocumentName);
        }


        //Favourite
        [HttpPut("Favourite/{id}")]
        public IActionResult Favourite(int id)
        {
            int n = 0;
            try
            {

                var newObj = __cgDocsContext.Documents.First(obj => obj.FolderId == id);
                newObj.DocumentIsFavourite = true;
                __cgDocsContext.Documents.Update(newObj);
                __cgDocsContext.SaveChanges();

                n = 200;//indicating success
            }

            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }

        //Unfavourite
        [HttpPut("Unfavourite/{id}")]
        public IActionResult Unfavourite(int id)
        {
            int n = 0;
            try
            {
                var newObj = __cgDocsContext.Documents.First(obj => obj.FolderId == id);
                newObj.DocumentIsFavourite= false;
                __cgDocsContext.Documents.Update(newObj);
                __cgDocsContext.SaveChanges();

                n = 200;//indicating success
            }

            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }


        //Soft Delete a Folder
        [HttpPut("SoftDelete/{id}")]
        public IActionResult SoftDelete(int id)
        {
            int n = 0;
            try
            {
                
                var newObj = __cgDocsContext.Documents.First(obj => obj.DocumentId == id);
                newObj.DocumentIsDeleted = true;
                __cgDocsContext.Documents.Update(newObj);
                __cgDocsContext.SaveChanges();

                n = 200;//indicating success
            }

            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }

        // Undelete a folder
        [HttpPut("Undelete/{id}")]
        public IActionResult Undelete(int id)
        {
            int n = 0;
            try
            {
                var newObj = __cgDocsContext.Documents.First(obj => obj.FolderId == id);
                newObj.DocumentIsDeleted = false;
                __cgDocsContext.Documents.Update(newObj);
                __cgDocsContext.SaveChanges();
                n = 200;
            }
            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }

    }
}
