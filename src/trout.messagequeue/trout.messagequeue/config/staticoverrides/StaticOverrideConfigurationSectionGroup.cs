using System.Configuration;

namespace trout.messagequeue.config.staticoverrides
{
    public class StaticOverrideConfigurationSectionGroup : ConfigurationSection
    {
        [ConfigurationProperty("overrides")]
        public StaticOverrideCollection StaticOverrides
        {
            get { return ((StaticOverrideCollection)(base["overrides"])); }
        }
    }
}