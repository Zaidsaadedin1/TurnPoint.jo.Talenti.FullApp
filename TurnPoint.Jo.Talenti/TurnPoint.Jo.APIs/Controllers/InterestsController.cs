using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Common.InterestDtos;

namespace TurnPoint.Jo.APIs.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestsService _interestsService;

        public InterestsController(IInterestsService interestsService)
        {
            _interestsService = interestsService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllInterests")]
        public async Task<ActionResult<GenericResponse<IEnumerable<GetInterestDto>>>> GetAllInterestsAsync()
        {
            var response = await _interestsService.GetAllInterestsAsync();
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpGet("GetInterestById")]
        public async Task<ActionResult<GenericResponse<GetInterestDto>>> GetInterestByIdAsync([FromQuery] int interestId)
        {
            var response = await _interestsService.GetInterestByIdAsync(interestId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("AddInterest")]
        public async Task<ActionResult<GenericResponse<bool>>> AddInterestAsync([FromBody] string newInterest)
        {
            if (string.IsNullOrWhiteSpace(newInterest))
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid interest data."
                });
            }

            var response = await _interestsService.AddInterestAsync(newInterest);
            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPut("UpdateInterest")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateInterestAsync([FromQuery] int interestId, [FromBody] string updatedInterestName)
        {
            if (string.IsNullOrWhiteSpace(updatedInterestName) || interestId <= 0)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid interest data."
                });
            }

            var response = await _interestsService.UpdateInterestAsync(interestId, updatedInterestName);
            if (!response.Success)
            {
                return Conflict(response);
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpDelete("DeleteInterest")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteInterestAsync([FromQuery] int interestId)
        {
            if (interestId <= 0)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid interest ID."
                });
            }

            var response = await _interestsService.DeleteInterestAsync(interestId);
            if (!response.Success)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
