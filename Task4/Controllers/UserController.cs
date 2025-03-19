using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementSystem.Data;
using UserManagementSystem.Models;
using UserManagementSystem.Models.DTOs;
using UserManagementSystem.Filters;
namespace UserManagementSystem.Controllers
{
    [TypeFilter(typeof(LogoutIfBlockedFilter))]
    [Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public async Task<IActionResult> Index()
        {
           List<ApplicationUserDto> users= await db.ApplicationUsers.Select(x=>new ApplicationUserDto()
            {
                Id=x.Id,
                Email=x.Email,
                LastLogin=x.LastLogin,
                Name=x.FullName,
                IsChecked=false,
                IsBlocked = x.LockoutEnd.HasValue && x.LockoutEnd > DateTime.UtcNow
            }).ToListAsync();
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> BlockUsers(List<ApplicationUserDto> applicationUserDtos)
        {
            var checkedUserDtos= applicationUserDtos.Where(x => x.IsChecked && !x.IsBlocked).Select(x=>x.Id);
            if (!checkedUserDtos.Any())
                return RedirectToAction(nameof(Index));
                
            await db.ApplicationUsers.Where(x => checkedUserDtos.Contains(x.Id))
                .ExecuteUpdateAsync(s=>s.SetProperty(u=>u.LockoutEnd, DateTime.UtcNow.AddYears(100)));
            TempData["Success"] = "User(s) have been blocked";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UnblockUsers(List<ApplicationUserDto> applicationUserDtos)
        {
            var checkedUserDtos = applicationUserDtos.Where(x => x.IsChecked && x.IsBlocked).Select(x => x.Id);
            if (!checkedUserDtos.Any())
                return RedirectToAction(nameof(Index));

            await db.ApplicationUsers.Where(x => checkedUserDtos.Contains(x.Id))
              .ExecuteUpdateAsync(s => s.SetProperty(u => u.LockoutEnd, default(DateTimeOffset?)));           
            TempData["Success"] = "User(s) have been unblocked";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUsers(List<ApplicationUserDto> applicationUserDtos)
        {
            List<string> checkedUserDtos = applicationUserDtos.Where(x => x.IsChecked).Select(x => x.Id).ToList();
            if (!checkedUserDtos.Any())
                return RedirectToAction(nameof(Index));
            await db.ApplicationUsers.Where(x => checkedUserDtos.Contains(x.Id)).ExecuteDeleteAsync();
            TempData["Success"] = "User(s) have been removed";
            return RedirectToAction(nameof(Index));
        }
    }
}
