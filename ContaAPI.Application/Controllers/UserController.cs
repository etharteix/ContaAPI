using System;
using ContaAPI.Domain.Interfaces;
using ContaAPI.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContaAPI.Application.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IServiceUser _serviceUser;

        public UserController(IServiceUser serviceUser) =>
            _serviceUser = serviceUser;


        [HttpPost]
        public IActionResult Register([FromBody] CreateUserModel userModel)
        {
            try
            {
                var user = _serviceUser.Insert(userModel);

                //return Created($"/api/users/{user?.Id}", user?.Id);
                return Created($"/api/users/{user?.Id}", user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateUserModel userModel)
        {
            try
            {
                var user = _serviceUser.Update(id, userModel);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Remove([FromRoute] Guid id)
        {
            try
            {
                _serviceUser.Delete(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        public IActionResult RecoverAll()
        {
            try
            {
                var users = _serviceUser.RecoverAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Recover([FromRoute] Guid id)
        {
            try
            {
                var user = _serviceUser.RecoverById(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginUserModel userModel)
        {
            try
            {
                var user = _serviceUser.Login(userModel);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}