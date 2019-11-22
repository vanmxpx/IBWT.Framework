using System;
using System.Collections.Concurrent;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace IBWT.Framework.State
{
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class StateContext
    {   
        public const string Default = "default";
        public const string Back = "back";
        public ConcurrentStack<string> History { get; } = new ConcurrentStack<string>();
        public string TopCommand { get; private set; } 

        [JsonProperty(Required = Required.Always)]
        public long Id { get; private set; }

        public StateContext(long id)
        {
            Id = id;
            History.Push(Default);
            TopCommand = Default;
        }

        public void ApplyCommand(string command)
        {
            if(command.Equals(Back))
                StepBack();
            else
                StepForward(command);
        }

        public string StepForward(string command) 
        {
            if(command != this.TopCommand)
                History.Push(command);
            return TopCommand = command;
        }

        public string StepBack() 
        {
            if(History.Count < 2)
                return TopCommand;
            if(!History.TryPop(out string declinedCommand))
                throw new InvalidOperationException($"Error on POP. Cannot step back in state object with Id = {Id} and history = {HistoryAsList()}");
            if(!History.TryPeek(out string topCommand))
                throw new InvalidOperationException($"Error on PEEK. Cannot step back in state object with Id = {Id} and history = {HistoryAsList()}");
            return TopCommand = topCommand;
        }

        public string HistoryAsList()
        {
           return History.Select(x => x).Aggregate((s1, s2) => s1 + "::" + s2);
        }

        public string Serialize()
        {
           return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        public static StateContext Deserialize(string json)
        {
           return JsonConvert.DeserializeObject<StateContext>(json);
        }
    }
}