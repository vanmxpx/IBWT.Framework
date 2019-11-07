using System.Collections.Concurrent;
using System.Collections.Generic;

namespace IBWT.Framework.Services
{
    public class StateCache
    {
        private readonly Dictionary<int, ConcurrentStack<string>> _stateList = new Dictionary<int, ConcurrentStack<string>>(); // ConcurrentDictionary

        
    }
}