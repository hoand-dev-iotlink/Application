using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace IoT.Web.Components.InfoBox
{
    public class MainInfoBoxViewComponent : AbpViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("~/Components/InfoBox/Default.cshtml");
        }
    }
}
