using System;

namespace Reporter.Services.Reports.Models
{
    public class Report
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
    }
}