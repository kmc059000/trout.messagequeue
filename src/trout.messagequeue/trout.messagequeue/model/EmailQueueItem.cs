using System;

namespace trout.messagequeue.model
{

    /// <summary>
    /// 
    /// </summary>
    public class EmailQueueItem
    {
        /// <summary>
        /// ID.
        /// </summary>
        /// <value>
        /// ID.
        /// </value>
        public int ID { get; set; }
       
        /// <summary>
        /// To address
        /// </summary>
        /// <value>
        /// To address
        /// </value>
        public string To { get; set; }
        /// <summary>
        /// cc address
        /// </summary>
        /// <value>
        /// cc address
        /// </value>
        public string Cc { get; set; }
        /// <summary>
        /// BCC address
        /// </summary>
        /// <value>
        /// BCC address
        /// </value>
        public string Bcc { get; set; }
        /// <summary>
        /// subject.
        /// </summary>
        /// <value>
        /// subject.
        /// </value>
        public string Subject { get; set; }
        /// <summary>
        /// body.
        /// </summary>
        /// <value>
        /// body.
        /// </value>
        public string Body { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the email is sent.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is sent; otherwise, <c>false</c>.
        /// </value>
        public bool IsSent { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has failed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has failed; otherwise, <c>false</c>.
        /// </value>
        public bool IsFailed { get; set; }
        /// <summary>
        /// number of send tries.
        /// </summary>
        /// <value>
        /// number of send tries.
        /// </value>
        public byte NumberTries { get; set; }
        /// <summary>
        /// create date.
        /// </summary>
        /// <value>
        /// create date.
        /// </value>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// last send try date.
        /// </summary>
        /// <value>
        /// last send try date.
        /// </value>
        public DateTime? LastTryDate { get; set; }
        /// <summary>
        /// send date.
        /// </summary>
        /// <value>
        /// send date.
        /// </value>
        public DateTime? SendDate { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance's body is HTML.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance's body is HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsBodyHtml { get; set; }
        /// <summary>
        /// attachment count.
        /// </summary>
        /// <value>
        /// attachment count.
        /// </value>
        public byte AttachmentCount { get; set; }
    }
}