using System.Diagnostics;

namespace Battlescript;

public class CurlyBracesInstruction : ArrayInstruction
{
    public CurlyBracesInstruction(List<Token> tokens) : base([])
    {
        var closingSeparatorIndex = InstructionUtilities.GetTokenIndex(tokens, ["}"]);
        var tokensInSeparators = tokens.GetRange(1, closingSeparatorIndex - 1);
        InitializeDelimiter(tokensInSeparators);
        InitializeValues(tokensInSeparators);
        ParseNext(tokens, closingSeparatorIndex + 1);
    }

    public override Variable? Interpret(
        CallStack callStack, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var stringValues = new Dictionary<string, Variable>();
        var intValues = new Dictionary<int, Variable>();
    
        // Dictionaries will be an ArrayInstruction with colon delimiters and two values within an ArrayInstruction
        // of comma delimiters if there are multiple entries. Dictionaries will be an ArrayInstruction of colon
        // delimiters with two values if there is only one entry.
        if (Delimiter == Consts.Comma)
        {
            foreach (var dictValue in Values)
            {
                InterpretAndAddKvp(callStack, stringValues, intValues, dictValue!);
            }
        } 
        else if (Delimiter == Consts.Colon)
        {
            InterpretAndAddKvp(callStack, stringValues, intValues, this);
        } else if (Values.Count != 0)
        {
            throw new Exception("Badly formed dictionary");
        }
    
        return BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(intValues, stringValues));
    }
    
    private void InterpretAndAddKvp(CallStack callStack, Dictionary<string, Variable> stringValues, Dictionary<int, Variable> intValues, Instruction? instruction)
    {
        var kvp = IsValidKvp(instruction);
        var value = kvp.Values[1]!.Interpret(callStack); 
        var indexValue = GetIndexValue(callStack, kvp.Values[0]!);
        if (indexValue.IntValue is not null)
        {
            intValues.Add(indexValue.IntValue.Value, value);
        }
        else
        {
            stringValues.Add(indexValue.StringValue!, value);
        }
    }

    private ArrayInstruction IsValidKvp(Instruction? instruction)
    {
        if (instruction is ArrayInstruction { Delimiter: ":", Values: [not null, not null] } kvpInstruction)
        {
            return kvpInstruction;
        }
        else
        {
            throw new Exception("Badly formed dictionary");
        }
    }
        
    private (int? IntValue, string? StringValue) GetIndexValue(CallStack callStack, Instruction index)
    {
        Variable indexVariable;
        if (index is ArrayInstruction indexInst)
        {
            var listVariable = indexInst.Values.Select(x => x.Interpret(callStack)).ToList();
            indexVariable = listVariable[0]!;
        }
        else
        {
            indexVariable = index.Interpret(callStack);
        }
    
        if (BsTypes.Is(BsTypes.Types.Int, indexVariable))
        {
            return (BsTypes.GetIntValue(indexVariable), null);
        } else if (BsTypes.Is(BsTypes.Types.String, indexVariable))
        {
            return (null, BsTypes.GetStringValue(indexVariable));
        }
        else
        {
            throw new Exception("Invlaid dictionary index, must be int or string");
        }
    }
}