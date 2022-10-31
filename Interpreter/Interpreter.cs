using System.Diagnostics;

namespace BattleScript; 

public class Interpreter {
    public ScopeStack lexicalContexts { get; set; }
    public ContextStack ongoingContexts { get; set; }

    public Interpreter() {
        lexicalContexts = new ScopeStack();
        ongoingContexts = new ContextStack();
    }

    public ScopeStack Run(List<Instruction> instructions) {
        foreach (Instruction instruction in instructions) {
            InterpretInstruction(instruction);
        }

        return lexicalContexts;
    }

    private ScopeVariable InterpretInstruction(Instruction instruction) {
        switch (instruction.Type) {
            case Consts.InstructionTypes.Assignment:
                return HandleAssignment(instruction);
            case Consts.InstructionTypes.Number: 
            case Consts.InstructionTypes.String: 
            case Consts.InstructionTypes.Boolean:
                return HandleValueType(instruction);
            case Consts.InstructionTypes.Declaration:
                return HandleDeclaration(instruction);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return HandleOperation(instruction);
            case Consts.InstructionTypes.SquareBraces:
                return HandleSquareBraces(instruction);
            case Consts.InstructionTypes.Dictionary:
                return HandleDictionary(instruction);
        }

        return new ScopeVariable();
    }

    private ScopeVariable HandleAssignment(Instruction instruction) {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);
        left.Copy(right);
        return left;
    }
    
    private ScopeVariable HandleValueType(Instruction instruction) {
        return new ScopeVariable(Consts.VariableTypes.Value, instruction.Value);
    }
    
    private ScopeVariable HandleDeclaration(Instruction instruction) {
        Debug.Assert(instruction.Value is string);
        List<string> path = new List<string>() { instruction.Value };
        return lexicalContexts.Add(path);
    }

    private ScopeVariable HandleVariable(Instruction instruction) {
        ScopeVariable var = lexicalContexts.Get(instruction.Value);
        if (instruction.Next is not null) {
            ongoingContexts.Add(var);
            var = InterpretInstruction(instruction.Next);
            ongoingContexts.Pop();
        }
        return var;
    }
    
    private ScopeVariable HandleOperation(Instruction instruction) {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);
        
        Debug.Assert(left.Value is int);
        Debug.Assert(right.Value is int);

        dynamic? result;
        switch (instruction.Value) {
            case "==":
                result = left.Value == right.Value;
                break;
            case "<":
                result = left.Value < right.Value;
                break;
            case ">":
                result = left.Value > right.Value;
                break;
            case "+":
                result = left.Value + right.Value;
                break;
            case "*":
                result = left.Value * right.Value;
                break;
            default:
                throw new SystemException("Invalid operator");
        }

        return new ScopeVariable(Consts.VariableTypes.Value, result);
    }

    private ScopeVariable HandleSquareBraces(Instruction instruction) {
        if (ongoingContexts.Empty()) {
            // Handle array
            List<ScopeVariable> entries = new List<ScopeVariable>();
            foreach (Instruction entryInstruction in instruction.Value) {
                ScopeVariable entryResult = InterpretInstruction(entryInstruction);
                entries.Add(entryResult);
            }
            return new ScopeVariable(Consts.VariableTypes.Array, entries);
        }
        else {
            // Handle index
            ScopeVariable index = InterpretInstruction(instruction.Value[0]);
            return ongoingContexts.GetCurrentContext().Value[index.Value];
        }
    }

    private ScopeVariable HandleDictionary(Instruction instruction) {
        Dictionary<dynamic, ScopeVariable> entries = new Dictionary<dynamic, ScopeVariable>();
        
        for (int i = 0; i < instruction.Value.Count; i = i + 2) {
            ScopeVariable key = InterpretInstruction(instruction.Value[i]);
            ScopeVariable value = InterpretInstruction(instruction.Value[i + 1]);
            entries.Add(key.Value, value);
        }

        return new ScopeVariable(Consts.VariableTypes.Dictionary, entries);
    }
}