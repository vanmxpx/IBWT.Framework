using System;
using System.Collections.Concurrent;
using IBWT.Framework.Abstractions;
using IBWT.Framework.State;
using IBWT.Framework.State.Providers;

namespace IBWT.Framework.Services.State
{
    public class StateCacheService<TStateCache> : IStateCacheService 
    where TStateCache : IStateProvider, new()
    {
        TStateCache _stateCache = new TStateCache();

        public void CacheContext(IUpdateContext updateContext)
        {
            StateContext currentState = _stateCache.GetState(updateContext.Update.GetChatId());

            if(updateContext.Update.CallbackQuery != null)
            {
                string[] parts = updateContext.Update.CallbackQuery.Data.Split("::");

                if (parts.Length == 0)
                    throw new ArgumentException("Invalid CallbackQuery state - button must contain data with state id ended with ::");

                currentState.ApplyCommand(parts[0]);
            }
            updateContext.Items.Add("History", currentState.HistoryAsList());
            updateContext.Items.Add("State", currentState.TopCommand);            
        }
    }
}