using IoT.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace IoT.Permissions
{
    public class IoTPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(IoTPermissions.GroupName);

            //Define your own permissions here. Example:
            //myGroup.AddPermission(IoTPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<IoTResource>(name);
        }
    }
}
