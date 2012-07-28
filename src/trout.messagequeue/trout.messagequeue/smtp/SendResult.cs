using System;

namespace trout.messagequeue.smtp
{
    /// <summary>
    /// Result of a MailMessage send attempt
    /// </summary>
    public sealed class SendResult
    {
        /// <summary>
        /// Whether the send attempt was successful
        /// </summary>
        public readonly bool IsSuccess;

        /// <summary>
        /// String message of the send attempt results
        /// </summary>
        public readonly String Message;

        /// <summary>
        /// Number of tries that were required to send the email
        /// </summary>
        public readonly int Tries;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        /// <param name="tries"></param>
        public SendResult(bool isSuccess, string message, int tries)
        {
            IsSuccess = isSuccess;
            Tries = tries;
            Message = message;
        }
    }
}