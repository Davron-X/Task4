using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models.DTOs
{
    public class RegistrationDto
    {

        [Required]
        [StringLength(60)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Display(Name ="Remember me")]
        public bool IsRemember { get; set; } = false;
    }
}
