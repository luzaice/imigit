using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult<List<UserModel>> GetUsers()
        {
            return Ok(_service.GetUsers());
        }

        [HttpGet("{id}")]
        public ActionResult<UserModel> GetUser(string id)
        {
            var user = _service.GetUser(id);

            if (user.Id == null)
            {
                return NotFound("User does not exist");
            }

            else
            {
                return Ok(user);
            }
        }

        [Authorize(Roles = "Admin, Guest")]
        [HttpPost]
        public ActionResult<UserModel> AddUser([FromBody]UserModel user)
        {
            if (user == null)
                return BadRequest();
            _service.AddUser(user);
            return Created($"user/{user.Id}", user);
        }

        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public ActionResult<UserModel> UpdateUser(string id, [FromBody]UserModel user)
        {
            var userEntity = _service.GetUser(id);
            if (userEntity.Id == null)
            {
                return NotFound("User does not exist");
            }

            _service.UpdateUser(id, user);
            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{id}")]
        public ActionResult<string> DeleteUser(string id)
        {
            var userEntity = _service.GetUser(id);
            if (userEntity == null)
            {
                return NotFound("User does not exist");
            }

            _service.DeleteUser(id);
            return NoContent();
        }
    }
}
