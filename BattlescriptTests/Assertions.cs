using Battlescript;

namespace BattlescriptTests;

public static class Assertions
{
    public static void AssertInputProducesLexerOutput(string input, List<Token> expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        AssertTokenListsEqual(lexerResult, expected);
        
        void AssertTokenListsEqual(List<Token> listInput, List<Token> listExpected)
        {
            Assert.That(listInput.Count, Is.EqualTo(listExpected.Count));
            for (var i = 0; i < listInput.Count; i++)
            {
                AssertTokensEqual(listInput[i], listExpected[i]);
            }
        }
        
        void AssertTokensEqual(Token tokenInput, Token tokenExpected)
        {
            Assert.That(tokenInput.Type, Is.EqualTo(tokenExpected.Type));
            Assert.That(tokenInput.Value, Is.EqualTo(tokenExpected.Value));
        }
    }

    public static void AssertInputProducesParserOutput(string input, List<Instruction> expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        Postparser.Run(parserResult);

        AssertInstructionListsEqual(parserResult, expected);
    }
    
    public static void AssertInstructionListsEqual(List<Instruction?> listInput, List<Instruction?> listExpected)
    {
        Assert.That(listInput.Count, Is.EqualTo(listExpected.Count));
        for (var i = 0; i < listInput.Count; i++)
        {
            AssertInstructionsEqual(listInput[i], listExpected[i]);
        }
    }

    public static void AssertInstructionsEqual(Instruction? input, Instruction? expected)
    {
        if (input is null && expected is null) return;
        Assert.That(input, Is.Not.Null);
        Assert.That(expected, Is.Not.Null);
        
        if (ReferenceEquals(input, expected)) return;
        if (input.GetType() != expected.GetType()) return;

        if (input.Next is not null) AssertInstructionsEqual(input.Next, expected.Next);
        AssertInstructionListsEqual(input.Instructions!, expected.Instructions!);
        
        switch (input)
        {
            case ArrayInstruction arrayInstruction:
                CompareArrayInstructions(arrayInstruction, expected as ArrayInstruction);
                return;
            case AssignmentInstruction assignmentInstruction:
                CompareAssignmentInstructions(assignmentInstruction, expected as AssignmentInstruction);
                return;
            case BuiltInInstruction builtInInstruction:
                CompareBuiltInInstructions(builtInInstruction, expected as BuiltInInstruction);
                return;
            case ClassInstruction classInstruction:
                CompareClassInstructions(classInstruction, expected as ClassInstruction);
                return;
            case ConstantInstruction constantInstruction:
                CompareConstantInstructions(constantInstruction, expected as ConstantInstruction);
                return;
            case ElseInstruction elseInstruction:
                CompareElseInstructions(elseInstruction, expected as ElseInstruction);
                return;
            case ForInstruction forInstruction:
                CompareForInstructions(forInstruction, expected as ForInstruction);
                return;
            case FunctionInstruction functionInstruction:
                CompareFunctionInstructions(functionInstruction, expected as FunctionInstruction);
                return;
            case IfInstruction ifInstruction:
                CompareIfInstructions(ifInstruction, expected as IfInstruction);
                return;
            case ImportInstruction importInstruction:
                CompareImportInstructions(importInstruction, expected as ImportInstruction);
                return;
            case LambdaInstruction lambdaInstruction:
                CompareLambdaInstructions(lambdaInstruction, expected as LambdaInstruction);
                return;
            case NumericInstruction numericInstruction:
                CompareNumericInstructions(numericInstruction, expected as NumericInstruction);
                return;
            case OperationInstruction operationInstruction:
                CompareOperationInstructions(operationInstruction, expected as OperationInstruction);
                return;
            case PrincipleTypeInstruction principleTypeInstruction:
                ComparePrincipleTypeInstructions(principleTypeInstruction, expected as PrincipleTypeInstruction);
                return;
            case ReturnInstruction returnInstruction:
                CompareReturnInstructions(returnInstruction, expected as ReturnInstruction);
                return;
            case StringInstruction stringInstruction:
                CompareStringInstructions(stringInstruction, expected as StringInstruction);
                return;
            case VariableInstruction variableInstruction:
                CompareVariableInstructions(variableInstruction, expected as VariableInstruction);
                return;
            case WhileInstruction whileInstruction:
                CompareWhileInstructions(whileInstruction, expected as WhileInstruction);
                return;
            default:
                return;
        }
        
        void CompareArrayInstructions(ArrayInstruction inputInst, ArrayInstruction expectedInst)
        {
            AssertInstructionListsEqual(inputInst.Values, expectedInst.Values);
            if (inputInst.Separator is not null) Assert.That(inputInst.Separator, Is.EqualTo(expectedInst.Separator));
            if (inputInst.Delimiter is not null) Assert.That(inputInst.Delimiter, Is.EqualTo(expectedInst.Delimiter));
        }

        void CompareAssignmentInstructions(AssignmentInstruction inputInst, AssignmentInstruction expectedInst)
        {
            Assert.That(inputInst.Operation, Is.EqualTo(expectedInst.Operation));
            AssertInstructionsEqual(inputInst.Left, expectedInst.Left);
            AssertInstructionsEqual(inputInst.Right, expectedInst.Right);
        }

        void CompareBuiltInInstructions(BuiltInInstruction inputInst, BuiltInInstruction expectedInst)
        {
            Assert.That(inputInst.Name, Is.EqualTo(expectedInst.Name));
            AssertInstructionListsEqual(inputInst.Parameters!, expectedInst.Parameters!);
        }

        void CompareClassInstructions(ClassInstruction inputInst, ClassInstruction expectedInst)
        {
            Assert.That(inputInst.Name, Is.EqualTo(expectedInst.Name));
            
            if (Consts.BuiltInTypes.Contains(inputInst.Name)) return;
            
            AssertInstructionListsEqual(inputInst.Superclasses!, expectedInst.Superclasses!);
        }

        void CompareConstantInstructions(ConstantInstruction inputInst, ConstantInstruction expectedInst)
        {
            Assert.That(inputInst.Value, Is.EqualTo(expectedInst.Value));
        }

        void CompareElseInstructions(ElseInstruction inputInst, ElseInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.Condition, expectedInst.Condition);
        }

