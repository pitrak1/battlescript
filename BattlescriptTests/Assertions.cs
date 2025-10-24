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
    
    public static void AssertInputsProduceSameParserOutput(string input, List<Instruction> expected)
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
        Assert.That(input.GetType(), Is.EqualTo(expected.GetType()));

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
            case MemberInstruction memberInstruction:
                CompareMemberInstructions(memberInstruction, expected as MemberInstruction);
                return;
            case TryInstruction tryInstruction:
                CompareTryInstructions(tryInstruction, expected as TryInstruction);
                return;
            case ExceptInstruction exceptInstruction:
                CompareExceptInstructions(exceptInstruction, expected as ExceptInstruction);
                return;
            case FinallyInstruction finallyInstruction:
                CompareFinallyInstructions(finallyInstruction, expected as FinallyInstruction);
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
            AssertInstructionListsEqual(inputInst.Arguments!, expectedInst.Arguments!);
        }

        void CompareClassInstructions(ClassInstruction inputInst, ClassInstruction expectedInst)
        {
            Assert.That(inputInst.Name, Is.EqualTo(expectedInst.Name));
            
            if (BsTypes.TypeStrings.Contains(inputInst.Name)) return;
            
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
            Assert.That(inputInst.Parameters.Names, Is.EquivalentTo(expectedInst.Parameters.Names));
            AssertInstructionDictionariesEqual(inputInst.Parameters.DefaultValues, expectedInst.Parameters.DefaultValues);
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
            Assert.That(inputInst.Parameters.Names, Is.EquivalentTo(expectedInst.Parameters.Names));
            AssertInstructionDictionariesEqual(inputInst.Parameters.DefaultValues, expectedInst.Parameters.DefaultValues);
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

        void CompareMemberInstructions(MemberInstruction inputInst, MemberInstruction expectedInst)
        {
            Assert.That(inputInst.Value, Is.EqualTo(expectedInst.Value));
        }
        
        void CompareTryInstructions(TryInstruction inputInst, TryInstruction expectedInst)
        {
            Assert.That(inputInst.Excepts.Count, Is.EqualTo(expectedInst.Excepts.Count));
            for (var i = 0; i < inputInst.Excepts.Count; i++)
            {
                AssertInstructionsEqual(inputInst.Excepts[i], expectedInst.Excepts[i]);
            }
            
            AssertInstructionsEqual(inputInst.Else, expectedInst.Else);
            AssertInstructionsEqual(inputInst.Finally, expectedInst.Finally);
        }
        
        void CompareExceptInstructions(ExceptInstruction inputInst, ExceptInstruction expectedInst)
        {
            AssertInstructionsEqual(inputInst.ExceptionType, expectedInst.ExceptionType);
            AssertInstructionsEqual(inputInst.ExceptionVariable, expectedInst.ExceptionVariable);
        }
        
        void CompareFinallyInstructions(FinallyInstruction inputInst, FinallyInstruction expectedInst)
        {
            return;
        }
    }
    
    public static void AssertInstructionDictionariesEqual(Dictionary<string, Instruction?> input, Dictionary<string, Instruction?> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        foreach (var (key, value) in input)
        {
            Assert.That(expected.ContainsKey(key));
            AssertInstructionsEqual(expected[key], value);
        }
    }
    
    public static void AssertVariableDictionariesEqual(Dictionary<string, Variable?> input, Dictionary<string, Variable?> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        foreach (var (key, value) in input)
        {
            Assert.That(expected.ContainsKey(key));

            if (value is null)
            {
                Assert.That(expected[key], Is.Null);
            }
            else
            {
                AssertVariablesEqual(expected[key], value);
            }
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

    public static void AssertVariable(Memory memory, string name, Variable expected)
    {
        var variable = memory.GetVariable(name);
        AssertVariablesEqual(variable, expected);
    }

    public static void AssertVariableListsEqual(List<Variable?> input, List<Variable> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (var i = 0; i < input.Count; i++)
        {
            AssertVariablesEqual(input[i], expected[i]);
        }
    }
    
    

    public static void AssertVariablesEqual(Variable? input, Variable expected)
    {
        Assert.That(input, Is.Not.Null);
        if (ReferenceEquals(input, expected)) return;
        Assert.That(input.GetType(), Is.EqualTo(expected.GetType()));

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
            case MappingVariable mappingVariable:
                CompareMappingVariables(mappingVariable, expected as MappingVariable);
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
            if (input.Value is int)
            {
                Assert.That(input.Value, Is.EqualTo(expected.Value));;
            } else if (input.Value is double)
            {
                Assert.That(input.Value, Is.InRange(expected.Value - Consts.FloatingPointTolerance, expected.Value + Consts.FloatingPointTolerance));
            }
            else
            {
                throw new Exception("Expected an int or float for numeric variable, got " + input.Value + " of type " + input.Value.GetType().Name);
            }
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
            AssertVariableDictionariesEqual(input.Values, expected.Values);
            CompareClassVariables(input.Class, expected.Class);
        }
        
        void CompareClassVariables(ClassVariable input, ClassVariable expected)
        {
            Assert.That(input.Name, Is.EqualTo(expected.Name));
            AssertVariableDictionariesEqual(input.Values, expected.Values);
            Assert.That(input.SuperClasses.Count, Is.EqualTo(expected.SuperClasses.Count));
            for (var i = 0; i < input.SuperClasses.Count; i++)
            {
                CompareClassVariables(input.SuperClasses[i], expected.SuperClasses[i]);
            }
        }

        void CompareFunctionVariables(FunctionVariable input, FunctionVariable expected)
        {
            Assert.That(input.Parameters.Names, Is.EquivalentTo(expected.Parameters.Names));
            AssertInstructionDictionariesEqual(input.Parameters.DefaultValues, expected.Parameters.DefaultValues);
            AssertInstructionListsEqual(input.Instructions!, expected.Instructions!);
        }

        void CompareMappingVariables(MappingVariable input, MappingVariable expected)
        {
            AssertVariableDictionariesEqual(input.StringValues, expected.StringValues);
            
            Assert.That(input.IntValues.Count, Is.EqualTo(expected.IntValues.Count));
            foreach (var (key, value) in input.IntValues)
            {
                Assert.That(expected.IntValues.ContainsKey(key));
                AssertVariablesEqual(expected.IntValues[key], value);
            }
        }
    }

    public static void AssertStacktrace(
        List<(string File, int Line, string Expression, string Function)> input,
        List<(string File, int Line, string Expression, string Function)> expected)
    {
        Assert.That(input.Count, Is.EqualTo(expected.Count));
        for (int i = 0; i < input.Count; i++)
        {
            Assert.That(input[i].File, Is.EqualTo(expected[i].File));
            Assert.That(input[i].Line, Is.EqualTo(expected[i].Line));
            Assert.That(input[i].Expression, Is.EqualTo(expected[i].Expression));
            Assert.That(input[i].Function, Is.EqualTo(expected[i].Function));
        }
    }
}