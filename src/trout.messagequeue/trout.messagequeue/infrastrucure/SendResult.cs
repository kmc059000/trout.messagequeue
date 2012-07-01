using System;

namespace trout.messagequeue.infrastrucure
{
    public class SendResult
    {
        public bool IsSuccess { get; private set; }
        public String Message { get; private set; }

        public SendResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}