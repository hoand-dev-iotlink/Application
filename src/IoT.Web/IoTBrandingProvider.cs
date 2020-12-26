using Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Components;
using Volo.Abp.DependencyInjection;

namespace IoT.Web
{
    [Dependency(ReplaceServices = true)]
    public class IoTBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "IoT";
    }
}
