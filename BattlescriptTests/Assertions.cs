using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertTokenEqual(Token input, Token expected)
    {
        Assert.That(input.Type, Is.EqualTo(expected.Type));
        Assert.That(input.Value, Is.EqualTo(expected.Value));
    }

    public static void AssertTokenListEqual(List<Token> input, List<Token> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (var i = 0; i < input.Count; i++)
        {
            AssertTokenEqual(input[i], expected[i]);
        }
    }

    public static void AssertInstructionEqual(Instruction? input, Instruction? expected)
    {
        if (input is null)
        {
            Assert.That(expected, Is.Null);
        }
        else if (input is AssignmentInstruction assignmentInst)
        {
            Assert.That(expected, Is.TypeOf<AssignmentInstruction>());
            var expectedInst = (AssignmentInstruction)expected!;
            
            Assert.That(assignmentInst.Operation, Is.EqualTo(expectedInst.Operation));
            AssertInstructionEqual(assignmentInst.Left, expectedInst.Left);
            AssertInstructionEqual(assignmentInst.Right, expectedInst.Right);
        }
        else if (input is BooleanInstruction booleanInst)
        {
            Assert.That(expected, Is.TypeOf<BooleanInstruction>());
            var expectedInst = (BooleanInstruction)expected!;
            
            Assert.That(booleanInst.Value, Is.EqualTo(expectedInst.Value));
        }
        else if (input is ClassInstruction classInst)
        {
            Assert.That(expected, Is.TypeOf<ClassInstruction>());
            var expectedInst = (ClassInstruction)expected!;
            
            Assert.That(classInst.Name, Is.EqualTo(expectedInst.Name));
            AssertInstructionListEqual(classInst.Superclasses, expectedInst.Superclasses);
            AssertInstructionListEqual(classInst.Instructions, expectedInst.Instructions);
        }
        else if (input is DictionaryInstruction dictInst)
        {
            Assert.That(expected, Is.TypeOf<DictionaryInstruction>());
            var expectedInst = (DictionaryInstruction)expected!;
            
            AssertInstructionListEqual(dictInst.Values, expectedInst.Values);
        }
        else if (input is FunctionInstruction functionInst)
        {
            Assert.That(expected, Is.TypeOf<FunctionInstruction>());
            var expectedInst = (FunctionInstruction)expected!;
            
            Assert.That(functionInst.Name, Is.EqualTo(expectedInst.Name));
            AssertInstructionListEqual(functionInst.Parameters, expectedInst.Parameters);
            AssertInstructionListEqual(functionInst.Instructions, expectedInst.Instructions);
        }
        else if (input is IfInstruction ifInst)
        {
            Assert.That(expected, Is.TypeOf<IfInstruction>());
            var expectedInst = (IfInstruction)expected!;
            
            AssertInstructionEqual(ifInst.Condition, expectedInst.Condition);
            AssertInstructionListEqual(ifInst.Instructions, expectedInst.Instructions);
        }
        else if (input is KeyValuePairInstruction kvpInst)
        {
            Assert.That(expected, Is.TypeOf<KeyValuePairInstruction>());
            var expectedInst = (KeyValuePairInstruction)expected!;
            
            AssertInstructionEqual(kvpInst.Left, expectedInst.Left);
            AssertInstructionEqual(kvpInst.Right, expectedInst.Right);
        }
        else if (input is NumberInstruction numberInst)
        {
            Assert.That(expected, Is.TypeOf<NumberInstruction>());
            var expectedInst = (NumberInstruction)expected!;
            
            Assert.That(numberInst.Value, Is.EqualTo(expectedInst.Value));
        }
        else if (input is OperationInstruction operationInst)
        {
            Assert.That(expected, Is.TypeOf<OperationInstruction>());
            var expectedInst = (OperationInstruction)expected!;
            
            Assert.That(operationInst.Operation, Is.EqualTo(expectedInst.Operation));
            AssertInstructionEqual(operationInst.Left, expectedInst.Left);
            AssertInstructionEqual(operationInst.Right, expectedInst.Right);
        }
        else if (input is ParensInstruction parensInst)
        {
            Assert.That(expected, Is.TypeOf<ParensInstruction>());
            var expectedInst = (ParensInstruction)expected!;
            
            AssertInstructionListEqual(parensInst.Instructions, expectedInst.Instructions);
            AssertInstructionEqual(parensInst.Next, expectedInst.Next);
        }
        else if (input is ReturnInstruction returnInst)
        {
            Assert.That(expected, Is.TypeOf<ReturnInstruction>());
            var expectedInst = (ReturnInstruction)expected!;
            
            AssertInstructionEqual(returnInst.Value, expectedInst.Value);
        }
        else if (input is SquareBracketsInstruction squareBracketsInst)
        {
            Assert.That(expected, Is.TypeOf<SquareBracketsInstruction>());
            var expectedInst = (SquareBracketsInstruction)expected!;
            
            AssertInstructionListEqual(squareBracketsInst.Values, expectedInst.Values);
            AssertInstructionEqual(squareBracketsInst.Next, expectedInst.Next);
        }
        else if (input is StringInstruction stringInst)
        {
            Assert.That(expected, Is.TypeOf<StringInstruction>());
            var expectedInst = (StringInstruction)expected!;
            
            Assert.That(stringInst.Value, Is.EqualTo(expectedInst.Value));
        }
        else if (input is VariableInstruction variableInst)
        {
            Assert.That(expected, Is.TypeOf<VariableInstruction>());
            var expectedInst = (VariableInstruction)expected!;
            
            Assert.That(variableInst.Name, Is.EqualTo(expectedInst.Name));
            AssertInstructionEqual(variableInst.Next, expectedInst.Next);
        }
        else if (input is WhileInstruction whileInst)
        {
            Assert.That(expected, Is.TypeOf<WhileInstruction>());
            var expectedInst = (WhileInstruction)expected!;
            
            AssertInstructionEqual(whileInst.Condition, expectedInst.Condition);
            AssertInstructionListEqual(whileInst.Instructions, expectedInst.Instructions);
        }
        else
        {
            throw new Exception("Unknown input type");
        }
    }

    public static void AssertInstructionListEqual(List<Instruction>? input, List<Instruction>? expected)
    {
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        Assert.That(input.Count, Is.EqualTo(expected.Count));

        for (var i = 0; i < input.Count; i++)
        {
            AssertInstructionEqual(input[i], expected[i]);
        }
    }
}