using System;
using System.Configuration;
using trout.messagequeue.queue.overrides;

namespace trout.messagequeue.config.staticoverrides
{
    /// <summary>
    /// Implementation of IStaticOverridesProvider which provides Static Overrides from the application configuration file.
    /// </summary>
    public sealed class ConfigFileStaticOverridesProvider : IStaticOverridesProvider
    {
        private OverrideList List;

        /// <summary>
        /// List of all Static Overrides to apply when sending
        /// </summary>
        public OverrideList StaticOverrides
        {
            get
            {
                return List ?? (List = CreateStaticOverrideList());
            }
        }

        private OverrideList CreateStaticOverrideList()
        {
            OverrideList list = new OverrideList();

            StaticOverrideConfigurationSectionGroup section = (StaticOverrideConfigurationSectionGroup)ConfigurationManager.GetSection("staticOverrides");

            if (section != null)
            {
                foreach (StaticOverrideConfigurationElement overrideConfig in section.StaticOverrides)
                {
                    MailMessageOverride mmOverride = (MailMessageOverride)Activator.CreateInstance(overrideConfig.StaticOverrideType);

                    if(overrideConfig.DoOverride)
                    {
                        mmOverride.Override(overrideConfig.Override);
                    }

                    if (overrideConfig.DoAppend)
                    {
                        mmOverride.Append(overrideConfig.Append);
                    }

                    if (overrideConfig.DoPrepend)
                    {
                        mmOverride.Prepend(overrideConfig.Prepend);
                    }

                    if (overrideConfig.Clear)
                    {
                        mmOverride.Clear();
                    }

                    list.Add(mmOverride);
                }
            }

            return list;
        }
    }
}
