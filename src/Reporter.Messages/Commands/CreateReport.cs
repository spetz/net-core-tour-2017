using System;

namespace Reporter.Messages.Commands
{
    public class CreateReport : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}