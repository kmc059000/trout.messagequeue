namespace trout.messagequeue.config
{
    /// <summary>
    /// Configuration values for the file system based attachment file system
    /// </summary>
    public interface IFileSystemAttachmentHandlerConfig
    {
        /// <summary>
        /// Path for storing attachments
        /// </summary>
        string AttachmentPath { get; }

        /// <summary>
        /// Path for storage
        /// </summary>
        string StoragePath { get; }
    }
}