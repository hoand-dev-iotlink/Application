using System;
using System.Collections.Generic;
using System.Text;
using IoT.Localization;
using Volo.Abp.Application.Services;

namespace IoT
{
    /* Inherit your application services from this class.
     */
    public abstract class IoTAppService : ApplicationService
    {
        protected IoTAppService()
        {
            LocalizationResource = typeof(IoTResource);
        }
    }
}
