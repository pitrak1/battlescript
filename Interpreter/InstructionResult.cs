using Newtonsoft.Json;

namespace BattleScript; 

public class InstructionResult {
    [JsonProperty("variableValue")]
    public ScopeVariable? VariableValue { get; set; }
}