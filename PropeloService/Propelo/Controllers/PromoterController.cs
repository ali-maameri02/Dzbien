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
    public class PromoterController : ControllerBase
    {
        private readonly IPromoterRepository _promoterRepository;
        private readonly IMapper _mapper;

        public PromoterController(IPromoterRepository promoterRepository, IMapper mapper)
        {
            _promoterRepository = promoterRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type= typeof(IEnumerable<Promoter>))]
        public IActionResult GetPromoters()
        {
            var promoters =_mapper.Map<List<PromoterDTO>>(_promoterRepository.GetPromoters());
            
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(promoters);
        }

        [HttpGet("{promoterId}")]
        [ProducesResponseType(200, Type= typeof(Promoter))]
        [ProducesResponseType(400)]
        public IActionResult GetPromoter(int promoterId)
        {
            if(!_promoterRepository.PromoterExists(promoterId))
                return NotFound();

            var promoter = _mapper.Map<PromoterDTO>(_promoterRepository.GetPromoter(promoterId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(promoter);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePromoter([FromBody] PromoterDTO promoterCreate)
        {
            if(promoterCreate == null)
                return BadRequest(ModelState);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var promoterToCreate = _mapper.Map<Promoter>(promoterCreate);

            if(!_promoterRepository.CreatePromoter(promoterToCreate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully created");
        }

        [HttpPut("{promoterId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdatePromoter(int promoterId, [FromBody] PromoterDTO promoterUpdate)
        {
            if (promoterUpdate == null)
                return BadRequest(ModelState);

            if (promoterId != promoterUpdate.Id)
                return BadRequest(ModelState);

            if (!_promoterRepository.PromoterExists(promoterId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var promoterToUpdate = _mapper.Map<Promoter>(promoterUpdate);

            if (!_promoterRepository.UpdatePromoter(promoterToUpdate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully updated");
        }

        [HttpDelete("{promoterId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteApartment(int promoterId)
        {
            if (!_promoterRepository.PromoterExists(promoterId))
                return NotFound();

            var promoterToDelete = _promoterRepository.GetPromoter(promoterId);

            if (!_promoterRepository.DeletePromoter(promoterToDelete))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully deleted");
        }
    }
}
