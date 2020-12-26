using IoT.MongoDB;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace IoT.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(IoTMongoDbModule),
        typeof(IoTApplicationContractsModule)
        )]
    public class IoTDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
