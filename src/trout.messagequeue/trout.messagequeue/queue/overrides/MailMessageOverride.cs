using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace trout.emailservice.queue.overrides
{
    public abstract class MailMessageOverride
    {

        protected string overrideString, prependString, appendString;
        protected bool clear;

        public MailMessageOverride Override(string value)
        {
            overrideString = value;
            return this;
        }

        public MailMessageOverride Prepend(string value)
        {
            prependString += value;
            return this;
        }

        public MailMessageOverride Append(string value)
        {
            appendString += value;
            return this;
        }

        public MailMessageOverride Clear()
        {
            clear = true;
            return this;
        }

        /// <summary>
        /// Will apply override first then apply prepend and append strings, and finally clear if needed
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract MailMessage ApplyOverride(MailMessage message);

        protected MailAddressCollection ApplyMailAddressOverride(MailAddressCollection addresses)
        {
            if (clear)
            {
                addresses.Clear();
            }
            else
            {
                if (!string.IsNullOrEmpty(overrideString))
                {
                    addresses.Clear();
                    addresses.Add(overrideString);
                }

                if (!string.IsNullOrEmpty(prependString))
                {
                    var old = addresses.ToString();
                    addresses.Clear();
                    addresses.Add(prependString);
                    addresses.Add(old);
                }

                if (!string.IsNullOrEmpty(appendString))
                {
                    addresses.Add(appendString);
                }
            }

            return addresses;
        }

        protected string ApplyStringOverride(string original)
        {
            var output = "";

            if (!clear)
            {
                output = original;

                if (!string.IsNullOrEmpty(overrideString))
                {
                    output = overrideString;
                }

                if (!string.IsNullOrEmpty(prependString))
                {
                    output = prependString + output;
                }

                if (!string.IsNullOrEmpty(appendString))
                {
                    output = output + appendString;
                }
            }

            return output;
        }
    }
}
