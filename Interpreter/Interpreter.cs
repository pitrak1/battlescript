using System.Diagnostics;

namespace BattleScript; 

public class Interpreter {
    public static ScopeStack Run(List<Instruction> instructions) {
        ScopeStack scopeStack = new ScopeStack();
        
        foreach (Instruction instruction in instructions) {
            InterpretInstruction(instruction, scopeStack);
        }

        return scopeStack;
    }

    private static ScopeVariable InterpretInstruction(Instruction instruction, ScopeStack scopeStack, bool leftSide = false) {
        switch (instruction.Type) {
            case Consts.InstructionTypes.Assignment:
                return HandleAssignment(instruction, scopeStack, leftSide);
            case Consts.InstructionTypes.Number: 
            case Consts.InstructionTypes.String: 
            case Consts.InstructionTypes.Boolean:
                return HandleValueType(instruction, scopeStack, leftSide);
            case Consts.InstructionTypes.Declaration:
                return HandleDeclaration(instruction, scopeStack, leftSide);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction, scopeStack, leftSide);
        }

        return new ScopeVariable();
    }

    private static ScopeVariable HandleAssignment(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        ScopeVariable left = InterpretInstruction(instruction.Left, scopeStack, true);
        ScopeVariable right = InterpretInstruction(instruction.Right, scopeStack);
        left.Copy(right);
        return left;
    }
    
    private static ScopeVariable HandleValueType(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        return new ScopeVariable(Consts.VariableTypes.Value, instruction.Value);
    }
    
    private static ScopeVariable HandleDeclaration(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        Debug.Assert(leftSide);
        Debug.Assert(instruction.Value is string);
        List<string> path = new List<string>() { instruction.Value };
        return scopeStack.Add(path);
    }

    private static ScopeVariable HandleVariable(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        return scopeStack.Get(instruction.Value);
    }
}