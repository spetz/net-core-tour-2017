using System;

namespace Reporter.Messages.Events
{
    public class ReportCreated : IEvent
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Text { get; }

        protected ReportCreated()
        {
        }

        public ReportCreated(Guid id, string name, string text)
        {
            Id = id;
            Name = name;
            Text = text;
        }
    }
}