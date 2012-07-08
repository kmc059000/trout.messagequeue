using System;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace trout.messagequeue.infrastrucure
{
    public sealed class DotNetBuiltInSmtpClient : ISmtpClient
    {
        readonly System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        private const int RetryAttempts = 3;


        public SendResult Send(MailMessage mailMessage)
        {
            return Send(mailMessage, RetryAttempts);
        }

        private SendResult Send(MailMessage mailMessage, int retries)
        {
            try
            {
                client.Send(mailMessage);
            }
            catch(ArgumentNullException)
            {
                return new SendResult(false, string.Format("Message is null"), RetryAttempts - retries + 1); 
            }
            catch (ObjectDisposedException)
            {
                return new SendResult(false, "Client Disposed", RetryAttempts - retries + 1);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if(mailMessage.From == null)
                {
                    return new SendResult(false, "From is null", RetryAttempts - retries + 1);
                }

                if (mailMessage.To.Count == 0 && mailMessage.To.Count == 0 && mailMessage.To.Count == 0)
                {
                    return new SendResult(false, "No recipients", RetryAttempts - retries + 1);
                }

                if (client.DeliveryMethod == SmtpDeliveryMethod.Network && string.IsNullOrEmpty(client.Host))
                {
                    return new SendResult(false, "Host is not set", RetryAttempts - retries + 1);
                }

                if (client.DeliveryMethod == SmtpDeliveryMethod.Network && (client.Port <= 0 || client.Port >65535))
                {
                    return new SendResult(false, string.Format("{0} port is invalid", client.Port), RetryAttempts - retries + 1);
                }

                return new SendResult(false, "Invalid Operation: " + invalidOperationException.Message, RetryAttempts - retries + 1);
            }
            catch (SmtpFailedRecipientsException failedRecipientsException)
            {
                var failedrecipients =
                    failedRecipientsException
                    .InnerExceptions
                    .Aggregate(new StringBuilder(), (current, next) => current.Append(", ").Append(next))
                    .ToString();

                return new SendResult(false, string.Format("Failed Recipients: {0} - {1}", failedRecipientsException.StatusCode, failedrecipients), RetryAttempts - retries + 1);
            }
            catch (SmtpException smtpException)
            {
                //only retry 3 times
                if(smtpException.StatusCode == SmtpStatusCode.MailboxBusy && retries > 0)
                {
                    return Send(mailMessage, --retries);
                }

                return new SendResult(false, string.Format("SMTP Exception {0}", smtpException.StatusCode), RetryAttempts - retries + 1);
            }

            return new SendResult(true, "Success", RetryAttempts - retries + 1);
        }
    }
}