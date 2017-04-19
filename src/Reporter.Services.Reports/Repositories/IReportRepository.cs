using System.Collections.Generic;
using Reporter.Services.Reports.Models;

namespace Reporter.Services.Reports.Repositories
{
    public interface IReportRepository
    {
        ISet<Report> Reports { get; }
    }
}