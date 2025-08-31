using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;

        public AreaController(IAreaRepository areaRepository, IMapper mapper)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Area>))]
        public IActionResult GetAreas()
        {
            var areas = _mapper.Map<List<AreaDTO>>(_areaRepository.GetAreas());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(areas);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateArea([FromBody] AreaDTO areaCreate)
        {
            if (areaCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var areaToCreate = _mapper.Map<Area>(areaCreate);

            if (!_areaRepository.CreateArea(areaToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the area {areaToCreate.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{areaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateArea(int areaId, [FromBody] AreaDTO areaUpdate)
        {
            if (areaUpdate == null || areaId != areaUpdate.Id)
                return BadRequest(ModelState);

            if (!_areaRepository.AreaExists(areaId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var areaToUpdate = _mapper.Map<Area>(areaUpdate);

            if (!_areaRepository.UpdateArea(areaToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong updating the area {areaToUpdate.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully updated");
        }

        [HttpDelete("{areaId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteArea(int areaId)
        {
            if (!_areaRepository.AreaExists(areaId))
                return NotFound();

            var areaToDelete = _areaRepository.GetArea(areaId);

            if (!_areaRepository.DeleteArea(areaToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the area {areaId}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully deleted");
        }
    }
}
