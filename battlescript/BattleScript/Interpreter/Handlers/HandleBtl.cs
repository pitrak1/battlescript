using System.Diagnostics;
using System.Security.Cryptography;
using BattleScript.Exceptions;
using BattleScript.Instructions;
using BattleScript.Core;

namespace BattleScript.InterpreterNS;

public partial class Interpreter
{
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
}