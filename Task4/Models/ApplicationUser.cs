using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using UserManagementSystem.Models.DTOs;

namespace UserManagementSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }

        public static ApplicationUser CreateUser(string fullName,string email)
        {
            ApplicationUser applicationUser = new()
            {
                FullName = fullName,
                UserName = email,
                Email =email
            };
            return applicationUser;
        }

    }
}
