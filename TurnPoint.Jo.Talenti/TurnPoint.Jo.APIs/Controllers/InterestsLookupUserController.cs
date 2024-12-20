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
            if (userId <= 0 || newInterests == null || newInterests.Count == 0)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid user ID or interests data."
                });
            }

            var result = await _userInterestsService.AddInterestsToUserAsync(userId, newInterests);
            if (!result)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to add interests."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "Interests added successfully.",
                Data = result
            });
        }

        [AllowAnonymous]
        [HttpDelete("RemoveInterestsFromUser")]
        public async Task<ActionResult<GenericResponse<bool>>> RemoveInterestsFromUserAsync([FromQuery] int userId, [FromQuery] List<int> interestIds)
        {
            if (userId <= 0 || interestIds == null || interestIds.Count == 0)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid user ID or interests data."
                });
            }

            var result = await _userInterestsService.RemoveInterestsFromUserAsync(userId, interestIds);
            if (!result)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to remove interests."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "Interests removed successfully.",
                Data = result
            });
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

            var interests = await _userInterestsService.GetUserInterestsAsync(userId);
            if (interests == null || interests.Count == 0)
            {
                return NotFound(new GenericResponse<List<GetInterestDto>>
                {
                    Success = false,
                    Message = "No interests found for this user.",
                    Data = null
                });
            }

            return Ok(new GenericResponse<List<GetInterestDto>>
            {
                Success = true,
                Message = "User interests retrieved successfully.",
                Data = interests
            });
        }
    }
}
