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
    public class ApartmentDocumentController : ControllerBase
    {
        private readonly IApartmentDocumentRepository _apartmentDocumentRepository;
        private readonly IMapper _mapper;

        public ApartmentDocumentController(IApartmentDocumentRepository apartmentDocumentRepository, IMapper mapper)
        {
            _apartmentDocumentRepository = apartmentDocumentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetDocuments()
        {
            var documents = await _apartmentDocumentRepository.GetDocumentsAsync();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDocumentById(int id)
        {
            var document = await _apartmentDocumentRepository.GetDocumentByIdAsync(id);

            if (document == null)
                return NotFound();

            return Ok(document);
        }

        [HttpGet("apartment/{apartmentId}")]
        public async Task<IActionResult> GetDocumentByApartmentId(int apartmentId)
        {
            var document = await _apartmentDocumentRepository.GetApartmentDocumentsByApartmentIdAsync(apartmentId);

            if (document == null)
                return NotFound();

            return Ok(document);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDocument([FromForm] ApartmentDocumentDTO documentDto)
        {
            try
            {
                if (documentDto.Documents == null || !documentDto.Documents.Any())
                {
                    return BadRequest("No files uploaded.");
                }

                // Call the repository method to save multiple pictures
                var pictures = await _apartmentDocumentRepository.CreateDocumentAsync(documentDto);

                if (pictures == null)
                    return StatusCode(500, "File Upload Failed");

                var createdPictures = await _apartmentDocumentRepository.SaveAllAsync();
                if (createdPictures==null)
                    return StatusCode(500, "File Upload Failed");

                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateDocument(int Id, [FromForm] ApartmentDocumentDTO documentDto)
        {
            try
            {
                if (documentDto.Documents == null || !documentDto.Documents.Any())
                {
                    return BadRequest("No files uploaded.");
                }

                // Call the repository method to save multiple pictures
                var pictures = await _apartmentDocumentRepository.UpdateDocumentAsync(documentDto);

                if (pictures == null)
                    return StatusCode(500, "File Upload Failed");

                var createdPictures = await _apartmentDocumentRepository.SaveAllAsync();
                if (createdPictures == null)
                    return StatusCode(500, "File Upload Failed");

                return Ok("File Uploaded Successfully");
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocument(int id)
        {
            try
            {
                var document = await _apartmentDocumentRepository.DeleteDocumentAsync(id);
                if (document == null)
                {
                    return NotFound("Document not found.");
                }
                return Ok("File Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
