using System;
using System.Configuration;

namespace trout.messagequeue.config.staticoverrides
{
    public sealed class StaticOverrideConfigurationElement : ConfigurationElement
    {
        public Type StaticOverrideType
        {
            get { return Type.GetType(StaticOverrideTypeString); }
            set { StaticOverrideTypeString = value.ToString(); }
        }

        [ConfigurationProperty("type", DefaultValue = "", IsKey = true, IsRequired = true)]
        private string StaticOverrideTypeString
        {
            get
            {
                return base["type"].ToString();
            }
            set
            {
                base["type"] = value;
            }
        }

        [ConfigurationProperty("doAppend", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool DoAppend
        {
            get
            {
                return ((bool)(base["doAppend"]));
            }
            set
            {
                base["doAppend"] = value;
            }
        }

        [ConfigurationProperty("append", IsKey = false, IsRequired = false, DefaultValue = null)]
        public string Append
        {
            get
            {
                return ((string)(base["append"]));
            }
            set
            {
                base["append"] = value;
            }
        }

        [ConfigurationProperty("clear", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool Clear
        {
            get
            {
                return ((bool)(base["clear"]));
            }
            set
            {
                base["clear"] = value;
            }
        }

        [ConfigurationProperty("doOverride", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool DoOverride
        {
            get
            {
                return ((bool)(base["doOverride"]));
            }
            set
            {
                base["doOverride"] = value;
            }
        }

        [ConfigurationProperty("override", IsKey = false, IsRequired = false, DefaultValue = null)]
        public string Override
        {
            get
            {
                return ((string)(base["override"]));
            }
            set
            {
                base["override"] = value;
            }
        }

        [ConfigurationProperty("doPrepend", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool DoPrepend
        {
            get
            {
                return ((bool)(base["doPrepend"]));
            }
            set
            {
                base["doPrepend"] = value;
            }
        }

        [ConfigurationProperty("prepend", IsKey = false, IsRequired = false, DefaultValue = null)]
        public string Prepend
        {
            get
            {
                return ((string)(base["prepend"]));
            }
            set
            {
                base["prepend"] = value;
            }
        }
    }
}
