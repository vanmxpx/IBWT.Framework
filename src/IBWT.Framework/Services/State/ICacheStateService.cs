using IBWT.Framework.Abstractions;

namespace IBWT.Framework.Services.State
{
    public interface IStateCacheService
    {
        void CacheContext(IUpdateContext updateContext);
    }
}