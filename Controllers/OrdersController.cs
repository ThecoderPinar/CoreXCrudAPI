using AutoMapper;
using CoreXCrud.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using CoreXCrud.DTOs.OrderDtos;
using CoreXCrud.Entities;

namespace CoreXCrud.Controllers
{
[Authorize] // 📌 JWT ile yetkilendirme ekledik
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderDto> _validator;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<OrderDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            Log.Information("GET /api/Orders çağrıldı");
            var orders = await _unitOfWork.Orders.GetAllAsync();
            var ordersDto = _mapper.Map<IEnumerable<OrderDto>>(orders);
            return Ok(ordersDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            Log.Information("GET /api/Orders/{Id} çağrıldı", id);
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
            {
                Log.Warning("Sipariş bulunamadı: {Id}", id);
                return NotFound();
            }
            var orderDto = _mapper.Map<OrderDto>(order);
            return Ok(orderDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderDto orderDto)
        {
            Log.Information("POST /api/Orders çağrıldı. Yeni sipariş: {@Order}", orderDto);
            var validationResult = await _validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Sipariş doğrulama hatası: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var order = _mapper.Map<Order>(orderDto);
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.CompleteAsync();

            var createdOrderDto = _mapper.Map<OrderDto>(order);
            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, createdOrderDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderDto orderDto)
        {
            if (id != orderDto.Id) return BadRequest();
            Log.Information("PUT /api/Orders/{Id} çağrıldı. Güncellenen sipariş: {@Order}", id, orderDto);
            var validationResult = await _validator.ValidateAsync(orderDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Sipariş doğrulama hatası: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var order = _mapper.Map<Order>(orderDto);
            await _unitOfWork.Orders.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            Log.Information("DELETE /api/Orders/{Id} çağrıldı", id);
            await _unitOfWork.Orders.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
