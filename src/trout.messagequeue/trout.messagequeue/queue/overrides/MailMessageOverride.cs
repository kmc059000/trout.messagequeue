using System.Net.Mail;

namespace trout.messagequeue.queue.overrides
{
    /// <summary>
    /// Override which gets applied to emails prior to being sent
    /// </summary>
    public abstract class MailMessageOverride
    {

        /// <summary>
        /// 
        /// </summary>
        protected string overrideString, prependString, appendString;


        /// <summary>
        /// 
        /// </summary>
        protected bool clear;

        /// <summary>
        /// Overrides the value before sending. The value is completely replaced.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public MailMessageOverride Override(string value)
        {
            overrideString = value;
            return this;
        }

        /// <summary>
        /// Places the override string before the existing string before sending.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public MailMessageOverride Prepend(string value)
        {
            prependString += value;
            return this;
        }

        /// <summary>
        /// Places the override string after the existing string before sending.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public MailMessageOverride Append(string value)
        {
            appendString += value;
            return this;
        }

        /// <summary>
        /// Removes the existing value completely. All other overrides will not change the email
        /// </summary>
        /// <returns></returns>
        public MailMessageOverride Clear()
        {
            clear = true;
            return this;
        }

        /// <summary>
        /// Will apply overrides first, then apply prepend and append strings, and finally clear if needed
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public abstract MailMessage ApplyOverride(MailMessage message);

        /// <summary>
        /// Applies the override to a MailAddressCollection
        /// </summary>
        /// <param name="addresses">The addresses.</param>
        /// <returns></returns>
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
                    if(!string.IsNullOrWhiteSpace(old)) addresses.Add(old);
                }

                if (!string.IsNullOrEmpty(appendString))
                {
                    addresses.Add(appendString);
                }
            }

            return addresses;
        }

        /// <summary>
        /// Returns the override on a string field
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns></returns>
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
