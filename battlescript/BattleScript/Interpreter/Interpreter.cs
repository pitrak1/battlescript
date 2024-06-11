using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;

namespace BattleScript.Core;

public partial class Interpreter
{
    /*
     * This is to keep track of the hierarchy of code blocks available to the current function.
     * This is altered when:
     * - when a new block is created for an if/else/while or a function/class definition
     * - when a class method is called (so that variables in the class will be in scope even without using the self keyword)
     */
    ScopeStack lexicalContexts;

    /*
     * This is to keep track of instructions that are analyzed in parts, keeping the value from the previous part
     * This has to be a stack, not a single value, because of expressions like these: function_1(function_2());
     * If this were just a value, the context of function_2 would overwrite the context of function_1.
     * This is altered when:
     * - Instructions are executed that contain multiple parts using separators (parens, dots, indexes, curly braces)
     */
    Stack<Variable> ongoingContexts;

    CustomCallbacks callbacks;

    Variable btlContext;

    List<Instruction> instructions;

    public Interpreter(List<Instruction> _instructions)
    {
        instructions = _instructions;
        ongoingContexts = new Stack<Variable>();
        lexicalContexts = new ScopeStack();
        callbacks = new CustomCallbacks();
        btlContext = new Variable(
            Consts.VariableTypes.Dictionary,
            new Dictionary<dynamic, Variable>()
        );
    }

    public Dictionary<string, Variable> Run()
    {
        foreach (Instruction instruction in instructions)
        {
            interpretInstruction(instruction);
        }

        return lexicalContexts.Peek();
    }

    private Variable interpretInstruction(Instruction instruction)
    {
        switch (instruction.Type)
        {
            case Consts.InstructionTypes.Assignment:
                return handleAssignment(instruction);
            case Consts.InstructionTypes.Number:
                return new Variable(Consts.VariableTypes.Number, instruction.Value);
            case Consts.InstructionTypes.String:
                return new Variable(Consts.VariableTypes.String, instruction.Value);
            case Consts.InstructionTypes.Boolean:
                return new Variable(Consts.VariableTypes.Boolean, instruction.Value);
            case Consts.InstructionTypes.Declaration:
                return handleDeclaration(instruction);
            case Consts.InstructionTypes.Variable:
                return handleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return handleOperation(instruction);
            case Consts.InstructionTypes.SquareBraces:
                return handleSquareBraces(instruction);
            case Consts.InstructionTypes.Dictionary:
                return handleDictionary(instruction);
            case Consts.InstructionTypes.If:
                return handleIf(instruction);
            case Consts.InstructionTypes.Else:
                return handleElse(instruction);
            case Consts.InstructionTypes.While:
                return handleWhile(instruction);
            case Consts.InstructionTypes.Function:
                return handleFunction(instruction);
            case Consts.InstructionTypes.Parens:
                return handleParens(instruction);
            case Consts.InstructionTypes.Return:
                return handleReturn(instruction);
            case Consts.InstructionTypes.Btl:
                return handleBtl(instruction);
        }

        return new Variable();
    }

    private Variable handleAssignment(Instruction instruction)
    {
        Variable left = interpretInstruction(instruction.Left!);
        Variable right = interpretInstruction(instruction.Right!);
        return left.CopyProperties(right);
    }

    private Variable handleDeclaration(Instruction instruction)
    {
        Debug.Assert(instruction.Value is string, "Variables must be declared as strings");
        Variable var = new Variable();
        lexicalContexts.Peek().Add(instruction.Value, var);
        return var;
    }

    private Variable handleVariable(Instruction instruction)
    {
        Variable var = lexicalContexts.GetVariable(instruction.Value);
        if (instruction.Next is not null)
        {
            ongoingContexts.Push(var);
            Variable result = interpretInstruction(instruction.Next);
            ongoingContexts.Pop();
            return result;
        }
        return var;
    }

