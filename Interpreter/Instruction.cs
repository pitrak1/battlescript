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
    
    [JsonProperty("value")]
    public dynamic? Value { get; set; }
    
    [JsonProperty("line")]
    public int? Line { get; set; }
    
    [JsonProperty("column")]
    public int? Column { get; set; }
    
    [JsonProperty("instructions")]
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    
    [JsonProperty("next")]
    public Instruction? Next { get; set; }

    public Instruction SetAssignmentValues(Consts.InstructionTypes type, dynamic left, dynamic right) {
        Type = type;
        Left = left;
        Right = right;
        return this;
    }

    public Instruction SetOperationValues(Consts.InstructionTypes type, dynamic left, dynamic right, string value) {
        Type = type;
        Left = left;
        Right = right;
        Value = value;
        return this;
    }

    public Instruction SetVariableValues(Consts.InstructionTypes type, dynamic value) {
        Type = type;
        Value = value;
        return this;
    }

    public Instruction SetDebugValues(int? line, int? column) {
        Line = line;
        Column = column;
        return this;
    }
    
    public static bool operator ==(Instruction a, Instruction b) {
        if (a.Instructions.Count == b.Instructions.Count) {
            for (int i = 0; i < a.Instructions.Count; i++) {
                if (a.Instructions[i] != b.Instructions[i]) {
                    return false;
                }
            }
        }
        else {
            return false;
        }

        if (a.Next is null != b.Next is null) {
            return false;
        }
        else if (a.Next is not null && b.Next is not null && a.Next != b.Next) {
            return false;
        }
        
        if (a.Left is null != b.Left is null) {
            return false;
        }
        else if (a.Left is not null && b.Left is not null && a.Left != b.Left) {
            return false;
        }
        
        if (a.Right is null != b.Right is null) {
            return false;
        }
        else if (a.Right is not null && b.Right is not null && a.Right != b.Right) {
            return false;
        }

        return a.Type == b.Type && a.Value == b.Value && a.Operator == b.Operator;
    }

    public static bool operator !=(Instruction a, Instruction b) {
        if (a.Instructions.Count == b.Instructions.Count) {
            for (int i = 0; i < a.Instructions.Count; i++) {
                if (a.Instructions[i] != b.Instructions[i]) {
                    return true;
                }
            }
        }
        else {
            return true;
        }
        
        if (a.Next is null != b.Next is null) {
            return true;
        }
        else if (a.Next is not null && b.Next is not null && a.Next != b.Next) {
            return true;
        }
        
        if (a.Left is null != b.Left is null) {
            return true;
        }
        else if (a.Left is not null && b.Left is not null && a.Left != b.Left) {
            return true;
        }
        
        if (a.Right is null != b.Right is null) {
            return true;
        }
        else if (a.Right is not null && b.Right is not null && a.Right != b.Right) {
            return true;
        }

        return a.Type != b.Type || a.Value != b.Value || a.Operator != b.Operator;
    }
}

