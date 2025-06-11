using AutoMapper;
using CoreXCrud.DTOs.ProductDtos;
using CoreXCrud.Models;
using CoreXCrud.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CoreXCrud.Controllers
{
    [Authorize]  // 📌 JWT ile yetkilendirme ekledik
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<ProductDto> _validator;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<ProductDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            Log.Information("GET /api/Products çağrıldı");
            var products = await _unitOfWork.Products.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            Log.Information("GET /api/Products/{Id} çağrıldı", id);
            var product = await _unitOfWork.Products.GetByIdAsync(id);
            if (product == null)
            {
                Log.Warning("Ürün bulunamadı: {Id}", id);
                return NotFound();
            }
            var productDto = _mapper.Map<ProductDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            Log.Information("POST /api/Products çağrıldı. Yeni ürün: {@Product}", productDto);
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Ürün doğrulama hatası: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            var createdProductDto = _mapper.Map<ProductDto>(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, createdProductDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto productDto)
        {
            if (id != productDto.Id) return BadRequest();
            Log.Information("PUT /api/Products/{Id} çağrıldı. Güncellenen ürün: {@Product}", id, productDto);
            var validationResult = await _validator.ValidateAsync(productDto);
            if (!validationResult.IsValid)
            {
                Log.Warning("Ürün doğrulama hatası: {@Errors}", validationResult.Errors);
                return BadRequest(validationResult.Errors);
            }

            var product = _mapper.Map<Product>(productDto);
            await _unitOfWork.Products.UpdateAsync(product);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            Log.Information("DELETE /api/Products/{Id} çağrıldı", id);
            await _unitOfWork.Products.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
