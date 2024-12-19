using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;

namespace TurnPoint.Jo.APIs.Controllers
{
    [Route("api/userinterests")]
    [ApiController]
    public class InterestsLookupUserController : ControllerBase
    {
        private readonly IInterestsLookupUserService _userInterestsService;

        public InterestsLookupUserController(IInterestsLookupUserService userInterestsService)
        {
            _userInterestsService = userInterestsService;
        }

        [AllowAnonymous]
        [HttpPost("AddInterestsToUser")]
        public async Task<IActionResult> AddInterestsToUserAsync([FromQuery] int userId, [FromBody] List<int> newInterests)
        {
            var result = await _userInterestsService.AddInterestsToUserAsync(userId, newInterests);
            if (result)
            {
                return Ok(new { message = "Interests added successfully." });
            }

            return BadRequest(new { message = "Failed to add interests." });
        }

        [AllowAnonymous]
        [HttpDelete("RemoveInterestsFromUser")]
        public async Task<IActionResult> RemoveInterestsFromUserAsync([FromQuery] int userId, [FromQuery] List<int> interestIds)
        {
            var result = await _userInterestsService.RemoveInterestsFromUserAsync(userId, interestIds);
            if (result)
            {
                return Ok(new { message = "Interests removed successfully." });
            }

            return BadRequest(new { message = "Failed to remove interests." });
        }

        [AllowAnonymous]
        [HttpGet("GetUserInterests")]
        public async Task<IActionResult> GetUserInterestsAsync([FromQuery] int userId)
        {
            var interests = await _userInterestsService.GetUserInterestsAsync(userId);
            if (interests == null || interests.Count == 0)
            {
                return NotFound(new { message = "No interests found for this user." });
            }

            return Ok(interests);
        }
    }
}
