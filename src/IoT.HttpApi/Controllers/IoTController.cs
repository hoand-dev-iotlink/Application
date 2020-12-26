using IoT.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace IoT.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class IoTController : AbpController
    {
        protected IoTController()
        {
            LocalizationResource = typeof(IoTResource);
        }
    }
}