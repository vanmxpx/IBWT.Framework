using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using IBWT.Framework.Abstractions;
using IBWT.Framework.State;
using IBWT.Framework.State.Providers;

namespace IBWT.Framework.Services.State
{
    public class StateCacheService<TStateCache> : IStateCacheService
    where TStateCache : IStateProvider, new()
    {
        private UpdateDelegate _updateDelegate;
        public StateCacheService(
            IBotBuilder botBuilder
        )
        {
            _updateDelegate = botBuilder.Build();
        }
        TStateCache _stateCache = new TStateCache();

        public async Task UpdateState(IUpdateContext updateContext, string state, string data = null)
        {
            var chatId = updateContext.Update.GetChatId();
            if (chatId.HasValue)
            {
                StateContext currentState = _stateCache.GetState(chatId.Value);
                currentState.ApplyCommand(state);
                updateContext.Items["History"] = currentState.HistoryAsList();
                updateContext.Items["State"] = currentState.TopCommand;
                updateContext.Items["Data"] = data;
                await _updateDelegate(updateContext)
                        .ConfigureAwait(false);
            }
        }
        public void CacheContext(IUpdateContext updateContext)
        {
            var chatId = updateContext.Update.GetChatId();
            if (chatId.HasValue)
            {
                StateContext currentState = _stateCache.GetState(chatId.Value);
                if (updateContext.Update.CallbackQuery != null)
                {
                    string[] parts = updateContext.Update.CallbackQuery.Data.Split("::");

                    if (parts.Length == 0)
                        throw new ArgumentException("Invalid CallbackQuery state - button must contain data with state id ended with ::");

                    currentState.ApplyCommand(parts[0]);

                    if (parts.Length > 1)
                        updateContext.Items.Add("Data", parts[1]);
                }
                updateContext.Items.Add("History", currentState.HistoryAsList());
                updateContext.Items.Add("State", currentState.TopCommand);
            }

        }
    }
}