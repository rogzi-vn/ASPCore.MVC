using Volo.Abp.Settings;

namespace ASPCoreMVC.Settings
{
    public class ASPCoreMVCSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(ASPCoreMVCSettings.MySetting1));
        }
    }
}
