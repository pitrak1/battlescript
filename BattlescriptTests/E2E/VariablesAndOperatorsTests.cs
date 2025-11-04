using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class VariablesAndOperatorsTests
{
    [TestFixture]
    public class VariableTypes
    {
        [Test]
        public void SupportsStringVariablesUsingSingleQuotes()
        {
            var input = "x = 'asdf'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "asdf");
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsStringVariablesUsingDoubleQuotes()
        {
            var input = "x = \"asdf\"";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "asdf");
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsFloats()
        {
            var input = "x = 5.5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 5.5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsIntegers()
        {
            var input = "x = 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void SupportsBooleans()
        {
            var input = "x = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, 1);
            
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsLists()
        {
            var input = "x = []";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.List, new List<Variable>());
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsDictionaries()
        {
            var input = "x = {}";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable([]));
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsClasses()
        {
            var input = "class asdf():\n\ty = 3";
            var (callStack, closure) = Runner.Run(input);
            var expected = new ClassVariable("asdf", new Dictionary<string, Variable>
            {
                {"y", BsTypes.Create(BsTypes.Types.Int, 3)}
            });
            Assertions.AssertVariable(callStack, closure, "asdf", expected);
        }
        
        [Test]
        public void SupportsObjects()
        {
            var input = "class asdf():\n\ty = 3\nx = asdf()";
            var (callStack, closure) = Runner.Run(input);
            var classValues = new Dictionary<string, Variable>
            {
                { "y", BsTypes.Create(BsTypes.Types.Int, 3) }
            };
            var expected = new ObjectVariable(classValues, new ClassVariable("asdf", classValues));
            
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class UnaryOperators
    {
        [Test]
        public void SupportsUnaryPlus()
        {
            var input = "x = 6\ny = +x";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }

        [Test]
        public void SupportsUnaryMinus()
        {
            var input = "x = 6\ny = -x";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, -6);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
    }
    
    [TestFixture]
    public class IsAndIsNotOperators
    {
        [Test]
        public void IsReturnsTrueWhenVariableIsTheSame()
        {
            var input = "x = []\ny = x\nz = x is y";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariable(callStack, closure, "z", expected);
        }
        
        [Test]
        public void IsReturnsFalseWhenVariableIsNotTheSame()
        {
            var input = "x = {}\ny = {}\nz = x is y";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "z", expected);
        }
    }
    
    [TestFixture]
    public class InAndNotInOperators
    {
        [Test]
        public void ReturnsTrueWhenSubstringIsFound()
        {
            var input = "x = 'asd' in 'asdf'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenSubstringIsNotFound()
        {
            var input = "x = 'asdx' in 'asdf'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ThrowsErrorWhenRightArgumentIsStringAndLeftArgumentIsNot()
        {
            var input = "x = 5 in 'asdf'";
            Assert.Throws<InterpreterInvalidOperationException>(() => Runner.Run(input));
        }

        [Test]
        public void ReturnsTrueWhenValueIsFoundInList()
        {
            var input = "x = 5 in [1, 2, 3, 4, 5]";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenValueIsNotFoundInList()
        {
            var input = "x = 6 in [1, 2, 3, 4, 5]";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ReturnsTrueWhenValueIsFoundInKeysOfDictionary()
        {
            var input = "x = 5 in {5: 4, 3: 2}";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenValueIsNotFoundInKeysOfDictionary()
        {
            var input = "x = 6 in {5: 4, 3: 2}";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ThrowsErrorWhenRightArgumentIsAnythingButStringListOrDictionary()
        {
            var input = "class x():\n\ty=5\nz = 5 in x";
            Assert.Throws<InterpreterInvalidOperationException>(() => Runner.Run(input));
        }
    }

    [TestFixture]
    public class AssignmentOperators
    {
        [Test]
        public void SupportsAddAssignment()
        {
            var input = "x = 5\nx += 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 10);
            
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsSubtractAssignment()
        {
            var input = "x = 5\nx -= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 0);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsMultiplyAssignment()
        {
            var input = "x = 5\nx *= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 25);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void SupportsTrueDivisionAssignment()
        {
            var input = "x = 8\nx /= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 1.6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsFloorDivisionAssignment()
        {
            var input = "x = 8\nx //= 5";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 1);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsModuloAssignment()
        {
            var input = "x = 9\nx %= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 1);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsPowerAssignment()
        {
            var input = "x = 9\nx **= 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 81);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class Parentheses
    {
        [Test]
        public void HandlesParenthesisAsLeftOperand()
        {
            var input = "x = (1 + 2) * 2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void HandlesParenthesisAsRightOperand()
        {
            var input = "x = 2 * (1 + 2)";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class OperatorPrecedence
    {
        [Test]
        public void ValuesParenthesesOverPower()
        {
            var input = "x = 2 ** (1 + 2)";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ValuesPowerOverDivisionMultiplicationAndModulo()
        {
            var input = "x = 4 * 2 ** 3";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 32);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void ValuesDivisionMultiplicationAndModuloOverAdditionAndSubtraction()
        {
            var input = "x = 8 - 16 // 4";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 4);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ValuesAdditionAndSubtractionOverComparison()
        {
            var input = "x = 3 <= 8 - 7";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
    
    [TestFixture]
    public class ResultType
    {
        [Test]
        public void UnaryOperatorsReturnIntegerIfGivenInteger()
        {
            var input = "x = -1";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, -1);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void UnaryOperatorsReturnFloatIfGivenFloat()
        {
            var input = "x = -1.0";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, -1.0);
            
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void TrueDivisionAlwaysReturnsAFloat()
        {
            var input = "x = 12/3";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 4.0);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void FloorDivisionAlwaysReturnsAnInteger()
        {
            var input = "x = 13.5//3.2";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 4);
            
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnIntegerIfBothOperandsAreIntegers()
        {
            var input = "x = 12 * 3";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 36);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnFloatIfBothOperandsAreFloats()
        {
            var input = "x = 2.5 * 4.0";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 10.0);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnFloatIfEitherOperandIsFloat()
        {
            var input = "x = 2.5 * 4";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Float, 10.0);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
    
    [TestFixture]
    public class TruthinessAndFalsiness
    {
        [Test]
        public void EmptyListsAreFalsy()
        {
            var input = "x = False\nif []:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NonEmptyListsAreTruthy()
        {
            var input = "x = False\nif [1]:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void EmptyDictionariesAreFalsy()
        {
            var input = "x = False\nif {}:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NonEmptyDictionariesAreTruthy()
        {
            var input = "x = False\nif {'asdf': 1}:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void EmptyStringsAreFalsy()
        {
            var input = "x = False\nif '':\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NonEmptyStringsAreTruthy()
        {
            var input = "x = False\nif 'asdf':\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ZeroIntegerIsFalsy()
        {
            var input = "x = False\nif 0:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NonZeroIntegerIsTruthy()
        {
            var input = "x = False\nif 1:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ZeroFloatIsFalsy()
        {
            var input = "x = False\nif 0.0:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NonZeroFloatIsTruthy()
        {
            var input = "x = False\nif 1.5:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void FalseIsFalsy()
        {
            var input = "x = False\nif False:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void TrueIsTruthy()
        {
            var input = "x = False\nif True:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void NoneIsFalsy()
        {
            var input = "x = False\nif None:\n\tx = True";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class FormattedStrings
    {
        [Test]
        public void SupportsBasicStrings()
        {
            var input = "x = f'asdf'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "asdf");
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsInsertedVariables()
        {
            var input = "y = 15\nx = f'asdf{y}'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "asdf15");
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsInsertedExpressions()
        {
            var input = "y = 15\nx = f'asdf{y + 6}'";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "asdf21");
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}