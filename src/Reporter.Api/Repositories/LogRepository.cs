using System.Collections.Generic;

namespace Reporter.Api.Repositories
{
    public class LogRepository : ILogRepository
    {
        private static readonly ISet<string> _logs = new HashSet<string>();
        public ISet<string> Logs => _logs;
    }
}