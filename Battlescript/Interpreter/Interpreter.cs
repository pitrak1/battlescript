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
            case Consts.InstructionTypes.If:
                return HandleIf(instruction);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return HandleOperation(instruction);
            case Consts.InstructionTypes.SquareBrackets:
                return HandleSquareBrackets(instruction);
            case Consts.InstructionTypes.Parens:
                return HandleParens(instruction);
            case Consts.InstructionTypes.SetDefinition:
                return HandleSetDefinition(instruction);
            case Consts.InstructionTypes.DictionaryDefinition:
                return HandleDictionaryDefinition(instruction);
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

    private Variable HandleIf(Instruction instruction)
    {
        var condition = InterpretInstruction(instruction.Value);
        // This will likely eventually be a function to determine if something is "truthy"
        if (condition.Type == Consts.VariableTypes.Boolean &&
            condition.Value is true)
        {
            _memory.AddScope();
            InterpretList(instruction.Instructions);
            _memory.RemoveScope();
        }

        return new Variable(Consts.VariableTypes.Null, null);
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

    private Variable HandleSquareBrackets(Instruction instruction)
    {
        if (_ongoingContextsStack.Count > 0)
        {
            // This means that it's trying to get a member of a list/dict/etc, not create a list.
            // On the TODO list
            return new Variable(0, 0);
        }
        else
        {
            var values = new List<Variable>();
            // If we got here, that means that the value of the instruction should be a list of instructions
            foreach (var instValue in instruction.Value)
            {
                values.Add(InterpretInstruction(instValue));
            }
            return new Variable(Consts.VariableTypes.List, values);
        }
    }
    
    private Variable HandleParens(Instruction instruction)
    {
        if (_ongoingContextsStack.Count > 0)
        {
            // This means that it's trying to call a function, not create a tuple
            // The parser doesn't currently support that correctly
            return new Variable(0, 0);
        }
        else
        {
            return new Variable(Consts.VariableTypes.Tuple, InterpretList(instruction.Value));
        }
    }

    private Variable HandleSetDefinition(Instruction instruction)
    {
        return new Variable(Consts.VariableTypes.Set, InterpretList(instruction.Value));
    }
    
    private Variable HandleDictionaryDefinition(Instruction instruction)
    {
        return new Variable(Consts.VariableTypes.Dictionary, InterpretKvpList(instruction.Value));
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

    private List<Variable> InterpretList(List<Instruction> instructions)
    {
        var values = new List<Variable>();
        foreach (var instValue in instructions)
        {
            values.Add(InterpretInstruction(instValue));
        }
        return values;
    }
    
    private Dictionary<string, Variable> InterpretKvpList(List<(Instruction Key, Instruction Value)> instructions)
    {
        var values = new Dictionary<string, Variable>();
        foreach (var instValue in instructions)
        {
            var key = InterpretInstruction(instValue.Key);
            var value = InterpretInstruction(instValue.Value);
            values[key.Value] = value;
        }
        return values;
    }
}