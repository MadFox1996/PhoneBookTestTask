using Data;
using Data.EntityModel;
using Data.Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhoneBookTestTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneBookController : ControllerBase
    {
        private readonly IRepository _db;
        private readonly IHashingHelper _hashingHelper;
        private readonly IJwtAuthenticationManager _jwtAuthenticationManager;

        public PhoneBookController(IRepository db, IHashingHelper hashingHelper, IJwtAuthenticationManager jwtAuthenticationManager)
        {
            _db = db;
            _hashingHelper = hashingHelper;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Get([FromQuery] PhoneBookEntryPagination phoneBookEntryParameters)
        {
            if (phoneBookEntryParameters.PageNumber < 0 || phoneBookEntryParameters.PageSize <= 0)
            {
                return BadRequest("Pagination Error");
            }
            return Ok(_db.GetPhoneBookEntries(phoneBookEntryParameters));
        }

        [Authorize]
        [HttpGet("Image")]
        public IActionResult Image([FromQuery] string userName)
        {
            byte[]? image = _db.GetUserImage(userName);
            
            if(image == null)
            {
                return NotFound();
            }

            return File(image, "image/jpeg");
        }      
    }
}
