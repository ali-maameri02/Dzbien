using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;
using Propelo.Repository;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IMapper _mapper;

        public PropertyController(IPropertyRepository propertyRepository, IMapper mapper)
        {
            _propertyRepository = propertyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType (200, Type=typeof(IEnumerable<Property>))]
        public IActionResult GetProperties()
        {
            //var properties = _propertyRepository.GetProperties();
            //var propertiesDTO = _mapper.Map<List<PropertyDTO>>(properties);

            var properties = _mapper.Map<List<PropertyDTO>>(_propertyRepository.GetProperties());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(properties);
        }

        [HttpGet("{propertyId}")]
        [ProducesResponseType(200, Type=typeof(Property))]
        [ProducesResponseType(400)]
        public IActionResult GetProperty(int propertyId)
        {
            if (!_propertyRepository.PropertyExists(propertyId))
                return NotFound();

            var property = _mapper.Map<PropertyDTO>(_propertyRepository.GetProperty(propertyId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(property);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProperty([FromBody] PropertyDTO propertyCreate)
        {
            if (propertyCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyToCreate = _mapper.Map<Property>(propertyCreate);

            if (!_propertyRepository.CreateProperty(propertyToCreate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully created");
        }

        [HttpPut("{propertyId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePropertyPicture(int propertyId, [FromBody] PropertyDTO propertyUpdate)
        {
            if (propertyUpdate == null)
                return BadRequest(ModelState);

            if (propertyId != propertyUpdate.Id)
                return BadRequest(ModelState);

            if (!_propertyRepository.PropertyExists(propertyId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyToUpdate = _mapper.Map<Property>(propertyUpdate);

            if (!_propertyRepository.UpdateProperty(propertyToUpdate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully Update");
        }

        [HttpDelete("{propertyId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProperty(int propertyId)
        {
            if (!_propertyRepository.PropertyExists(propertyId))
                return NotFound();

            var propertyToDelete= _propertyRepository.GetProperty(propertyId);

            if (!_propertyRepository.DeleteProperty(propertyToDelete))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully deleted");
        }
    }
}
