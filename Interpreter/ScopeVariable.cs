using Newtonsoft.Json;

namespace BattleScript; 

public class ScopeVariable {
    [JsonProperty("integerValue")]
    public int? IntegerValue { get; set; }
}