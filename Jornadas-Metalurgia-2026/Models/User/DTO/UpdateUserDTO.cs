using System.ComponentModel.DataAnnotations;

namespace Jornadas_Metalurgia_2026.Models.User.DTO
{
    public class UpdateUserDTO
    {
        [Required]
        public string UserName { get; set; } = null;
        [Required]
        public string Email { get; set; } = null;

        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null;
    }
}
