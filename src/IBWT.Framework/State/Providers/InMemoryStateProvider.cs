using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IBWT.Framework.State.Providers
{
    public class InMemoryStateProvider : IStateProvider
    {
        private readonly ConcurrentDictionary<long, StateContext> _stateList = new ConcurrentDictionary<long, StateContext>();

        public bool Clear(long id)
        {
            return _stateList.Remove(id, out StateContext state);
        }

        public void ClearAll()
        {
            _stateList.Clear();
        }

        public StateContext GetState(long id) 
        {
            _stateList.TryGetValue(id, out StateContext state);

            if(state == null)
            {
                state = new StateContext(id);
                if(!_stateList.TryAdd(id, state))
                    throw new InvalidOperationException($"Error on adding new State. Id = {id}");
            }
            return state;
        }
    }
}