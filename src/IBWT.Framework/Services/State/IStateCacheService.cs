using System.Threading.Tasks;
using IBWT.Framework.Abstractions;

namespace IBWT.Framework.Services.State
{
    public interface IStateCacheService
    {
        void CacheContext(IUpdateContext updateContext);
        Task UpdateState(IUpdateContext updateContext, string state, string data = null);
        Task InitUpdate(IUpdateContext updateContext);
    }
}