using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;

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
        return lexicalContexts.AddVariable(path);
    }

    private ScopeVariable HandleVariable(Instruction instruction) {
        ScopeVariable var = lexicalContexts.GetVariable(instruction.Value);
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

    private ScopeVariable HandleIf(Instruction instruction) {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        if (InterpreterUtilities.isTruthy(condition)) {
            lexicalContexts.Add(new ScopeVariable());
            foreach (Instruction ifInstruction in instruction.Instructions) {
                InterpretInstruction(ifInstruction);
            }
            lexicalContexts.Pop();
        }
        else if (instruction.Next is not null) {
            InterpretInstruction(instruction.Next);
        }
        
        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleElse(Instruction instruction) {
        ScopeVariable condition = new ScopeVariable();
        if (instruction.Value is not null) {
            condition = InterpretInstruction(instruction.Value);
        }

        if (instruction.Value is null || InterpreterUtilities.isTruthy(condition)) {
            lexicalContexts.Add(new ScopeVariable());
            foreach (Instruction elseInstruction in instruction.Instructions) {
                InterpretInstruction(elseInstruction);
            }
            lexicalContexts.Pop();
        } else if (instruction.Next is not null) {
            InterpretInstruction(instruction.Next);
        }
        return new ScopeVariable(Consts.VariableTypes.Value);
    }

    private ScopeVariable HandleWhile(Instruction instruction) {
        ScopeVariable condition = InterpretInstruction(instruction.Value);
        while (InterpreterUtilities.isTruthy(condition)) {
            lexicalContexts.Add(new ScopeVariable());
            foreach (Instruction ifInstruction in instruction.Instructions) {
                InterpretInstruction(ifInstruction);
            }
            lexicalContexts.Pop();
            condition = InterpretInstruction(instruction.Value);
        }

        return new ScopeVariable(Consts.VariableTypes.Value);
    }
    
    private ScopeVariable HandleFunction(Instruction instruction) {
        List<ScopeVariable> args = new List<ScopeVariable>();
        foreach (Instruction inst in instruction.Value) {
            args.Add(new ScopeVariable(Consts.VariableTypes.Value, inst.Value));
        }
        return new ScopeVariable(Consts.VariableTypes.Function, args, instruction.Instructions);
    }
    
    private ScopeVariable HandleParens(Instruction instruction) {
        Debug.Assert(!ongoingContexts.Empty());

        ScopeVariable called = ongoingContexts.GetCurrentContext();
        
        switch (called.Type) {
            case Consts.VariableTypes.Function:
                ScopeVariable functionScope = RunFunction(called.Value, called.Instructions, instruction.Value);
                ScopeVariable? returnValue = new ScopeVariable(Consts.VariableTypes.Value);
                if (functionScope.HasVariable("return")) {
                    returnValue = functionScope.GetVariable("return");
                }
                return returnValue;
            case Consts.VariableTypes.Class:
                ScopeVariable objectScope = new ScopeVariable();
                objectScope.Copy(called);
                objectScope.Type = Consts.VariableTypes.Object;
                return objectScope;
            default:
                throw new SystemException("Invalid instruction type to call");
        }
    }

    private ScopeVariable HandleReturn(Instruction instruction) {
        ScopeVariable result = InterpretInstruction(instruction.Value);
        lexicalContexts.AddVariable(new List<string>() { "return" }, result);
        return result;
    }
    
    private ScopeVariable HandleClass(Instruction instruction) {
        ScopeVariable resultingScope = RunFunction(
            new List<ScopeVariable>(), 
            instruction.Instructions, 
            new List<Instruction>()
        );
        return new ScopeVariable(Consts.VariableTypes.Class, resultingScope.Value);
    }

    private ScopeVariable RunFunction(List<ScopeVariable> functionValue, List<Instruction> instructions, List<Instruction> args) {
        lexicalContexts.Add(new ScopeVariable());
        
        if (args.Count != functionValue.Count) {
            throw new WrongNumberOfArgumentsException(args.Count, functionValue.Count);
        }

        for (int i = 0; i < args.Count; i++) {
            string argName = functionValue[i].Value;
            ScopeVariable argValue = InterpretInstruction(args[i]);
            lexicalContexts.AddVariable(new List<string>() { argName }, argValue);
        }
        
        foreach (Instruction funcInstruction in instructions) {
            InterpretInstruction(funcInstruction);
        }

        return lexicalContexts.Pop();
    }
}