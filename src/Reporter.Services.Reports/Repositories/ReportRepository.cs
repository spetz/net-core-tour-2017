using System.Collections.Generic;
using Reporter.Services.Reports.Models;

namespace Reporter.Services.Reports.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private static readonly ISet<Report> _reports = new HashSet<Report>();
        public ISet<Report> Reports => _reports;
    }
}