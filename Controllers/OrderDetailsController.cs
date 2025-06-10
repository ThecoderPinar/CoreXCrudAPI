using AutoMapper;
using CoreXCrud.DTOs;
using CoreXCrud.Models;
using CoreXCrud.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CoreXCrud.Controllers
{
[Authorize] // 📌 JWT ile yetkilendirme ekledik
[Route("api/[controller]")]
[ApiController]
public class OrderDetailsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<OrderDetailDto> _validator;

        public OrderDetailsController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<OrderDetailDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrderDetails()
        {
            var orderDetails = await _unitOfWork.OrderDetails.GetAllAsync();
            var orderDetailsDto = _mapper.Map<IEnumerable<OrderDetailDto>>(orderDetails);
            return Ok(orderDetailsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetail(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetails.GetByIdAsync(id);
            if (orderDetail == null) return NotFound();
            var orderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
            return Ok(orderDetailDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailDto orderDetailDto)
        {
            var validationResult = await _validator.ValidateAsync(orderDetailDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            await _unitOfWork.CompleteAsync();

            var createdOrderDetailDto = _mapper.Map<OrderDetailDto>(orderDetail);
            return CreatedAtAction(nameof(GetOrderDetail), new { id = orderDetail.Id }, createdOrderDetailDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailDto orderDetailDto)
        {
            if (id != orderDetailDto.Id) return BadRequest();
            var validationResult = await _validator.ValidateAsync(orderDetailDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var orderDetail = _mapper.Map<OrderDetail>(orderDetailDto);
            await _unitOfWork.OrderDetails.UpdateAsync(orderDetail);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderDetail(int id)
        {
            await _unitOfWork.OrderDetails.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
