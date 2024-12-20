using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;
using TurnPoint.Jo.APIs.Common.Shared;
using TurnPoint.Jo.APIs.Common.InterestDtos;

namespace TurnPoint.Jo.APIs.Controllers
{
    [Route("api/interests")]
    [ApiController]
    public class InterestsController : ControllerBase
    {
        private readonly IInterestsService _interestsService;

        public InterestsController(IInterestsService interestsService)
        {
            _interestsService = interestsService;
        }

        [AllowAnonymous]
        [HttpGet("GetAllInterestsAsync")]
        public async Task<ActionResult<GenericResponse<IEnumerable<GetInterestDto>>>> GetAllInterestsAsync()
        {
            var interests = await _interestsService.GetAllInterestsAsync();
            return Ok(new GenericResponse<IEnumerable<GetInterestDto>>
            {
                Success = true,
                Message = "Retrieved all interests successfully.",
                Data = interests
            });
        }

        [HttpGet("GetInterestByIdAsync")]
        public async Task<ActionResult<GenericResponse<GetInterestDto>>> GetInterestByIdAsync([FromQuery] int interestId)
        {
            var interest = await _interestsService.GetInterestByIdAsync(interestId);
            if (interest == null)
            {
                return NotFound(new GenericResponse<GetInterestDto>
                {
                    Success = false,
                    Message = $"Interest with ID {interestId} not found."
                });
            }

            return Ok(new GenericResponse<GetInterestDto>
            {
                Success = true,
                Message = "Interest retrieved successfully.",
                Data = interest
            });
        }

        [AllowAnonymous]
        [HttpPost("AddInterestAsync")]
        public async Task<ActionResult<GenericResponse<bool>>> AddInterestAsync([FromBody] string newInterest)
        {
            if (string.IsNullOrEmpty(newInterest))
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid interest data."
                });
            }

            var result = await _interestsService.AddInterestAsync(newInterest);
            if (!result)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Failed to add interest. It already exists."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = "Interest added successfully."
            });
        }

        [AllowAnonymous]
        [HttpPut("UpdateInterestAsync")]
        public async Task<ActionResult<GenericResponse<bool>>> UpdateInterestAsync([FromQuery] int interestId, [FromBody] string updatedInterestName)
        {
            if (string.IsNullOrEmpty(updatedInterestName) || interestId < 1)
            {
                return BadRequest(new GenericResponse<bool>
                {
                    Success = false,
                    Message = "Invalid interest data. New name or ID is null."
                });
            }

            var result = await _interestsService.UpdateInterestAsync(interestId, updatedInterestName);
            if (!result)
            {
                return Conflict(new GenericResponse<bool>
                {
                    Success = false,
                    Message = $"Interest with ID {interestId} already exists or update failed."
                });
            }

            return Ok(new GenericResponse<object>
            {
                Success = true,
                Message = $"Interest with ID {interestId} updated successfully."
            });
        }

        [AllowAnonymous]
        [HttpDelete("DeleteInterestAsync")]
        public async Task<ActionResult<GenericResponse<bool>>> DeleteInterestAsync([FromQuery] int interestId)
        {
            var result = await _interestsService.DeleteInterestAsync(interestId);
            if (!result)
            {
                return NotFound(new GenericResponse<bool>
                {
                    Success = false,
                    Message = $"Interest with ID {interestId} not found."
                });
            }

            return Ok(new GenericResponse<bool>
            {
                Success = true,
                Message = $"Interest with ID {interestId} deleted successfully."
            });
        }
    }
}
