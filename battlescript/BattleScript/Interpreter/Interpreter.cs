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
    ContextStack ongoingContexts;

    CustomCallbacks callbacks;

    ScopeVariable btlContext;

    List<Instruction> instructions;

    public Interpreter(List<Instruction> _instructions)
    {
        instructions = _instructions;
        ongoingContexts = new ContextStack();
        lexicalContexts = new ScopeStack();
        callbacks = new CustomCallbacks();
        btlContext = new ScopeVariable(
            Consts.VariableTypes.Dictionary,
            new Dictionary<dynamic, ScopeVariable>()
        );
    }

    public ScopeStack Run(List<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
        {
            interpretInstruction(instruction);
        }

        return lexicalContexts;
    }

    private ScopeVariable interpretInstruction(Instruction instruction)
    {
        switch (instruction.Type)
        {
            case Consts.InstructionTypes.Assignment:
                return handleAssignment(instruction);
            case Consts.InstructionTypes.Number:
            case Consts.InstructionTypes.String:
            case Consts.InstructionTypes.Boolean:
                return handleLiteral(instruction);
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

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }

    private ScopeVariable handleAssignment(Instruction instruction)
    {
        ScopeVariable left = interpretInstruction(instruction.Left!);
        ScopeVariable right = interpretInstruction(instruction.Right!);
        return left.CopyProperties(right);
    }

    private ScopeVariable handleLiteral(Instruction instruction)
    {
        return new ScopeVariable(Consts.VariableTypes.Literal, instruction.Value);
    }

    private ScopeVariable handleDeclaration(Instruction instruction)
    {
        Debug.Assert(instruction.Value is string, "Variables must be declared as strings");
        List<string> path = new List<string>() { instruction.Value };
        return lexicalContexts.AddVariableToCurrentScope(path);
    }

    private ScopeVariable handleVariable(Instruction instruction)
    {
        ScopeVariable var = lexicalContexts.GetVariable(instruction.Value);
        if (instruction.Next is not null)
        {
            ongoingContexts.Add(var);
            ScopeVariable result = interpretInstruction(instruction.Next);
            ongoingContexts.Pop();
            return result;
        }
        return var;
    }

    private ScopeVariable handleOperation(Instruction instruction)
    {
        ScopeVariable left = interpretInstruction(instruction.Left!);
        ScopeVariable right = interpretInstruction(instruction.Right!);

        dynamic? result;
        switch (instruction.Value)
        {
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

        return new ScopeVariable(Consts.VariableTypes.Literal, result);
    }

    private ScopeVariable handleSquareBraces(Instruction instruction)
    {
        if (ongoingContexts.IsEmpty())
        {
            List<ScopeVariable> initializationEntries = new List<ScopeVariable>();

            foreach (Instruction inst in instruction.Value)
            {
                ScopeVariable result = interpretInstruction(inst);
                initializationEntries.Add(result);
            }

            return new ScopeVariable(Consts.VariableTypes.Array, initializationEntries);
        }
        else
        {
            ScopeVariable index = interpretInstruction(instruction.Value![0]);

            Debug.Assert(!ongoingContexts.IsEmpty(), "Expected to have a non-null indexed value");
            ScopeVariable indexed = ongoingContexts.GetCurrentContext();

            ScopeVariable result = indexed.GetIndex(index.Value);

            return result;
        }
    }

    private ScopeVariable handleDictionary(Instruction instruction)
    {
        Dictionary<dynamic, ScopeVariable> entries = new Dictionary<dynamic, ScopeVariable>();

        for (int i = 0; i < instruction.Value.Count; i = i + 2)
        {
            ScopeVariable key = interpretInstruction(instruction.Value[i]);
            ScopeVariable value = interpretInstruction(instruction.Value[i + 1]);
            entries.Add(key.Value, value);
        }

        return new ScopeVariable(Consts.VariableTypes.Dictionary, entries);
    }

    private ScopeVariable handleIf(Instruction instruction)
    {
        ScopeVariable condition = interpretInstruction(instruction.Value);
        if (Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
        }
        else if (instruction.Next is not null)
        {
            interpretInstruction(instruction.Next);
        }

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }

    private ScopeVariable handleElse(Instruction instruction)
    {
        ScopeVariable condition = instruction.Value is not null ?
            interpretInstruction(instruction.Value) :
            new ScopeVariable(Consts.VariableTypes.Literal);


        if (instruction.Value is null || Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
        }
        else if (instruction.Next is not null)
        {
            interpretInstruction(instruction.Next);
        }
        return new ScopeVariable(Consts.VariableTypes.Literal);
    }

    private ScopeVariable handleWhile(Instruction instruction)
    {
        ScopeVariable condition = interpretInstruction(instruction.Value);
        while (Utilities.variableIsTruthy(condition))
        {
            runCodeBlock(instruction.Instructions);
            condition = interpretInstruction(instruction.Value);
        }

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }

    private ScopeVariable handleFunction(Instruction instruction)
    {
        List<ScopeVariable> args = new List<ScopeVariable>();

        foreach (Instruction inst in instructions)
        {
            args.Add(new ScopeVariable(Consts.VariableTypes.Literal, inst.Value));
        }

        return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions);
    }

    private ScopeVariable handleParens(Instruction instruction)
    {
        Debug.Assert(!ongoingContexts.IsEmpty());

        ScopeVariable called = ongoingContexts.GetCurrentContext();

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

                lexicalContexts.AddNewScope();

                for (int i = 0; i < instruction.Value.Count; i++)
                {
                    string argName = called.Value[i].Value!;
                    ScopeVariable argValue = interpretInstruction(instruction.Value[i]);
                    lexicalContexts.AddVariableToCurrentScope(new List<string>() { argName }, argValue);
                }

                foreach (Instruction insideBlockInstruction in instructions)
                {
                    interpretInstruction(insideBlockInstruction);
                }

                ScopeVariable functionScope = lexicalContexts.Pop();
                ScopeVariable returnValue = new ScopeVariable(Consts.VariableTypes.Literal);

                if (functionScope.HasVariable("return"))
                {
                    returnValue = functionScope.GetVariable("return");
                }

                return returnValue;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }

    private ScopeVariable handleReturn(Instruction instruction)
    {
        ScopeVariable result = interpretInstruction(instruction.Value);
        lexicalContexts.AddVariableToCurrentScope(new List<string>() { "return" }, result);
        return result;
    }

    private ScopeVariable handleBtl(Instruction instruction)
    {
        Debug.Assert(instruction.Next is not null);
        Debug.Assert(instruction.Next.Type == Consts.InstructionTypes.SquareBraces);

        ScopeVariable var;
        string indexValue = instruction.Next.Value[0].Value;
        if (indexValue == "context")
        {
            var = btlContext;

            if (instruction.Next is not null)
            {
                ongoingContexts.Add(var);
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
            var = new ScopeVariable(Consts.VariableTypes.Literal, returnValue);

            if (instruction.Next.Next.Next is not null)
            {
                ongoingContexts.Add(var);
                var = interpretInstruction(instruction.Next.Next.Next);
                ongoingContexts.Pop();
            }
        }

        return var;
    }

    public void runCodeBlock(List<Instruction> instructions)
    {
        lexicalContexts.AddNewScope();
        foreach (Instruction insideBlockInstruction in instructions)
        {
            interpretInstruction(insideBlockInstruction);
        }
        lexicalContexts.Pop();
    }
}