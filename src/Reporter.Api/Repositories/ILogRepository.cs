using System.Collections.Generic;

namespace Reporter.Api.Repositories
{
    public interface ILogRepository
    {
         ISet<string> Logs { get; }
    }
}