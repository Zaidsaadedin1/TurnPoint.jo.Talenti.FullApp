using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Common.InterestDtos;

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
        public async Task<ActionResult<GenericResponse<bool>>> AddInterestsToUserAsync([FromQuery] int userId, [FromBody] List<int> newInterests)
        {
            if (userId <= 0 || newInterests == null || !newInterests.Any())
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid user ID or interests data."
                });
            }

            var response = await _userInterestsService.AddInterestsToUserAsync(userId, newInterests);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpDelete("RemoveInterestsFromUser")]
        public async Task<ActionResult<GenericResponse<bool>>> RemoveInterestsFromUserAsync([FromQuery] int userId, [FromQuery] List<int> interestIds)
        {
            if (userId <= 0 || interestIds == null || !interestIds.Any())
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid user ID or interests data."
                });
            }

            var response = await _userInterestsService.RemoveInterestsFromUserAsync(userId, interestIds);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("GetUserInterests")]
        public async Task<ActionResult<GenericResponse<List<GetInterestDto>>>> GetUserInterestsAsync([FromQuery] int userId)
        {
            if (userId <= 0)
            {
                return BadRequest(new GenericResponse<List<GetInterestDto>>
                {
                    Success = false,
                    Message = "Invalid user ID."
                });
            }

            var response = await _userInterestsService.GetUserInterestsAsync(userId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
