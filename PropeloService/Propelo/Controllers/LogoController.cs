using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Repository;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoController : ControllerBase
    {
        private readonly ILogoRepository _logoRepository;
        private readonly IMapper _mapper;

        public LogoController(ILogoRepository logoRepository, IMapper mapper)
        {
            _logoRepository = logoRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetLogos()
        {
            var logos = await _logoRepository.GetLogosAsync();
            return Ok(logos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogotById(int id)
        {
            var logo = await _logoRepository.GetLogoByIdAsync(id);

            if (logo == null)
                return NotFound();

            return Ok(logo);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePicturet([FromForm] LogoDTO logoDto)
        {
            try
            {
                var logo = await _logoRepository.CreateLogoAsync(logoDto);

                if (logo == null)
                    return StatusCode(500, "File Upload Failed");

                var createLogo = await _logoRepository.SaveAllAsync();
                if (createLogo == null)
                    return NotFound();

                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLogo(int id, [FromForm] LogoDTO logoDTO)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid logo ID.");
            }

            try
            {
                var updatedLogo = await _logoRepository.UpdateLogoAsync(logoDTO, id);
                if (updatedLogo == null)
                {
                    return NotFound();
                }

                return Ok("File update Successful");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here) and return a suitable error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogo(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid logo ID.");
            }

            if (await _logoRepository.DeleteLogoAsync(id))
            {
                return Ok("Logo deleted successfully");
            }

            return StatusCode(500, "Internal server error: Unable to delete logo");
        }
    }
}
