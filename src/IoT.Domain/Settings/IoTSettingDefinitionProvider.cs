using Volo.Abp.Settings;

namespace IoT.Settings
{
    public class IoTSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(IoTSettings.MySetting1));
        }
    }
}
