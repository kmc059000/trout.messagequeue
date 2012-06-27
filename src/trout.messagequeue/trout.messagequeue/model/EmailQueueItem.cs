﻿using System;

namespace trout.emailservice.model
{
    public class EmailQueueItem
    {
        public int ID { get; set; }
        public string To { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsSent { get; set; }
        public bool IsFailed { get; set; }
        public byte NumberTries { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? LastTryDate { get; set; }
        public DateTime? SendDate { get; set; }
    }
}