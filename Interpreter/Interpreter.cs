namespace BattleScript; 

public class Interpreter {
    public static ScopeStack Run(List<Instruction> instructions) {
        ScopeStack scopeStack = new ScopeStack();
        
        foreach (Instruction instruction in instructions) {
            InterpretInstruction(instruction, scopeStack);
        }
    }

    private static InstructionResult InterpretInstruction(Instruction instruction, ScopeStack scopeStack, bool leftSide = false) {
        switch (instruction.Type) {
            case Consts.InstructionTypes.Assignment:
                return HandleAssignment(instruction, scopeStack, leftSide);
            case Consts.InstructionTypes.Number:
                return HandleNumber(instruction, scopeStack, leftSide);
            // case Consts.InstructionTypes.String:
            //     return HandleString(instruction, scopeStack, leftSide);
            // case Consts.InstructionTypes.Boolean:
            //     return HandleBoolean(instruction, scopeStack, leftSide);
            // case Consts.InstructionTypes.Declaration:
            //     return HandleDeclaration(instruction, scopeStack, leftSide);
            // case Consts.InstructionTypes.Variable:
            //     return HandleVariable(instruction, scopeStack, leftSide);
        }
    }

    private static InstructionResult HandleAssignment(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        InstructionResult left = InterpretInstruction(instruction.Left, scopeStack, true);
        InstructionResult right = InterpretInstruction(instruction.Right, scopeStack);
    }
    
    private static InstructionResult HandleNumber(Instruction instruction, ScopeStack scopeStack, bool leftSide) {
        ScopeVariable variable = new ScopeVariable();
        variable.IntegerValue = instruction.IntegerValue;
        
        InstructionResult result = new InstructionResult();
        result.VariableValue = variable;
        return result;
    }
}