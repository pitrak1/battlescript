using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;

namespace BattleScript.Core;

public class Interpreter
{
    /*
     * This is to keep track of instructions that are analyzed in parts, keeping the value from the previous part
     * This has to be a stack, not a single value, because of expressions like these: function_1(function_2());
     * If this were just a value, the context of function_2 would overwrite the context of function_1.
     * This is altered when:
     * - Instructions are executed that contain multiple parts using separators (parens, dots, indexes, curly braces)
     */
    public ContextStack OngoingContexts { get; set; }

    /*
     * This is to keep track of the hierarchy of code blocks available to the current function.
     * This is altered when:
     * - when a new block is created for an if/else/while or a function/class definition
     * - when a class method is called (so that variables in the class will be in scope even without using the self keyword)
     */
    public ScopeStack LexicalContexts { get; set; }

    /*
     * This is to keep track of the current value of self.
     * This is altered when:
     * - An object variable is found with a next instruction
     * - Super is used to keep track of the object that's changing
     */
    public ContextStack SelfContexts { get; set; }

    /*
     * This is to keep track of the current class being defined or the class context that a method is being called in
     * This is altered when:
     * - a class is defined
     * - a class method is called
     * - When we use super so we know what our anchor class is
     */
    public ContextStack ClassContexts { get; set; }

    public CustomCallbacks Callbacks { get; set; }

    public ScopeVariable BtlContext { get; set; }

    public Interpreter()
    {
        OngoingContexts = new ContextStack();
        LexicalContexts = new ScopeStack();
        SelfContexts = new ContextStack();
        ClassContexts = new ContextStack();
        Callbacks = new CustomCallbacks();
        BtlContext = new ScopeVariable(Consts.VariableTypes.Dictionary, new Dictionary<dynamic, ScopeVariable>());
    }

    public ScopeStack Run(List<Instruction> instructions)
    {
        foreach (Instruction instruction in instructions)
        {
            InterpretInstruction(instruction);
        }

        return LexicalContexts;
    }

    private ScopeVariable InterpretInstruction(Instruction instruction)
    {
        switch (instruction.Type)
        {
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
            case Consts.InstructionTypes.If:
                return HandleIf(instruction);
            case Consts.InstructionTypes.Else:
                return HandleElse(instruction);
            case Consts.InstructionTypes.While:
                return HandleWhile(instruction);
            case Consts.InstructionTypes.Function:
                return HandleFunction(instruction);
            case Consts.InstructionTypes.Parens:
                return HandleParens(instruction);
            case Consts.InstructionTypes.Return:
                return HandleReturn(instruction);
            case Consts.InstructionTypes.Class:
                return HandleClass(instruction);
            case Consts.InstructionTypes.Self:
                return HandleSelf(instruction);
            case Consts.InstructionTypes.Super:
                return HandleSuper(instruction);
            case Consts.InstructionTypes.Constructor:
                return HandleConstructor(instruction);
            case Consts.InstructionTypes.Btl:
                return HandleBtl(instruction);
        }

        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleAssignment(Instruction instruction)
    {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);
        return left.Copy(right);
    }

    private ScopeVariable HandleValueType(Instruction instruction)
    {
        return new ScopeVariable(Consts.VariableTypes.Value, instruction.Value);
    }

    private ScopeVariable HandleDeclaration(Instruction instruction)
    {
        Debug.Assert(instruction.Value is string);
        List<string> path = new List<string>() { instruction.Value };
        return LexicalContexts.AddVariable(path);
    }

