using System;

namespace Reporter.Messages.Events
{
    public class CreateReportRejected : IEvent
    {
        public Guid Id { get; }
        public string Reason { get; }

        protected CreateReportRejected()
        {
        }

        public CreateReportRejected(Guid id, string reason)
        {
            Id = id;
            Reason = reason;
        }
    }
}