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
    public class SettingController : ControllerBase
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IMapper _mapper;

        public SettingController(ISettingRepository settingRepository, IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Setting>))]
        public IActionResult GetSettings()
        {
            var settings = _mapper.Map<List<SettingDTO>>( _settingRepository.GetSettings());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(settings);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateSetting([FromBody] SettingDTO settingCreate)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var settingToCreate = _mapper.Map<Setting>(settingCreate);

            if(!_settingRepository.CreateSetting(settingToCreate))
                return StatusCode(500, "A problem occurred while handling your request.");

            return Ok("Successfully created");
        }

        [HttpPut("{settingId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateSetting(int settingId, [FromBody] SettingDTO settingUpdate)
        {
            if (settingUpdate == null || settingId != settingUpdate.Id)
                return BadRequest(ModelState);

            if (!_settingRepository.SettingExists(settingId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var settingToUpdate = _mapper.Map<Setting>(settingUpdate);

            if (!_settingRepository.UpdateSetting(settingToUpdate))
                return StatusCode(500, "A problem occurred while handling your request.");

            return Ok("Successfully updated");
        }
    }
}
