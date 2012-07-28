using System.Configuration;

namespace trout.messagequeue.config.staticoverrides
{
    internal sealed class StaticOverrideConfigurationSectionGroup : ConfigurationSection
    {
        [ConfigurationProperty("overrides")]
        public StaticOverrideCollection StaticOverrides
        {
            get { return ((StaticOverrideCollection)(base["overrides"])); }
        }
    }
}