using System.Threading;
using System.Threading.Tasks;

namespace IBWT.Framework.Scheduler
{
    public interface IScheduledTask
    {
        string Schedule { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}