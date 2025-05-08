using Battlescript;

namespace BattlescriptTests;

public class InterpreterAssertions
{
    public static void AssertInputProducesOutput(string input, List<Dictionary<string, Variable>> expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        var interpreter = new Interpreter(parserResult);
        var interpreterResult = interpreter.Run();
        
        AssertScopeListEqual(interpreterResult, expected);
    }
    
    private static void AssertScopeListEqual(
        List<Dictionary<string, Variable>>? input,
        List<Dictionary<string, Variable>>? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else
        {
            Assert.That(expected, Is.Not.Null);
            Assert.That(input.Count, Is.EqualTo(expected.Count));

            for (var i = 0; i < input.Count; i++)
            {
                AssertScopesEqual(input[i], expected[i]);
            }
        }
    }

    private static void AssertScopesEqual(Dictionary<string, Variable>? input, Dictionary<string, Variable>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        
        foreach (var kvp in expected)
        {
            Assert.That(input, Contains.Key(kvp.Key));
            if (input[kvp.Key] is Variable)
            {
                AssertVariableEqual(input[kvp.Key], expected[kvp.Key]);
            }
            else
            {
                Assert.That(input[kvp.Key], Is.EqualTo(kvp.Value));
            }
        }
    }

    public static void AssertVariableEqual(Variable? input, Variable? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else if (input is BooleanVariable booleanVariable)
        {
            Assert.That(expected, Is.TypeOf<BooleanVariable>());
            var expectedVariable = (BooleanVariable)expected;
            
            Assert.That(booleanVariable.Value, Is.EqualTo(expectedVariable.Value));
        } else if (input is ClassVariable classVariable)
        {
            Assert.That(expected, Is.TypeOf<ClassVariable>());
            var expectedVariable = (ClassVariable)expected;

            foreach (var entry in classVariable.Values)
            {
                Assert.That(expectedVariable.Values.ContainsKey(entry.Key));
                AssertVariableEqual(expectedVariable.Values[entry.Key], entry.Value);
            }
        }
        else if (input is DictionaryVariable dictionaryVariable)
        {
            Assert.That(expected, Is.TypeOf<DictionaryVariable>());
            var expectedVariable = (DictionaryVariable)expected;
            
            var dictionaryValues = dictionaryVariable.Values.ConvertAll(v => (Variable)v);
            var expectedValues = expectedVariable.Values.ConvertAll(v => (Variable)v);
            AssertVariableListEqual(dictionaryValues, expectedValues);
        }
        else if (input is FunctionVariable functionVariable)
        {
            Assert.That(expected, Is.TypeOf<FunctionVariable>());
            var expectedVariable = (FunctionVariable)expected;
            
            Assertions.AssertInstructionListEqual(functionVariable.Parameters, expectedVariable.Parameters);
            Assertions.AssertInstructionListEqual(functionVariable.Instructions, expectedVariable.Instructions);
        }
        else if (input is KeyValuePairVariable kvpVariable)
        {
            Assert.That(expected, Is.TypeOf<KeyValuePairVariable>());
            var expectedVariable = (KeyValuePairVariable)expected;
            
            AssertVariableEqual(kvpVariable.Left, expectedVariable.Left);
            AssertVariableEqual(kvpVariable.Right, expectedVariable.Right);
        }
        else if (input is ListVariable listVariable)
        {
            Assert.That(expected, Is.TypeOf<ListVariable>());
            var expectedVariable = (ListVariable)expected;
            
            AssertVariableListEqual(listVariable.Values, expectedVariable.Values);
        } else if (input is NullVariable nullVariable)
        {
            Assert.That(expected, Is.TypeOf<NullVariable>());
        } else if (input is NumberVariable numberVariable)
        {
            Assert.That(expected, Is.TypeOf<NumberVariable>());
            var expectedVariable = (NumberVariable)expected;
            
            Assert.That(numberVariable.Value, Is.EqualTo(expectedVariable.Value));
        } else if (input is ObjectVariable objectVariable)
        {
            Assert.That(expected, Is.TypeOf<ObjectVariable>());
            var expectedVariable = (ObjectVariable)expected;
            
            foreach (var entry in objectVariable.Values)
            {
                Assert.That(expectedVariable.Values.ContainsKey(entry.Key));
                AssertVariableEqual(expectedVariable.Values[entry.Key], entry.Value);
            }
        } else if (input is StringVariable stringVariable)
        {
            Assert.That(expected, Is.TypeOf<StringVariable>());
            var expectedVariable = (StringVariable)expected;
            
            Assert.That(stringVariable.Value, Is.EqualTo(expectedVariable.Value));
        }
    }


    private static void AssertVariableListEqual(List<Variable>? input, List<Variable>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        for (var i = 0; i < input.Count; i++)
        {
            AssertVariableEqual(input[i], expected[i]);
        }
    }
    
    private static void AssertVariableDictionaryEqual(
        Dictionary<string, Variable>? input, 
        Dictionary<string, Variable>? expected
    )
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        foreach (var kvp in expected)
        {
            Assert.That(input, Contains.Key(kvp.Key));
            AssertVariableEqual(input[kvp.Key], expected[kvp.Key]);
        }
    }
}