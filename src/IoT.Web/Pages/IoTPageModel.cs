using IoT.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace IoT.Web.Pages
{
    /* Inherit your PageModel classes from this class.
     */
    public abstract class IoTPageModel : AbpPageModel
    {
        protected IoTPageModel()
        {
            LocalizationResourceType = typeof(IoTResource);
        }
    }
}