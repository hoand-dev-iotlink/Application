using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IoT.Web.Pages
{
    [Authorize]
    public class IndexModel : IoTPageModel
    {
        public IActionResult OnGet()
        {
            return RedirectToPage("./BTS/MapAnten/Index");
        }
    }
}