using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ScriveAPI.Helpers;
using ScriveAPI.Models;
using ScriveAPI.Services;

namespace ScriveAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            try
            {
                var registeredUser = await _userServices.Register(user.Username, user.Email, user.Password, user.ProfilePicture);
                return Ok(registeredUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var response = await _userServices.Login(request.Email, request.Password);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("user")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                // Get user ID from request header (assuming "x-auth-user-id")
                var userId = Request.Headers["x-auth-token"].FirstOrDefault();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Missing user ID in request header");
                }

                var user = await _userServices.GetUser(userId);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("user")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            try
            {
                // Get user ID from request header (assuming "x-auth-user-id")
                var userId = Request.Headers["x-auth-token"].FirstOrDefault();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Invalid user ID");
                }

                var updatedUser = await _userServices.UpdateUser(userId, request.Username, request.Email);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete]
        [Route("user")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var userId = Request.Headers["x-auth-token"].FirstOrDefault();

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("Missing user ID in request header");
                }

                var user = await _userServices.DeleteUser(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
