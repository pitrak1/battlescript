using System.Diagnostics;

namespace Battlescript;

public class ParenthesesInstruction : GenericArrayInstruction<Instruction?>
{
    public ParenthesesInstruction(List<Token> tokens)
    {
        var closeParensIndex = InstructionUtilities.GetTokenIndex(tokens, [")"]);
        
        PopulateValues(tokens.GetRange(1, closeParensIndex - 1), ",");
        
        if (tokens.Count > closeParensIndex + 1)
        {
            Next = InstructionFactory.Create(tokens.GetRange(closeParensIndex + 1, tokens.Count - closeParensIndex - 1));
        }
    }

    public ParenthesesInstruction(List<Instruction?> values, Instruction? next = null)
    {
        Values = values;
        Next = next;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        if (instructionContext is FunctionVariable functionVariable)
        {
            return functionVariable.RunFunction(memory, Values, objectContext);
        }
        else
        {
            if (instructionContext is ClassVariable classVariable)
            {
                var objectVariable = classVariable.CreateObject();
                var constructor = objectVariable.GetItem(memory, "__init__");
                if (constructor is FunctionVariable constructorVariable)
                {
                    List<Variable> arguments = [];
                    foreach (var argument in Values)
                    {
                        arguments.Add(argument.Interpret(memory, objectVariable, objectContext));
                    }
                    
                    constructorVariable.RunFunction(memory, arguments, objectVariable);
                }
                return objectVariable;
            }
            else
            {
                throw new Exception("Can only create an object of a class");
            }
        }
    }
}