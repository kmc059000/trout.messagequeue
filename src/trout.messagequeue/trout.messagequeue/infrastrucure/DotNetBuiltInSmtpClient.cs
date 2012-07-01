using System;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace trout.emailservice.infrastrucure
{
    public class DotNetBuiltInSmtpClient : ISmtpClient
    {
        readonly System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();

        public SendResult Send(MailMessage mailMessage)
        {
            return Send(mailMessage, 3);
        }

        private SendResult Send(MailMessage mailMessage, int retries)
        {
            try
            {
                client.Send(mailMessage);
            }
            catch(ArgumentNullException)
            {
                return new SendResult(false, string.Format("Message is null")); 
            }
            catch (ObjectDisposedException)
            {
                return new SendResult(false, "Client Disposed");
            }
            catch (InvalidOperationException invalidOperationException)
            {
                if(mailMessage.From == null)
                {
                    return new SendResult(false, "From is null");
                }

                if (mailMessage.To.Count == 0 && mailMessage.To.Count == 0 && mailMessage.To.Count == 0)
                {
                    return new SendResult(false, "No recipients");
                }

                
                if (client.DeliveryMethod == SmtpDeliveryMethod.Network && string.IsNullOrEmpty(client.Host))
                {
                    return new SendResult(false, "Host is not set");
                }

                if (client.DeliveryMethod == SmtpDeliveryMethod.Network && (client.Port <= 0 || client.Port >65535))
                {
                    return new SendResult(false, string.Format("{0} port is invalid", client.Port));
                }

                return new SendResult(false, "Invalid Operation: " + invalidOperationException.Message);
            }
            catch (SmtpFailedRecipientsException failedRecipientsException)
            {
                var failedrecipients =
                    failedRecipientsException
                    .InnerExceptions
                    .Aggregate(new StringBuilder(), (current, next) => current.Append(", ").Append(next))
                    .ToString();

                return new SendResult(false, string.Format("Failed Recipients: {0} - {1}", failedRecipientsException.StatusCode, failedrecipients));
            }
            catch (SmtpException smtpException)
            {
                //only retry 3 times
                if(smtpException.StatusCode == SmtpStatusCode.MailboxBusy && retries > 0)
                {
                    return Send(mailMessage, --retries);
                }

                return new SendResult(false, string.Format("SMTP Exception {0}", smtpException.StatusCode));
            }

            return new SendResult(true, "Success");
        }
    }
}