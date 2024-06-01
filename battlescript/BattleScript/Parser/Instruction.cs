using BattleScript.Core;

namespace BattleScript.Instructions;

public class Instruction
{
    public int? Line { get; set; }
    public int? Column { get; set; }
    public Consts.InstructionTypes? Type { get; set; }
    public dynamic? Value { get; set; }
    public Instruction? Next { get; set; }
    public List<Instruction> Instructions { get; set; }
    public Instruction? Left { get; set; }
    public Instruction? Right { get; set; }

    public Instruction(
        int? line = null,
        int? column = null,
        Consts.InstructionTypes? type = null,
        dynamic? value = null,
        Instruction? next = null,
        List<Instruction>? instructions = null,
        Instruction? left = null,
        Instruction? right = null
    )
    {
        Line = line;
        Column = column;
        Type = type;
        Value = value;
        Next = next;
        Instructions = instructions ?? new List<Instruction>();
        Left = left;
        Right = right;
    }


    // All of the stuff below this point is to override the == operator
    public override bool Equals(object? obj)
    {
        return this.Equals(obj as Instruction);
    }

    public bool Equals(Instruction instruction)
    {
        if (instruction is null)
        {
            return false;
        }

        if (ReferenceEquals(this, instruction))
        {
            return true;
        }

        if (GetType() != instruction.GetType())
        {
            return false;
        }

        if (Type != instruction.Type)
        {
            return false;
        }

        if (Value != instruction.Value)
        {
            return false;
        }

        if (Next != instruction.Next)
        {
            return false;
        }

        if (!Enumerable.SequenceEqual(Instructions, instruction.Instructions))
        {
            return false;
        }

        if (Left != instruction.Left)
        {
            return false;
        }

        if (Right != instruction.Right)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return (
            Type,
            Value?.GetHashCode(),
            Left?.GetHashCode(),
            Right?.GetHashCode(),
            Next?.GetHashCode(),
            Instructions.GetHashCode(),
            Line,
            Column
        ).GetHashCode();
    }

    public static bool operator ==(Instruction? lhs, Instruction? rhs)
    {
        if (lhs is null)
        {
            return rhs is null;
        }
        else if (rhs is null)
        {
            return false;
        }
        else
        {
            return lhs.Equals(rhs);
        }
    }

    public static bool operator !=(Instruction? lhs, Instruction? rhs)
    {
        return !(lhs == rhs);
    }
}