    private Variable handleOperation(Instruction instruction)
    {
        Variable left = interpretInstruction(instruction.Left!);
        Variable right = interpretInstruction(instruction.Right!);

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

    private Variable handleSquareBraces(Instruction instruction)
    {
        if (ongoingContexts.Count == 0)
        {
            List<Variable> initializationEntries = new List<Variable>();

            foreach (Instruction inst in instruction.Value)
            {
                Variable result = interpretInstruction(inst);
                initializationEntries.Add(result);
            }

            return new Variable(Consts.VariableTypes.Array, initializationEntries);
        }
        else
        {
            Variable index = interpretInstruction(instruction.Value![0]);

            Debug.Assert(ongoingContexts.Count != 0, "Expected to have a non-null indexed value");
            Variable indexed = ongoingContexts.Peek();

            Variable result = indexed.GetIndex(index.Value);

            return result;
        }
    }

    private Variable handleDictionary(Instruction instruction)
    {
        Dictionary<dynamic, Variable> entries = new Dictionary<dynamic, Variable>();

        for (int i = 0; i < instruction.Value.Count; i = i + 2)
        {
            Variable key = interpretInstruction(instruction.Value[i]);
            Variable value = interpretInstruction(instruction.Value[i + 1]);
            entries.Add(key.Value, value);
        }

        return new Variable(Consts.VariableTypes.Dictionary, entries);
    }

    private Variable handleIf(Instruction instruction)
    {
        Variable condition = interpretInstruction(instruction.Value);
        if (Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
        }
        else if (instruction.Next is not null)
        {
            interpretInstruction(instruction.Next);
        }

        return new Variable();
    }

    private Variable handleElse(Instruction instruction)
    {
        Variable condition = instruction.Value is not null ?
            interpretInstruction(instruction.Value) :
            new Variable();


        if (instruction.Value is null || Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
        }
        else if (instruction.Next is not null)
        {
            interpretInstruction(instruction.Next);
        }
        return new Variable();
    }

    private Variable handleWhile(Instruction instruction)
    {
        Variable condition = interpretInstruction(instruction.Value);
        while (Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
            condition = interpretInstruction(instruction.Value);
        }

        return new Variable();
    }

    private Variable handleFunction(Instruction instruction)
    {
        List<Variable> args = new List<Variable>();

        foreach (Instruction arg in instruction.Value)
        {
            args.Add(new Variable(null, arg.Value));
        }

        return new Variable(Consts.VariableTypes.Function, args, instruction.Instructions);
    }

    private Variable handleParens(Instruction instruction)
    {
        Debug.Assert(ongoingContexts.Count != 0);

        Variable called = ongoingContexts.Peek();

        switch (called.Type)
        {
            case Consts.VariableTypes.Function:
                if (instruction.Value.Count != called.Value.Count)
                {
                    throw new WrongNumberOfArgumentsException(
                        instruction.Value.Count,
                        called.Value.Count
                    );
                }

                lexicalContexts.Push(new Dictionary<string, Variable>());

                for (int i = 0; i < instruction.Value.Count; i++)
                {
                    string argName = called.Value[i].Value!;
                    Variable argValue = interpretInstruction(instruction.Value[i]);
                    lexicalContexts.Peek().Add(argName, new Variable().CopyProperties(argValue));
                }

                foreach (Instruction insideBlockInstruction in called.Instructions)
                {
                    interpretInstruction(insideBlockInstruction);
                }

                Dictionary<string, Variable> functionScope = lexicalContexts.Pop();
                Variable returnValue = new Variable();

                if (functionScope.ContainsKey("return"))
                {
                    returnValue = functionScope["return"];
                }

                return returnValue;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }

    private Variable handleReturn(Instruction instruction)
    {
        Variable result = interpretInstruction(instruction.Value);
        lexicalContexts.Peek().Add("return", result);
        return result;
    }

    private Variable handleBtl(Instruction instruction)
    {
        Debug.Assert(instruction.Next is not null);
        Debug.Assert(instruction.Next.Type == Consts.InstructionTypes.SquareBraces);

        Variable var;
        string indexValue = instruction.Next.Value[0].Value;
        if (indexValue == "context")
        {
            var = btlContext;

            if (instruction.Next is not null)
            {
                ongoingContexts.Push(var);
                var = interpretInstruction(instruction.Next.Next);
                ongoingContexts.Pop();
            }
        }
        else
        {
            List<dynamic> args = new List<dynamic>();
            if (instruction.Next.Next.Type == Consts.InstructionTypes.Parens)
            {
                foreach (Instruction inst in instruction.Next.Next.Value)
                {
                    args.Add(interpretInstruction(inst).Value);
                }
            }
            dynamic returnValue = callbacks.Run(indexValue, args);
            var = new Variable(Consts.VariableTypes.Number, returnValue);

            if (instruction.Next.Next.Next is not null)
            {
                ongoingContexts.Push(var);
                var = interpretInstruction(instruction.Next.Next.Next);
                ongoingContexts.Pop();
            }
        }

        return var;
    }

    public void runCodeBlock(List<Instruction> instructions)
    {
        lexicalContexts.Push(new Dictionary<string, Variable>());
        foreach (Instruction insideBlockInstruction in instructions)
        {
            interpretInstruction(insideBlockInstruction);
        }
        lexicalContexts.Pop();
    }
}