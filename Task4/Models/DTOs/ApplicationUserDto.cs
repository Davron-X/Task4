namespace UserManagementSystem.Models.DTOs
{
    public class ApplicationUserDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string? Email { get; set; }

        public DateTime? LastLogin { get; set; } 

        public bool IsChecked { get; set; }

        public bool IsBlocked { get; set; }
    }
}
