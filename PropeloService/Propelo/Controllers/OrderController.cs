using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Propelo.DTO;
using Propelo.Interfaces;
using Propelo.Models;

namespace Propelo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Order>))]
        public IActionResult GetOrders()
        {
            var orders = _mapper.Map<List<OrderDTO>>(_orderRepository.GetOrders());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        public IActionResult GetOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            var order = _mapper.Map<List<OrderDTO>>(_orderRepository.GetOrder(orderId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(order);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOrder([FromBody] Order orderCreate)
        {
            if(orderCreate == null)
                return BadRequest(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderToCreate =_mapper.Map<Order>(orderCreate);

            if(!_orderRepository.CreateOrder(orderToCreate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully created");
        }

        [HttpPut("{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult UpdateOrder(int orderId, [FromBody] Order orderUpdate)
        {
            if (orderUpdate == null)
                return BadRequest(ModelState);

            if(orderId != orderUpdate.Id)
                return BadRequest(ModelState);

            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var orderToUpdate = _mapper.Map<Order>(orderUpdate);

            if (!_orderRepository.UpdateOrder(orderToUpdate))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully updated");
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOrder(int orderId)
        {
            if (!_orderRepository.OrderExists(orderId))
                return NotFound();

            var orderToDelete = _orderRepository.GetOrder(orderId);

            if (!_orderRepository.DeleteOrder(orderToDelete))
                return StatusCode(500, "A problem happened while handling your request.");

            return Ok("Successfully deleted");
        }
    }
}
