using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsWebApp.Areas.Identity.Pages.Account.Manage
{
    public class NotificationsModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;

        public NotificationsModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<string> GetUserId()
        {
            return await Task.Run(() => _userManager.GetUserId(HttpContext.User).ToString());
        }
    }
}