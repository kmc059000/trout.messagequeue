using System.Net.Mail;
using trout.messagequeue.model;

namespace trout.messagequeue.attachments
{
    public interface IAttachmentFileSystem
    {
        void SaveAttachments(EmailQueueItem item, MailMessage mailMessage);
        Attachment[] GetAttachments(EmailQueueItem item);
    }
}