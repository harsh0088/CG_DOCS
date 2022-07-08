using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CgDocs.Models;
using CgDocs.RequestModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cgDocs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FolderController : ControllerBase
    {
        private readonly CgDocsContext _CgDocsContext;
        public FolderController(CgDocsContext Folderinfo)
        {
            _CgDocsContext = Folderinfo;
        }


        // GET: api/Folder
        [HttpGet]
        public IActionResult Get()
        {
            var getFolder = _CgDocsContext.Folders.ToList();
            return Ok(getFolder);
        }


        //Get folder by id
        [HttpGet("{id:int}")]
        public IActionResult showAllFilesInDashboard(int id)
        {
            try
            {
                var response = _CgDocsContext.Folders.Where(obj => obj.FolderCreatedBy == id && obj.FolderIsDeleted == false);
                if (response == null)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
        }


        //Show in favourite api
        [HttpGet("ShowInFav/{id:int}")]
        public IActionResult showInFav(int id)
        {
            try
            {
                var response = _CgDocsContext.Folders.Where(obj => obj.FolderCreatedBy == id && obj.FolderIsDeleted == false && obj.FolderIsFavourite == true);
                if (response == null)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
        }


        //Show folder in Trash api
        [HttpGet("ShowTrash/{id:int}")]
        public IActionResult GetForTrash(int id)
        {
            try
            {
                var response = _CgDocsContext.Folders.Where(obj => obj.FolderCreatedBy == id && obj.FolderIsDeleted == true);
                if (response == null)
                    return NotFound();
                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error!");
            }
        }


        //Create a new folder
        [HttpPost]
        public void Post([FromBody] FolderRequest value)
        {
            Folders obj = new Folders();
            obj.FolderName = value.FolderName;
            obj.FolderCreatedBy = value.FolderCreatedBy;
            obj.FolderCreatedAt = DateTime.Now;
            obj.FolderIsDeleted = value.FolderIsDeleted;
            _CgDocsContext.Folders.Add(obj);
            _CgDocsContext.SaveChanges();
        }

        //for searching a folder on dashboard page
        [HttpGet("{id}/{value}")]
        public IActionResult Search(int id, string value)
        {
            var result = _CgDocsContext.Folders.Where(e => e.FolderCreatedBy == id).Where(o => o.FolderName.Contains(value) && o.FolderIsDeleted == false);
            return Ok(result);
        }

        //for searching a folder in trash page
        [HttpGet("SearchInTrash/{id}/{value}")]
        public IActionResult SearchInTrash(int id, string value)
        {
            var result = _CgDocsContext.Folders.Where(e => e.FolderCreatedBy == id).Where(o => o.FolderName.Contains(value) && o.FolderIsDeleted == true);
            return Ok(result);
        }



        //Soft Delete a Folder
        [HttpPut("SoftDelete/{id}")]
        public IActionResult SoftDelete(int id)
        {
            int n = 0;
            try
            {
                //soft deleting files within folder
                var res = _CgDocsContext.Documents.Where(obj => obj.FolderId == id).ToList();
                foreach (var x in res)
                {
                    x.DocumentIsDeleted = true;
                    _CgDocsContext.Documents.Update(x);
                    _CgDocsContext.SaveChanges();
                }

                //soft deleting folder
                var newObj = _CgDocsContext.Folders.First(obj => obj.FolderId == id);
                newObj.FolderIsDeleted = true;
                _CgDocsContext.Folders.Update(newObj);
                _CgDocsContext.SaveChanges();

                n = 200;//indicating success
            }

            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }



        //Favouriting a folder api
        [HttpPut("Favourite/{id}")]
        public IActionResult Favourite(int id)
        {
            int n = 0;
            try
            {

                var newObj = _CgDocsContext.Folders.First(obj => obj.FolderId == id);
                newObj.FolderIsFavourite = true;
                _CgDocsContext.Folders.Update(newObj);
                _CgDocsContext.SaveChanges();

                n = 200;//indicating success
            }

            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }



        //Unfavouriting a folder api
        [HttpPut("Unfavourite/{id}")]
        public IActionResult Unfavourite(int id)
        {
            int n = 0;
            try
            {
                var newObj = _CgDocsContext.Folders.First(obj => obj.FolderId == id);
                newObj.FolderIsFavourite = false;
                _CgDocsContext.Folders.Update(newObj);
                _CgDocsContext.SaveChanges();

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
                var res = _CgDocsContext.Documents.Where(o => o.FolderId == id).ToList();
                foreach (var x in res)
                {
                    x.DocumentIsDeleted = false;
                    _CgDocsContext.Documents.Update(x);
                    _CgDocsContext.SaveChanges();
                }

                var newObj = _CgDocsContext.Folders.First(obj => obj.FolderId == id);
                newObj.FolderIsDeleted = false;
                _CgDocsContext.Folders.Update(newObj);
                _CgDocsContext.SaveChanges();
                n = 200;
            }
            catch (Exception e)
            {
                n = 404;
            }

            return StatusCode(n);
        }


        //Hard Delete a folder
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var delete = _CgDocsContext.Documents.Where(res => res.FolderId == id).ToList();
            delete.ForEach(res => _CgDocsContext.Documents.Remove(res));
            var del = _CgDocsContext.Folders.Where(res => res.FolderId == id).ToList();
            del.ForEach(res => _CgDocsContext.Folders.Remove(res));
            _CgDocsContext.SaveChanges();
        }


        //Show recent folders
        [HttpGet("ShowRecentFolders/{id}/{time}")]
        public IActionResult ShowRecent(int id, int time)
        {
            int m = 0;
            try
            {

                if (time == 6)
                {
                    var createdAt = DateTime.Now.AddHours(-6);

                    var res = _CgDocsContext.Folders.Where(o => o.FolderCreatedAt >= createdAt && o.FolderCreatedBy == id && o.FolderIsDeleted == false);
                    return Ok(res);
                }
                else if (time == 1)
                {
                    var createdAt = DateTime.Now.AddHours(-1);

                    var res = _CgDocsContext.Folders.Where(o => o.FolderCreatedAt >= createdAt && o.FolderCreatedBy == id && o.FolderIsDeleted == false);
                    return Ok(res);
                }
                else if (time == 30)
                {
                    var createdAt = DateTime.Now.AddMinutes(-30);

                    var res = _CgDocsContext.Folders.Where(o => o.FolderCreatedAt >= createdAt && o.FolderCreatedBy == id && o.FolderIsDeleted == false);
                    return Ok(res);
                }
                else if (time == 12)
                {
                    var createdAt = DateTime.Now.AddHours(-12);

                    var res = _CgDocsContext.Folders.Where(o => o.FolderCreatedAt >= createdAt && o.FolderCreatedBy == id && o.FolderIsDeleted == false);
                    return Ok(res);
                }
                else
                {
                    var createdAt = DateTime.Now.AddHours(-24);

                    var res = _CgDocsContext.Folders.Where(o => o.FolderCreatedAt >= createdAt && o.FolderCreatedBy == id && o.FolderIsDeleted == false);
                    return Ok(res);
                }

            }
            catch (Exception e)
            {
                m = 404;
                return StatusCode(m);
            }

        }
    }
}


