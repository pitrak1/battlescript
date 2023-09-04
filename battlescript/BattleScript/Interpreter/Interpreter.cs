using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
    /*
     * This is to keep track of the hierarchy of code blocks available to the current function.
     * This is altered when:
     * - when a new block is created for an if/else/while or a function/class definition
     * - when a class method is called (so that variables in the class will be in scope even without using the self keyword)
     */
    public ScopeStack LexicalContexts { get; set; }

    /*
     * This is to keep track of instructions that are analyzed in parts, keeping the value from the previous part
     * This has to be a stack, not a single value, because of expressions like these: function_1(function_2());
     * If this were just a value, the context of function_2 would overwrite the context of function_1.
     * This is altered when:
     * - Instructions are executed that contain multiple parts using separators (parens, dots, indexes, curly braces)
     */
    public ContextStack OngoingContexts { get; set; }

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
                return HandleLiteral(instruction);
            case Consts.InstructionTypes.Declaration:
                return HandleDeclaration(instruction);
            case Consts.InstructionTypes.Variable:
                return HandleVariable(instruction);
            case Consts.InstructionTypes.Operation:
                return HandleOperation(instruction);
                // case Consts.InstructionTypes.SquareBraces:
                //     return HandleSquareBraces(instruction);
                // case Consts.InstructionTypes.Dictionary:
                //     return HandleDictionary(instruction);
                // case Consts.InstructionTypes.If:
                //     return HandleIf(instruction);
                // case Consts.InstructionTypes.Else:
                //     return HandleElse(instruction);
                // case Consts.InstructionTypes.While:
                //     return HandleWhile(instruction);
                // case Consts.InstructionTypes.Function:
                //     return HandleFunction(instruction);
                // case Consts.InstructionTypes.Parens:
                //     return HandleParens(instruction);
                // case Consts.InstructionTypes.Return:
                //     return HandleReturn(instruction);
                // case Consts.InstructionTypes.Class:
                //     return HandleClass(instruction);
                // case Consts.InstructionTypes.Self:
                //     return HandleSelf(instruction);
                // case Consts.InstructionTypes.Super:
                //     return HandleSuper(instruction);
                // case Consts.InstructionTypes.Constructor:
                //     return HandleConstructor(instruction);
                // case Consts.InstructionTypes.Btl:
                //     return HandleBtl(instruction);
        }

        return new ScopeVariable(Consts.VariableTypes.Literal);
    }
}