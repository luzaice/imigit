using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [ApiController]
    [Route("image/{imageId}/[controller]")]
    public class CommentController : Controller
    {
        private readonly ICommentService _service;
        private readonly IImageService _imgservice;

        public CommentController(ICommentService service, IImageService imgService)
        {
            _service = service;
            _imgservice = imgService;
        }

        [HttpGet]
        public ActionResult<List<CommentModel>> GetImageComments(string imageId)
        {
            return Ok(_service.GetComments(imageId));
        }

        [HttpGet("{id}", Name = "GetCommentById")]
        public ActionResult<CommentModel> GetComment(string imageId, string id)
        {
            var com = _service.GetComment(imageId, id);
            if (_imgservice.GetImage(imageId) == null)
                return NotFound("Image does not exist");
            if (com == null)
                return NotFound("Comment does not exist");

            return Ok(com);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public ActionResult<CommentModel> AddComment(string imageId, [FromBody]CommentModel comment)
        {
            if (comment == null)
                return BadRequest();

            if (_imgservice.GetImage(imageId).Id == null)
                return NotFound("Comment does not exist");

            _service.AddComment(imageId, comment);

            return CreatedAtRoute("GetCommentById", new { imageId, id = comment.Id }, comment);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{id}")]
        public ActionResult UpdateComment(string imageId, string id, [FromBody]CommentModel comment)
        {
            if (_imgservice.GetImage(imageId) == null)
                return NotFound("Image does not exist");

            if (_service.GetComment(imageId, id) == null)
            {
                _service.AddComment(imageId, comment);
                return CreatedAtRoute("GetBookById", new { imageId, id = comment.Id }, comment);
            }

            _service.UpdateComment(imageId, id, comment);
            return Ok();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public ActionResult DeleteComment(string imageId, string id)
        {
            if (_imgservice.GetImage(imageId).Id == null)
                return NotFound("Image does not exist");

            if (_service.GetComment(imageId, id) == null)
                return NotFound("Comment does not exist");

            _service.DeleteComment(imageId, id);
            return NoContent();
        }
    }
}
