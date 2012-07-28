using System.Net.Mail;

namespace trout.messagequeue.smtp
{
    /// <summary>
    /// Interface which sends MailMessages
    /// </summary>
    public interface ISmtpClient
    {
        /// <summary>
        /// Attempts to send the MailMessage
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        SendResult Send(MailMessage mailMessage);
    }
}