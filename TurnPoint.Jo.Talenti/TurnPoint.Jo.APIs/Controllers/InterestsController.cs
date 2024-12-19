using Microsoft.AspNetCore.Mvc;
using TurnPoint.Jo.APIs.Interfaceses;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> GetAllInterestsAsync()
        {
            var interests = await _interestsService.GetAllInterestsAsync();
            return Ok(interests);
        }

        [HttpGet("GetInterestByIdAsync")]
        public async Task<IActionResult> GetInterestByIdAsync([FromQuery] int interestId)
        {
            var interest = await _interestsService.GetInterestByIdAsync(interestId);
            if (interest == null)
            {
                return NotFound(new { message = $"Interest with ID {interestId} not found." });
            }

            return Ok(interest);
        }

        [AllowAnonymous]
        [HttpPost("AddInterestAsync")]
        public async Task<IActionResult> AddInterestAsync([FromBody] string newInterest)
        {
            if (newInterest == null)
            {
                return BadRequest(new { message = "Invalid interest data." });
            }

            var result = await _interestsService.AddInterestAsync(newInterest);
            if (!result)
            {
                return BadRequest(new { message = "Failed to add interest Already Exists." });
            }

            return Ok(new { message = "Added Successfully." });
        }

        [AllowAnonymous]
        [HttpPut("UpdateInterestAsync")]
        public async Task<IActionResult> UpdateInterestAsync([FromQuery] int interestId, [FromBody] string updatedInterestName)
        {
            if (updatedInterestName == null || interestId < 1)
            {
                return BadRequest(new { message = "Invalid interest data new name or id is null." });
            }

            var result = await _interestsService.UpdateInterestAsync(interestId, updatedInterestName);
            if (result)
            {
                return Ok(new { message = $"Interest with ID {interestId} updated Successfully." });
            }

            return Conflict(new { message = $"Interest with ID {interestId} already found." });
        }

        [AllowAnonymous]
        [HttpDelete("DeleteInterestAsync")]
        public async Task<IActionResult> DeleteInterestAsync([FromQuery] int interestId)
        {
            var result = await _interestsService.DeleteInterestAsync(interestId);
            if (!result)
            {
                return NoContent();
            }

            return Ok(new { message = $"Interest with ID {interestId} Deleted Successfully." });
        }
    }
}
