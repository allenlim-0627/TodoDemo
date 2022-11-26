using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoList.DTO;
using ToDoList.Repositories;

namespace ToDoList.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        public IUserRepository _userRepo;

        public AuthenticationController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("Login")]
        public IActionResult Token(UserDTO user)
        {

            if (_userRepo.GetToken(user))
                return Ok(user);

            return BadRequest(new { message = "Not valid UserName" });

        }
    }
}
