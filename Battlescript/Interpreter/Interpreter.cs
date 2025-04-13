namespace Battlescript;

public class Interpreter(List<Instruction> instructions)
{
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

    private Variable InterpretInstruction(Instruction? instruction, Variable? context = null)
    {
        if (instruction is null)
        {
            return new Variable(Consts.VariableTypes.Null, null);
        }
        
        switch (instruction.Type)
        {
            case Consts.InstructionTypes.Assignment:
                return HandleAssignment(instruction);
            case Consts.InstructionTypes.Number:
                return new Variable(Consts.VariableTypes.Number, instruction.LiteralValue);
            case Consts.InstructionTypes.String:
                return new Variable(Consts.VariableTypes.String, instruction.LiteralValue);
            case Consts.InstructionTypes.Boolean:
                return new Variable(Consts.VariableTypes.Boolean, instruction.LiteralValue);
            case Consts.InstructionTypes.If:
                return HandleIf(instruction);
            case Consts.InstructionTypes.While:
                return HandleWhile(instruction);
            case Consts.InstructionTypes.Function:
                return HandleFunction(instruction);
            case Consts.InstructionTypes.Return:
                return HandleReturn(instruction);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return HandleOperation(instruction);
            case Consts.InstructionTypes.SquareBrackets:
                return HandleSquareBrackets(instruction, context);
            case Consts.InstructionTypes.Parens:
                return HandleParens(instruction, context);
            case Consts.InstructionTypes.SetDefinition:
                return HandleSetDefinition(instruction);
            case Consts.InstructionTypes.DictionaryDefinition:
                return HandleDictionaryDefinition(instruction);
            case Consts.InstructionTypes.Class:
                return HandleClassDefinition(instruction);
            default:
                throw new Exception($"Unknown instruction type: {instruction.Type}");
        }
    }

    private Variable HandleAssignment(Instruction instruction)
    {
        var left = InterpretInstruction(instruction.Left!);
        var right = InterpretInstruction(instruction.Right!);

        var result = InterpreterUtilities.ConductAssignment(instruction, left, right);
        return left.Copy(result);
    }

    private Variable HandleIf(Instruction instruction)
    {
        var condition = InterpretInstruction(instruction.Value);
        if (InterpreterUtilities.IsVariableTruthy(condition))
        {
            _memory.AddScope();
            InterpretList(instruction.Instructions);
            _memory.RemoveScope();
        }

        return new Variable(Consts.VariableTypes.Null, null);
    }

    private Variable HandleWhile(Instruction instruction)
    {
        var condition = InterpretInstruction(instruction.Value);
        // This will likely eventually be a function to determine if something is "truthy"
        while (InterpreterUtilities.IsVariableTruthy(condition))
        {
            _memory.AddScope();
            InterpretList(instruction.Instructions);
            _memory.RemoveScope();
            condition = InterpretInstruction(instruction.Value);
        }

        return new Variable(Consts.VariableTypes.Null, null);
    }
    
    private Variable HandleFunction(Instruction instruction)
    {
        var result = new Variable(
            Consts.VariableTypes.Function, 
            ParseFunctionDefinitionParameters(instruction.ValueList), 
            instruction.Instructions);
        var variable = _memory.GetAndCreateIfNotExists(instruction.Name);
        return variable.Copy(result);
    }

    private Variable HandleReturn(Instruction instruction)
    {
        var returnVariable = _memory.GetAndCreateIfNotExists("return");
        var returnValue = InterpretInstruction(instruction.Value);
        return returnVariable.Copy(returnValue);
    }

    private Variable HandleVariable(Instruction instruction)
    {
        var variable = _memory.GetAndCreateIfNotExists(instruction.Name);
        return instruction.Next is not null ? InterpretInstruction(instruction.Next, variable) : variable;
    }

    private Variable HandleOperation(Instruction instruction)
    {
        var left = InterpretInstruction(instruction.Left!);
        var right = InterpretInstruction(instruction.Right!);
        return InterpreterUtilities.ConductOperation(instruction, left, right);
    }

