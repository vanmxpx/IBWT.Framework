using System.Threading;
using System.Threading.Tasks;

namespace IBWT.Framework.Abstractions
{
    public delegate Task UpdateDelegate(IUpdateContext context, CancellationToken cancellationToken = default);
}