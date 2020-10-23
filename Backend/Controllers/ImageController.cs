using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ImageController : Controller
    {
        private readonly IImageService _service;

        public ImageController(IImageService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<List<ImageModel>> GetImages()
        {
            return Ok(_service.GetImages());
        }

        [HttpGet("{id}")]
        public ActionResult<ImageModel> GetImage(string id)
        {
            var img = _service.GetImage(id);

            if (img.Id == null)
            {
                return NotFound("Image does not exist");
            }

            else
            {
                return Ok(img);
            }
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult<ImageModel> UploadImage([FromForm]UploadModel img)
        {
            if (img == null)
                return BadRequest();

            var newImage = _service.UploadImage(img).Result;

            if (newImage.Id == null)
            {
                return BadRequest("Bad file type(must be valid image format)");
            }
            if (newImage.Caption == null)
            {
                return BadRequest("Caption is null");
            }
            if (newImage.Path == null)
            {
                return BadRequest("Image path is null");
            }

            return Created($"image/{newImage.Id}", newImage);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}")]
        public ActionResult<string> UpdateImage(string id, [FromBody]ImageModel model)
        {
            var imageEntity = _service.GetImage(id);
            if (imageEntity.Id == null)
            {
                return NotFound("Image does not exist");
            }

            _service.UpdateImage(id, model);
            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteImage(string id)
        {
            var imageEntity = _service.GetImage(id);
            if (imageEntity == null)
            {
                return NotFound();
            }

            _service.DeleteImage(id);
            return NoContent();
        }
    }
}
