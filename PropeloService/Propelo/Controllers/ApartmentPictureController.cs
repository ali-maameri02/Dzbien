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
    public class ApartmentPictureController : ControllerBase
    {
        private readonly IApartmentPictureRepository _apartmentPictureRepository;
        private readonly IMapper _mapper;
        public ApartmentPictureController(IApartmentPictureRepository apartmentPictureRepository, IMapper mapper)
        {
            _apartmentPictureRepository = apartmentPictureRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPicturets()
        {
            var Picturets = await _apartmentPictureRepository.GetPicturesAsync();
            return Ok(Picturets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPicturetById(int id)
        {
            var Picturet = await _apartmentPictureRepository.GetPictureByIdAsync(id);

            if (Picturet == null)
                return NotFound();

            return Ok(Picturet);
        }
        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetPicturetByApartmentId(int apartmentId)
        {
            var picturet = await _apartmentPictureRepository.GetApartmentPicturesByApartmentIdAsync(apartmentId);

            if (picturet == null)
                return NotFound();

            return Ok(picturet);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePicturet([FromForm] ApartmentPictureDTO picturetDto)
        {
            try
            {
                if (picturetDto.Pictures == null || !picturetDto.Pictures.Any())
                {
                    return BadRequest("No files uploaded.");
                }

                // Call the repository method to save multiple pictures
                var pictures = await _apartmentPictureRepository.CreateApartmentPictureAsync(picturetDto);

                if (pictures == null)
                    return StatusCode(500, "File Upload Failed");

                var createdPictures = await _apartmentPictureRepository.SaveAllAsync();
                if (createdPictures == null)
                    return NotFound();

                return Ok("Files Uploaded Successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdatePicturet(int Id, [FromForm] ApartmentPictureDTO picturetDto)
        {
            try
            {
                if (picturetDto.Pictures == null || !picturetDto.Pictures.Any())
                {
                    return BadRequest("No files uploaded.");
                }

                // Call the repository method to save multiple pictures
                var pictures = await _apartmentPictureRepository.UpdatePictureAsync(picturetDto);

                if (pictures == null)
                    return StatusCode(500, "File Upload Failed");

                var createdPictures = await _apartmentPictureRepository.SaveAllAsync();
                if (createdPictures == null)
                    return NotFound();

                return Ok("Files Uploaded Successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePicturet(int id)
        {
            try
            {
                var picture = await _apartmentPictureRepository.DeletePictureAsync(id);
                if (picture == null)
                    return NotFound();

                var deletedPicture = await _apartmentPictureRepository.SaveAllAsync();
                if (deletedPicture == null)
                    return NotFound();

                return Ok("File Deleted Successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