    private ScopeVariable HandleVariable(Instruction instruction)
    {
        ScopeVariable var = LexicalContexts.GetVariable(instruction.Value);
        if (instruction.Next is not null)
        {
            OngoingContexts.Add(var);
            if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Add(var); }
            ScopeVariable result = InterpretInstruction(instruction.Next);
            if (var.Type == Consts.VariableTypes.Object) { SelfContexts.Pop(); }
            OngoingContexts.Pop();
            var = result;
        }
        return var;
    }

    private ScopeVariable HandleOperation(Instruction instruction)
    {
        ScopeVariable left = InterpretInstruction(instruction.Left);
        ScopeVariable right = InterpretInstruction(instruction.Right);

        Debug.Assert(left.Value is int);
        Debug.Assert(right.Value is int);

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

        return new ScopeVariable(Consts.VariableTypes.Value, result);
    }

    private ScopeVariable HandleSquareBraces(Instruction instruction)
    {
        if (OngoingContexts.Empty())
        {
            // Handle array
            List<ScopeVariable> entries = new List<ScopeVariable>();
            foreach (Instruction entryInstruction in instruction.Value)
            {
                ScopeVariable entryResult = InterpretInstruction(entryInstruction);
                entries.Add(entryResult);
            }
            return new ScopeVariable(Consts.VariableTypes.Array, entries);
        }
        else
        {
            // Handle index
            ScopeVariable index = InterpretInstruction(instruction.Value[0]);
            ScopeVariable indexed = OngoingContexts.GetCurrentContext();
            ScopeVariable var = indexed.GetIndex(index.Value);

            if (index.Value is string && index.Value == "super")
            {
                ClassContexts.Add(OngoingContexts.GetCurrentContext());
                ScopeVariable result = HandleSuper(instruction);
                ClassContexts.Pop();
                return result;
            }

            if (instruction.Next is not null)
            {
                OngoingContexts.SetCurrentContext(var);
                var = InterpretInstruction(instruction.Next);
            }

            return var;
        }
    }

    private ScopeVariable HandleDictionary(Instruction instruction)
    {
        Dictionary<dynamic, ScopeVariable> entries = new Dictionary<dynamic, ScopeVariable>();

        for (int i = 0; i < instruction.Value.Count; i = i + 2)
        {
            ScopeVariable key = InterpretInstruction(instruction.Value[i]);
            ScopeVariable value = InterpretInstruction(instruction.Value[i + 1]);
            entries.Add(key.Value, value);
        }

        return new ScopeVariable(Consts.VariableTypes.Dictionary, entries);
    }

    private ScopeVariable HandleIf(Instruction instruction)
    {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        if (InterpreterUtilities.isTruthy(condition))
        {
            LexicalContexts.Add();
            foreach (Instruction ifInstruction in instruction.Instructions)
            {
                InterpretInstruction(ifInstruction);
            }
            LexicalContexts.Pop();
        }
        else if (instruction.Next is not null)
        {
            InterpretInstruction(instruction.Next);
        }

        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleElse(Instruction instruction)
    {
        ScopeVariable condition = new ScopeVariable(Consts.VariableTypes.Value);
        if (instruction.Value is not null)
        {
            condition = InterpretInstruction(instruction.Value);
        }

        if (instruction.Value is null || InterpreterUtilities.isTruthy(condition))
        {
            LexicalContexts.Add();
            foreach (Instruction elseInstruction in instruction.Instructions)
            {
                InterpretInstruction(elseInstruction);
            }
            LexicalContexts.Pop();
        }
        else if (instruction.Next is not null)
        {
            InterpretInstruction(instruction.Next);
        }
        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleWhile(Instruction instruction)
    {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        while (InterpreterUtilities.isTruthy(condition))
        {
            LexicalContexts.Add();
            foreach (Instruction ifInstruction in instruction.Instructions)
            {
                InterpretInstruction(ifInstruction);
            }
            LexicalContexts.Pop();
            condition = InterpretInstruction(instruction.Value);
        }

        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleFunction(Instruction instruction)
    {
        List<ScopeVariable> args = new List<ScopeVariable>();
        foreach (Instruction inst in instruction.Value)
        {
            args.Add(new ScopeVariable(Consts.VariableTypes.Value, inst.Value));
        }

        ScopeVariable? classObject = null;
        if (!ClassContexts.Empty())
        {
            classObject = ClassContexts.GetCurrentContext();
        }
        return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions, classObject);
    }

    private ScopeVariable HandleParens(Instruction instruction)
    {
        Debug.Assert(!OngoingContexts.Empty());

        ScopeVariable called = OngoingContexts.GetCurrentContext();

        switch (called.Type)
        {
            case Consts.VariableTypes.Function:
                if (!SelfContexts.Empty())
                {
                    LexicalContexts.Add(SelfContexts.GetCurrentContext());
                }

                ScopeVariable functionScope = RunFunction(called.Value, called.Instructions, instruction.Value, called.ClassObject);

                ScopeVariable? returnValue = new ScopeVariable(Consts.VariableTypes.Value);
                if (functionScope.HasVariable("return"))
                {
                    returnValue = functionScope.GetVariable("return");
                }

                if (!SelfContexts.Empty())
                {
                    LexicalContexts.Pop();
                }
                return returnValue;
            case Consts.VariableTypes.Class:
                ScopeVariable createdObject = null;
                ScopeVariable constructor = null;
                if (SelfContexts.Empty())
                {
                    createdObject = new ScopeVariable().CreateObject(called);
                    constructor = createdObject.ClassObject.GetConstructorForClass();
                }
                else
                {
                    createdObject = SelfContexts.GetCurrentContext();
                    constructor = OngoingContexts.GetCurrentContext().GetConstructorForClass();
                }
                if (constructor is not null)
                {
                    SelfContexts.Add(createdObject);
                    LexicalContexts.Add(createdObject);
                    RunFunction(constructor.Value, constructor.Instructions, instruction.Value, constructor.ClassObject);
                    SelfContexts.Pop();
                    LexicalContexts.Pop();
                }
                return createdObject;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }

    private ScopeVariable HandleReturn(Instruction instruction)
    {
        ScopeVariable result = InterpretInstruction(instruction.Value);
        LexicalContexts.AddVariable(new List<string>() { "return" }, result);
        return result;
    }

    private ScopeVariable HandleClass(Instruction instruction)
    {
        ScopeVariable var = new ScopeVariable(Consts.VariableTypes.Class);

        ClassContexts.Add(var);
        ScopeVariable resultingScope = RunFunction(
            new List<ScopeVariable>(),
            instruction.Instructions,
            new List<Instruction>()
        );
        ClassContexts.Pop();

        var.Value = resultingScope.Value;
        if (instruction.Value is not null)
        {
            var.Value.Add("super", InterpretInstruction(instruction.Value));
        }

        return var;
    }

    private ScopeVariable HandleSelf(Instruction instruction)
    {
        ScopeVariable var = SelfContexts.GetCurrentContext();
        if (instruction.Next is not null)
        {
            OngoingContexts.Add(var);
            var = InterpretInstruction(instruction.Next);
            OngoingContexts.Pop();
        }
        return var;
    }

    private ScopeVariable HandleSuper(Instruction instruction)
    {
        ScopeVariable classObject = ClassContexts.GetCurrentContext();
        ScopeVariable superObject = classObject.GetVariable("super");

        if (instruction.Next is not null)
        {
            OngoingContexts.Add(superObject);
            superObject = InterpretInstruction(instruction.Next);
            OngoingContexts.Pop();
        }
        return superObject;
    }

    private ScopeVariable HandleConstructor(Instruction instruction)
    {
        List<ScopeVariable> args = new List<ScopeVariable>();
        foreach (Instruction inst in instruction.Value)
        {
            args.Add(new ScopeVariable(Consts.VariableTypes.Value, inst.Value));
        }

        ScopeVariable? classObject = null;
        if (!ClassContexts.Empty())
        {
            classObject = ClassContexts.GetCurrentContext();
        }

        List<string> path = new List<string>() { "constructor" };
        ScopeVariable left = LexicalContexts.AddVariable(path);
        return left.Copy(new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions, classObject));
    }

    private ScopeVariable HandleBtl(Instruction instruction)
    {
        Debug.Assert(instruction.Next is not null);
        Debug.Assert(instruction.Next.Type == Consts.InstructionTypes.SquareBraces);

        ScopeVariable var;
        string indexValue = instruction.Next.Value[0].Value;
        if (indexValue == "context")
        {
            var = BtlContext;

            if (instruction.Next is not null)
            {
                OngoingContexts.Add(var);
                var = InterpretInstruction(instruction.Next.Next);
                OngoingContexts.Pop();
            }
        }
        else
        {
            List<dynamic> args = new List<dynamic>();
            if (instruction.Next.Next.Type == Consts.InstructionTypes.Parens)
            {
                foreach (Instruction inst in instruction.Next.Next.Value)
                {
                    args.Add(InterpretInstruction(inst).Value);
                }
            }
            dynamic returnValue = Callbacks.Run(indexValue, args);
            var = new ScopeVariable(Consts.VariableTypes.Value, returnValue);

            if (instruction.Next.Next.Next is not null)
            {
                OngoingContexts.Add(var);
                var = InterpretInstruction(instruction.Next.Next.Next);
                OngoingContexts.Pop();
            }
        }

        return var;
    }

    private ScopeVariable RunFunction(List<ScopeVariable> functionValue, List<Instruction> instructions, List<Instruction> args, ScopeVariable? classObject = null)
    {
        LexicalContexts.Add();

        if (args.Count != functionValue.Count)
        {
            throw new WrongNumberOfArgumentsException(args.Count, functionValue.Count);
        }

        if (classObject != null)
        {
            ClassContexts.Add(classObject);
        }

        for (int i = 0; i < args.Count; i++)
        {
            string argName = functionValue[i].Value;
            ScopeVariable argValue = InterpretInstruction(args[i]);
            LexicalContexts.AddVariable(new List<string>() { argName }, argValue);
        }

        foreach (Instruction funcInstruction in instructions)
        {
            InterpretInstruction(funcInstruction);
        }

        if (classObject != null)
        {
            ClassContexts.Pop();
        }

        return LexicalContexts.Pop();
    }
}