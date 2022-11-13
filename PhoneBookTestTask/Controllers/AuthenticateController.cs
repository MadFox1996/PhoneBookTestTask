using Data;
using Data.Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneBookTestTask.Model;

namespace PhoneBookTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IRepository _db;
        private readonly IHashingHelper _hashingHelper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public AuthenticateController(IRepository db, IHashingHelper hashingHelper, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _db = db;
            _hashingHelper = hashingHelper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserDto user)
        {
            _hashingHelper.CreatePasswordHash(user.Password, out byte[] hash, out byte[] salt);
            var result = _db.AddUser(user.ToModel(hash, salt));

            if (result == null)
            {
                return BadRequest("Login is already exists");
            }

            return Ok($"User {result.Login} registered.");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserDto request)
        {
            var user = _db.GetUser(request.Login);

            if (user == null)
            {
                return BadRequest("Login or password is incorrect");
            }

            var verify = _hashingHelper.VerifyPassword(request.Password, user.Hash, user.Salt);

            if (!verify)
            {
                return BadRequest("Login or password is incorrect");
            }

            var token = _jwtAuthenticationManager.Authenticate(request.Login, request.Password);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(token);
        }
    }
}
