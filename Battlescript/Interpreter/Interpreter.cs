namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
    private Stack<Variable> _ongoingContextsStack = [];
    private List<Instruction> _instructions = instructions;
    private int _instructionIndex = 0;
    private Memory _memory = new ();

    public List<Dictionary<string, Variable>> Run()
    {
        while (_instructionIndex < _instructions.Count)
        {
            InterpretInstruction(_instructions[_instructionIndex]);
            _instructionIndex++;
        }

        // This should always be the base scope and have only one entry, but at some point we're going
        // to add breakpoints, and then that will change
        return _memory.GetScopes();
    }

    private Variable InterpretInstruction(Instruction instruction)
    {
        switch (instruction.Type)
        {
            case Consts.InstructionTypes.Assignment:
                return HandleAssignment(instruction);
            case Consts.InstructionTypes.Number:
                return new Variable(Consts.VariableTypes.Number, instruction.Value);
            case Consts.InstructionTypes.String:
                return new Variable(Consts.VariableTypes.String, instruction.Value);
            case Consts.InstructionTypes.Boolean:
                return new Variable(Consts.VariableTypes.Boolean, instruction.Value);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return HandleOperation(instruction);
            default:
                throw new Exception($"Unknown instruction type: {instruction.Type}");
        }
    }

    private Variable HandleAssignment(Instruction instruction)
    {
        var left = InterpretInstruction(instruction.Left!);
        var right = InterpretInstruction(instruction.Right!);
        return left.Copy(right);
    }

    private Variable HandleVariable(Instruction instruction)
    {
        var variable = _memory.GetAndCreateIfNotExists(instruction.Value);
        return ReturnVariableOrInterpretNext(variable, instruction.Next);
    }

    private Variable HandleOperation(Instruction instruction)
    {
        var left = InterpretInstruction(instruction.Left!);
        var right = InterpretInstruction(instruction.Right!);

        dynamic? result;
        Consts.VariableTypes type;
        switch (instruction.Value)
        {
            case "==":
                result = left.Value == right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "<":
                result = left.Value < right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case ">":
                result = left.Value > right.Value;
                type = Consts.VariableTypes.Boolean;
                break;
            case "+":
                result = left.Value + right.Value;
                type = Consts.VariableTypes.Number;
                break;
            case "*":
                result = left.Value * right.Value;
                type = Consts.VariableTypes.Number;
                break;
            default:
                throw new SystemException("Invalid operator");
        }
        
        return new Variable(type, result);
    }

    private Variable ReturnVariableOrInterpretNext(Variable variable, Instruction? next)
    {
        if (next is not null)
        {
            _ongoingContextsStack.Push(variable);
            var result = InterpretInstruction(next);
            _ongoingContextsStack.Pop();
            return result;
        }
        return variable;
    }
}