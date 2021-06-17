using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserSevice _userService;
        public UserController(UserSevice userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public ActionResult<List<UserDetail>> Get() =>
            _userService.Get();

        [HttpPost]
        public ActionResult<UserDetail> Create(UserDetail user)
        {
            _userService.Create(user);

            return CreatedAtRoute("GetUser", new { id = user.Id.ToString() }, user);
        }
        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<UserDetail> Get(ObjectId id)
        {
            var book = _userService.Get(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }
    }
}