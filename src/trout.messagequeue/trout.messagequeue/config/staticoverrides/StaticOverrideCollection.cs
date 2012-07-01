using System.Configuration;

namespace trout.messagequeue.config.staticoverrides
{
    [ConfigurationCollection(typeof(StaticOverrideConfigurationElement))]
    public class StaticOverrideCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new StaticOverrideConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((StaticOverrideConfigurationElement)(element)).StaticOverrideType;
        }

        public StaticOverrideConfigurationElement this[int idx]
        {
            get
            {
                return (StaticOverrideConfigurationElement)BaseGet(idx);
            }
        }
    }
}