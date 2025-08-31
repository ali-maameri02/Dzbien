using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;
using Propelo.Repository;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoterPictureController : ControllerBase
    {
        private readonly IPromoterPictureRepository _promoterPictureRepository;
        private readonly IMapper _mapper;
        

        public PromoterPictureController(IPromoterPictureRepository promoterPictureRepository, IMapper mapper)
        {
            _promoterPictureRepository = promoterPictureRepository;
            _mapper = mapper;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetLogos()
        {
            var logos = await _promoterPictureRepository.GetPromoterPicturesAsync();
            return Ok(logos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLogotById(int id)
        {
            var logo = await _promoterPictureRepository.GetPromoterPictureByIdAsync(id);

            if (logo == null)
                return NotFound();

            return Ok(logo);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePicturet([FromForm] PromoterPictureDTO promoterPictureDto)
        {
            try
            {
                var picture = await _promoterPictureRepository.CreatePromoterPictureAsync(promoterPictureDto);

                if (picture == null)
                {
                    return StatusCode(500, "File Upload Failed");
                }

                var createdPicture = await _promoterPictureRepository.SaveAllAsync();
                if (createdPicture == null)
                {
                    return StatusCode(500, "File Upload Failed");
                }
                return Ok("File Uploaded Successfully");
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePicture(int id, [FromForm] PromoterPictureDTO promoterPictureDTO)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid logo ID.");
            }

            try
            {
                var updatedPicture = await _promoterPictureRepository.UpdatePromoterPicture(promoterPictureDTO, id);
                if (updatedPicture == null)
                {
                    return NotFound();
                }

                return Ok("File updated Successful");
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here) and return a suitable error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Picture ID.");
            }

            if (await _promoterPictureRepository.DeletePromoterPictureAsync(id))
            {
                return Ok("File deleted successfully");
            }

            return StatusCode(500, "Internal server error: Unable to delete file");
        }
    }
}
