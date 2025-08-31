using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApartmentController : ControllerBase
    {
        private readonly IApartmentRepository _apartmentRepository;
        private readonly IMapper _mapper;

        public ApartmentController(IApartmentRepository apartmentRepository, IMapper mapper)
        {
            _apartmentRepository = apartmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Apartment>))]
        public IActionResult GetApartments()
        {
            var apartments = _mapper.Map<List<ApartmentDTO>>(_apartmentRepository.GetApartments());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(apartments);
        }

        //get apartment by id
        [HttpGet("{apartmentId}")]
        [ProducesResponseType(200, Type = typeof(Apartment))]
        [ProducesResponseType(400)]
        public IActionResult GetApartment(int apartmentId)
        {
            if (!_apartmentRepository.ApartmentExists(apartmentId))
                return NotFound();

            var apartment = _mapper.Map<ApartmentDTO>(_apartmentRepository.GetApartment(apartmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(apartment);
        }

        [HttpGet("areas/{apartmentId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Area>))]
        [ProducesResponseType(400)]
        public IActionResult GetAreasByApartment(int apartmentId)
        {
            if (!_apartmentRepository.ApartmentExists(apartmentId))
                return NotFound();

            var areas = _mapper.Map<List<AreaDTO>>(_apartmentRepository.GetAreasByApartment(apartmentId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(areas);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateApartment([FromBody] ApartmentDTO apartmentCreate)
        {  
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

             var apartmentToCreate = _mapper.Map<Apartment>(apartmentCreate);

            if (!_apartmentRepository.CreateApartment(apartmentToCreate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully created");
        }

        [HttpPut("{apartmentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateApartment(int apartmentId, [FromBody] ApartmentDTO apartmentUpdate)
        {
            if (apartmentUpdate == null)
                return BadRequest(ModelState);

            if(apartmentId != apartmentUpdate.Id)
                return BadRequest(ModelState);

            if (!_apartmentRepository.ApartmentExists(apartmentId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var apartmentToUpdate = _mapper.Map<Apartment>(apartmentUpdate);

            if (!_apartmentRepository.UpdateApartment(apartmentToUpdate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully updated");
        }

        [HttpDelete("{apartmentId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteApartment(int apartmentId)
        {
            if (!_apartmentRepository.ApartmentExists(apartmentId))
                return NotFound();

            var apartmentToDelete = _apartmentRepository.GetApartment(apartmentId);

            if (!_apartmentRepository.DeleteApartment(apartmentToDelete))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully deleted");
        }
    }
}
