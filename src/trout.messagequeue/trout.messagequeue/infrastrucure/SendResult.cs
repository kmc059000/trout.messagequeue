using System;

namespace trout.messagequeue.infrastrucure
{
    public class SendResult
    {
        public readonly bool IsSuccess;
        public readonly String Message;
        public readonly int Tries;

        public SendResult(bool isSuccess, string message, int tries)
        {
            IsSuccess = isSuccess;
            Tries = tries;
            Message = message;
        }
    }
}