    private Variable HandleSquareBrackets(Instruction instruction, Variable? context)
    {
        if (context is not null)
        {
            if (instruction.ValueList.Count > 1)
            {
                throw new Exception("Too many index values");
            }

            if (instruction.ValueList[0].Type == Consts.InstructionTypes.KeyValuePair)
            {
                var leftIndex = InterpretInstruction(instruction.ValueList[0].Left);
                var rightIndex = InterpretInstruction(instruction.ValueList[0].Right);
                var result = context.GetRangeIndex((int?)leftIndex.Value, (int?)rightIndex.Value);
                return instruction.Next is not null ? InterpretInstruction(instruction.Next, result) : result;
            }
            else
            {
                var index = InterpretInstruction(instruction.ValueList[0]);
                var result =  context.GetIndex(index.Value);
                return instruction.Next is not null ? InterpretInstruction(instruction.Next, result) : result;
            }
        }
        else
        {
            var values = new List<Variable>();
            // If we got here, that means that the value of the instruction should be a list of instructions
            foreach (var instValue in instruction.ValueList)
            {
                values.Add(InterpretInstruction(instValue));
            }
            return new Variable(Consts.VariableTypes.List, values);
        }
    }
    
    private Variable HandleParens(Instruction instruction, Variable? context)
    {
        if (context is not null)
        {
            if (context.Type == Consts.VariableTypes.Function)
            {
                _memory.AddScope();
                TransferFunctionArguments(instruction, context);
                InterpretList(context.Instructions);
                var returnValue = _memory.Get("return");
                _memory.RemoveScope();
                return returnValue ?? new Variable(Consts.VariableTypes.Null, null);
            }
            else
            {
                return new Variable(Consts.VariableTypes.Object, context.Value);
            }
            
        }
        else
        {
            return new Variable(Consts.VariableTypes.Tuple, InterpretList(instruction.ValueList));
        }
    }

    private Variable HandleSetDefinition(Instruction instruction)
    {
        return new Variable(Consts.VariableTypes.Set, InterpretList(instruction.ValueList));
    }
    
    private Variable HandleDictionaryDefinition(Instruction instruction)
    {
        return new Variable(Consts.VariableTypes.Dictionary, InterpretKvpList(instruction.ValueList));
    }

    private Variable HandleClassDefinition(Instruction instruction)
    {
        List<Variable>? superclasses = null;
        if (instruction.ValueList.Count > 0)
        {
            superclasses = InterpretList(instruction.ValueList);
        }
        _memory.AddScope();
        InterpretList(instruction.Instructions);
        var classScope = _memory.RemoveScope();
        var classVariable = new Variable(Consts.VariableTypes.Class, classScope, null, superclasses);
        _memory.Set(instruction.Name, classVariable);
        return classVariable;
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
    
    private Dictionary<string, Variable> InterpretKvpList(List<Instruction> instructions)
    {
        var values = new Dictionary<string, Variable>();
        foreach (var kvp in instructions)
        {
            var key = InterpretInstruction(kvp.Left);
            var value = InterpretInstruction(kvp.Right);
            values[key.Value] = value;
        }
        return values;
    }

    private List<Instruction> ParseFunctionDefinitionParameters(List<Instruction> parameters)
    {
        var results = new List<Instruction>();
        foreach (var p in parameters)
        {
            if (p.Type != Consts.InstructionTypes.Variable)
            {
                // ERROR HERE
            }
            
            results.Add(p);
        }

        return results;
    }

    private void TransferFunctionArguments(Instruction instruction, Variable context)
    {
        var arguments = InterpretList(instruction.ValueList);
        for (int i = 0; i < context.Value.Count; i++)
        {
            var variable = _memory.GetAndCreateIfNotExists(context.Value[i].Name);
            var value = arguments[i];
            variable.Copy(value);
        }
    }
}