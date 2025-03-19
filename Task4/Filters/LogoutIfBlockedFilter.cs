using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using UserManagementSystem.Data;
using UserManagementSystem.Models;

namespace UserManagementSystem.Filters
{
    public class LogoutIfBlockedFilter : IAsyncAuthorizationFilter
    {
        private readonly ApplicationDbContext db;
        public LogoutIfBlockedFilter(ApplicationDbContext db)
        {
            this.db = db;
        }
        public  async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
                return;
            string? id = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id))
                return;
            ApplicationUser? user = await db.ApplicationUsers.FindAsync(id);
            if (user is null || user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            {
                await context.HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                context.Result = new RedirectToActionResult("Login", "Auth", new());
            }
        }
    }
}
