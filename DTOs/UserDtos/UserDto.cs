namespace CoreXCrud.DTOs.UserDtos
{
    /// <summary>
    /// Kullanıcı DTO Modeli
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
