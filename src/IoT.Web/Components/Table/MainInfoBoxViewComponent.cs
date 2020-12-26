using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace IoT.Web.Components.Table
{
    public class MainTableViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Components/Table/Default.cshtml");
        }
    }
}
