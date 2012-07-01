using System;
using System.Configuration;
using trout.messagequeue.config;
using trout.messagequeue.config.staticoverrides;

namespace trout.messagequeue.queue.overrides
{
    public static class StaticOverrideList
    {
        private static OverrideList List;

        public static OverrideList GetStaticOverrideList()
        {
            return List ?? (List = CreateStaticOverrideList());
        }

        public static void SetStaticOverrideList(OverrideList list)
        {
            List = list;
        }

        private static OverrideList CreateStaticOverrideList()
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
