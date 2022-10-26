namespace BattleScript; 

public class Instruction {
    public Consts.InstructionTypes? Type { get; set; }
    public string? Operator { get; set; }
    public dynamic? Left { get; set; }
    public dynamic? Right { get; set; }
    public dynamic? Value { get; set; }
    public int? Line { get; set; }
    public int? Column { get; set; }
    public List<Instruction> Instructions { get; set; } = new List<Instruction>();
    public Instruction? Next { get; set; }

    public Instruction SetAssignmentValues(Consts.InstructionTypes type, dynamic left, dynamic right) {
        Type = type;
        Left = left;
        Right = right;
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

        return a.Type == b.Type && a.Value == b.Value && a.Operator == b.Operator && a.Left == b.Left && a.Right == b.Right;
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

        return a.Type != b.Type || a.Value != b.Value || a.Operator != b.Operator || a.Left != b.Left || a.Right != b.Right;
    }
}

