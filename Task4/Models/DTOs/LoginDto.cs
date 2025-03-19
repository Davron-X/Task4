﻿using System.ComponentModel.DataAnnotations;

namespace UserManagementSystem.Models.DTOs
{
    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Display(Name = "Remember me")]
        public bool IsRemember { get; set; } = false;
    }
}
