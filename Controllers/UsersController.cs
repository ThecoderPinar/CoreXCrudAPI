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
public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<UserDto> _validator;

        public UsersController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<UserDto> validator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
        }

        // 📌 Tüm Kullanıcıları Getir
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(usersDto);
        }

        // 📌 Belirli Bir Kullanıcıyı Getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) return NotFound();
            var userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        // 📌 Yeni Kullanıcı Ekle
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDto userDto)
        {
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            var createdUserDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, createdUserDto);
        }

        // 📌 Kullanıcı Güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
        {
            if (id != userDto.Id) return BadRequest();
            var validationResult = await _validator.ValidateAsync(userDto);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var user = _mapper.Map<User>(userDto);
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }

        // 📌 Kullanıcıyı Sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            await _unitOfWork.Users.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }
    }
}