        void CompareForInstructions(ForInstruction inputInst, ForInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.BlockVariable, expectedInst.BlockVariable);
            AssertInstructionsEqual(inputInst.Range, expectedInst.Range);
        }
        
        void CompareFunctionInstructions(FunctionInstruction inputInst, FunctionInstruction expectedInst)
        {
            Assert.That(inputInst.Name, Is.EqualTo(expectedInst.Name));
            AssertInstructionListsEqual(inputInst.Parameters!, expectedInst.Parameters!);
        }
        
        void CompareIfInstructions(IfInstruction inputInst, IfInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.Condition, expectedInst.Condition);
        }

        void CompareImportInstructions(ImportInstruction inputInst, ImportInstruction expectedInst)
        {
            Assert.That(inputInst.FilePath, Is.EqualTo(expectedInst.FilePath));
            Assert.That(inputInst.ImportNames, Is.EquivalentTo(expectedInst.ImportNames));
        }

        void CompareLambdaInstructions(LambdaInstruction inputInst, LambdaInstruction expectedInst)
        {
            AssertInstructionListsEqual(inputInst.Parameters!, expectedInst.Parameters!);
        }

        void CompareNumericInstructions(NumericInstruction inputInst, NumericInstruction expectedInst)
        {
            Assert.That(inputInst.Value, Is.EqualTo(expectedInst.Value));
        }

        void CompareOperationInstructions(OperationInstruction inputInst, OperationInstruction expectedInst)
        {
            Assert.That(inputInst.Operation, Is.EqualTo(expectedInst.Operation));
            AssertInstructionsEqual(inputInst.Left, expectedInst.Left);
            AssertInstructionsEqual(inputInst.Right, expectedInst.Right);
        }

        void ComparePrincipleTypeInstructions(PrincipleTypeInstruction inputInst, PrincipleTypeInstruction expectedInst)
        {
            Assert.That(inputInst.Value, Is.EqualTo(expectedInst.Value));
            AssertInstructionListsEqual(inputInst.Parameters!, expectedInst.Parameters!);
        }
        
        void CompareReturnInstructions(ReturnInstruction inputInst, ReturnInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.Value, expectedInst.Value);
        }

        void CompareStringInstructions(StringInstruction inputInst, StringInstruction expectedInst)
        {
            Assert.That(inputInst.Value, Is.EqualTo(expectedInst.Value));
        }
        
        void CompareVariableInstructions(VariableInstruction inputInst, VariableInstruction expectedInst)
        {
            Assert.That(inputInst.Name, Is.EqualTo(expectedInst.Name));
        }
        
        void CompareWhileInstructions(WhileInstruction inputInst, WhileInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.Condition, expectedInst.Condition);
        }
    }
    
    public static void AssertInputProducesParserOutput(string input, Instruction? expected)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parserResult = InstructionFactory.Create(lexerResult);
        
        AssertInstructionsEqual(parserResult, expected);
    }

    public static void AssertVariablesEqual(Variable? input, Variable expected)
    {
        Assert.That(input, Is.Not.Null);
        if (ReferenceEquals(input, expected)) return;
        if (input.GetType() != expected.GetType()) return;

        switch (input)
        {
            case StringVariable stringVariable:
                CompareStringVariables(stringVariable, expected as StringVariable);
                break;
            case NumericVariable numericVariable:
                CompareNumericVariables(numericVariable, expected as NumericVariable);
                break;
            case SequenceVariable sequenceVariable:
                CompareSequenceVariables(sequenceVariable, expected as SequenceVariable);
                break;
            case ConstantVariable constantVariable:
                CompareConstantVariables(constantVariable, expected as ConstantVariable);
                break;
            case ObjectVariable objectVariable:
                CompareObjectVariables(objectVariable, expected as ObjectVariable);
                break;
            case ClassVariable classVariable:
                CompareClassVariables(classVariable, expected as ClassVariable);
                break;
            case DictionaryVariable dictionaryVariable:
                CompareDictionaryVariables(dictionaryVariable, expected as DictionaryVariable);
                break;
            case FunctionVariable functionVariable:
                CompareFunctionVariables(functionVariable, expected as FunctionVariable);
                break;
        }

        void CompareStringVariables(StringVariable input, StringVariable expected)
        {
            Assert.That(input.Value, Is.EqualTo(expected.Value));
        }
        
        void CompareNumericVariables(NumericVariable input, NumericVariable expected)
        {
            Assert.That(input.Value, Is.EqualTo(expected.Value));
        }

        void CompareSequenceVariables(SequenceVariable input, SequenceVariable expected)
        {
            Assert.That(input.Values.Count, Is.EqualTo(expected.Values.Count));
            for (var i = 0; i < input.Values.Count; i++)
            {
                AssertVariablesEqual(input.Values[i], expected.Values[i]);
            }
        }

        void CompareConstantVariables(ConstantVariable input, ConstantVariable expected)
        {
            Assert.That(input.Value, Is.EqualTo(expected.Value));
        }

        void CompareObjectVariables(ObjectVariable input, ObjectVariable expected)
        {
            CompareVariableDictionaries(input.Values, expected.Values);
            CompareClassVariables(input.Class, expected.Class);
        }
        
        void CompareVariableDictionaries(Dictionary<string, Variable> input, Dictionary<string, Variable> expected)
        {
            Assert.That(input.Count, Is.EqualTo(expected.Count));
            foreach (var (key, value) in input)
            {
                Assert.That(expected.ContainsKey(key));
                AssertVariablesEqual(expected[key], value);
            }
        }

        void CompareClassVariables(ClassVariable input, ClassVariable expected)
        {
            Assert.That(input.Name, Is.EqualTo(expected.Name));
            CompareVariableDictionaries(input.Values, expected.Values);
            Assert.That(input.SuperClasses.Count, Is.EqualTo(expected.SuperClasses.Count));
            for (var i = 0; i < input.SuperClasses.Count; i++)
            {
                CompareClassVariables(input.SuperClasses[i], expected.SuperClasses[i]);
            }
        }

        void CompareFunctionVariables(FunctionVariable input, FunctionVariable expected)
        {
            AssertInstructionListsEqual(input.Parameters!, expected.Parameters!);
            AssertInstructionListsEqual(input.Instructions!, expected.Instructions!);
        }

        void CompareDictionaryVariables(DictionaryVariable input, DictionaryVariable expected)
        {
            CompareVariableDictionaries(input.StringValues, expected.StringValues);
            
            Assert.That(input.IntValues.Count, Is.EqualTo(expected.IntValues.Count));
            foreach (var (key, value) in input.IntValues)
            {
                Assert.That(expected.IntValues.ContainsKey(key));
                AssertVariablesEqual(expected.IntValues[key], value);
            }
        }
    }
}