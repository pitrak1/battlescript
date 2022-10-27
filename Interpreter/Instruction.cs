using Newtonsoft.Json;

namespace BattleScript; 

public class Instruction {
    [JsonProperty("type")]
    public Consts.InstructionTypes? Type { get; set; }
    
    [JsonProperty("operator")]
    public string? Operator { get; set; }
    
    [JsonProperty("left")]
    public Instruction? Left { get; set; }
    
    [JsonProperty("right")]
    public Instruction? Right { get; set; }
    
    [JsonProperty("integerValue")]
    public int? IntegerValue { get; set; }
    
    [JsonProperty("stringValue")]
    public string? StringValue { get; set; }
    
    [JsonProperty("boolValue")]
    public bool? BoolValue { get; set; }
    
    [JsonProperty("instructionValue")]
    public Instruction? InstructionValue { get; set; }
    
    [JsonProperty("instructionListValue")]
    public List<Instruction>? InstructionListValue { get; set; } = new List<Instruction>();
    
    [JsonProperty("line")]
    public int? Line { get; set; }
    
    [JsonProperty("column")]
    public int? Column { get; set; }
    
    [JsonProperty("instructions")]
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    
    [JsonProperty("next")]
    public Instruction? Next { get; set; }
}

