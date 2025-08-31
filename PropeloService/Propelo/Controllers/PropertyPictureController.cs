using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;
using Propelo.Repository;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyPictureController : ControllerBase
    {
        private readonly IPropertyPictureRepository _propertyPictureRepository;
        private readonly IMapper _mapper;

        public PropertyPictureController(IPropertyPictureRepository propertyPictureRepository, IMapper mapper)
        {
            _propertyPictureRepository = propertyPictureRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPicturets()
        {
            var Picturets = await _propertyPictureRepository.GetPropertyPicturesAsync();
            return Ok(Picturets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPropertyPicturesAsync(int id)
        {
            var Picturet = await _propertyPictureRepository.GetPropertyPictureByIdAsync(id);

            if (Picturet == null)
                return NotFound();

            return Ok(Picturet);
        }

        [HttpGet("property/{propertyId}")]
        public async Task<IActionResult> GetPropertyPicturesByPropertyIdAsync(int propertyId)
        {
            var picturet = await _propertyPictureRepository.GetPropertyPicturesByPropertyIdAsync(propertyId);

            if (picturet == null)
                return NotFound();

            return Ok(picturet);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePictures([FromForm] PropertyPictureDTO pictureDto)
        {
            try
            {
                if (pictureDto.Pictures == null || !pictureDto.Pictures.Any())
                {
                    return BadRequest("No files uploaded.");
                }

                // Call the repository method to save multiple pictures
                var pictures = await _propertyPictureRepository.CreatePropertyPictureAsync(pictureDto);

                if (pictures == null)
                    return StatusCode(500, "File Upload Failed");

                var CreatedPictures = await _propertyPictureRepository.SaveAllAsync();
                if (CreatedPictures == null)
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
        public async Task<IActionResult> UpdatePicture(int id, [FromForm] PropertyPictureDTO pictureDto)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Picture ID.");
            }

            try
            {
                var updatedPicture = await _propertyPictureRepository.UpdatePropertyPictureAsync(pictureDto);
                if (updatedPicture == null)
                {
                    return NotFound("Picture not found.");
                }

                return Ok("Picture updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // Delete a picture by id
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicture(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Picture ID.");
            }

            try
            {
                var deletedPicture = await _propertyPictureRepository.DeletePropertyPictureAsync(id);
                if (deletedPicture == null)
                {
                    return NotFound("Picture not found.");
                }

                return Ok("Picture deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
