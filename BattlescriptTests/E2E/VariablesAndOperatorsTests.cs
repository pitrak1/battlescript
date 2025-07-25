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
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.String, "asdf");
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsStringVariablesUsingDoubleQuotes()
        {
            var input = "x = \"asdf\"";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.String, "asdf");
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsFloats()
        {
            var input = "x = 5.5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, 5.5);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsIntegers()
        {
            var input = "x = 5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 5);
            
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void SupportsBooleans()
        {
            var input = "x = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, 1);
            
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsLists()
        {
            var input = "x = []";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.List, new List<Variable>());
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsDictionaries()
        {
            var input = "x = {}";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable([]));
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsClasses()
        {
            var input = "class asdf():\n\ty = 3";
            var memory = Runner.Run(input);
            var expected = new ClassVariable("asdf", new Dictionary<string, Variable>
            {
                {"y", memory.CreateBsType(Memory.BsTypes.Int, 3)}
            });
            Assertions.AssertVariable(memory, "asdf", expected);
        }
        
        [Test]
        public void SupportsObjects()
        {
            var input = "class asdf():\n\ty = 3\nx = asdf()";
            var memory = Runner.Run(input);
            var classValues = new Dictionary<string, Variable>
            {
                { "y", memory.CreateBsType(Memory.BsTypes.Int, 3) }
            };
            var expected = new ObjectVariable(classValues, new ClassVariable("asdf", classValues));
            
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }

    [TestFixture]
    public class UnaryOperators
    {
        [Test]
        public void SupportsUnaryPlus()
        {
            var input = "x = 6\ny = +x";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SupportsUnaryMinus()
        {
            var input = "x = 6\ny = -x";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, -6);
            Assertions.AssertVariable(memory, "y", expected);
        }
    }
    
    [TestFixture]
    public class IsAndIsNotOperators
    {
        [Test]
        public void IsReturnsTrueWhenVariableIsTheSame()
        {
            var input = "x = []\ny = x\nz = x is y";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariable(memory, "z", expected);
        }
        
        [Test]
        public void IsReturnsFalseWhenVariableIsNotTheSame()
        {
            var input = "x = {}\ny = {}\nz = x is y";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "z", expected);
        }
    }
    
    [TestFixture]
    public class InAndNotInOperators
    {
        [Test]
        public void ReturnsTrueWhenSubstringIsFound()
        {
            var input = "x = 'asd' in 'asdf'";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenSubstringIsNotFound()
        {
            var input = "x = 'asdx' in 'asdf'";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenValueIsNotFoundInList()
        {
            var input = "x = 6 in [1, 2, 3, 4, 5]";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ReturnsTrueWhenValueIsFoundInKeysOfDictionary()
        {
            var input = "x = 5 in {5: 4, 3: 2}";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ReturnsFalseWhenValueIsNotFoundInKeysOfDictionary()
        {
            var input = "x = 6 in {5: 4, 3: 2}";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 10);
            
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsSubtractAssignment()
        {
            var input = "x = 5\nx -= 5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 0);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsMultiplyAssignment()
        {
            var input = "x = 5\nx *= 5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 25);
            
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void SupportsTrueDivisionAssignment()
        {
            var input = "x = 8\nx /= 5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, 1.6);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsFloorDivisionAssignment()
        {
            var input = "x = 8\nx //= 5";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 1);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsModuloAssignment()
        {
            var input = "x = 9\nx %= 2";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 1);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsPowerAssignment()
        {
            var input = "x = 9\nx **= 2";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 81);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }

    [TestFixture]
    public class Parentheses
    {
        [Test]
        public void HandlesParenthesisAsLeftOperand()
        {
            var input = "x = (1 + 2) * 2";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void HandlesParenthesisAsRightOperand()
        {
            var input = "x = 2 * (1 + 2)";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }

    [TestFixture]
    public class OperatorPrecedence
    {
        [Test]
        public void ValuesParenthesesOverPower()
        {
            var input = "x = 2 ** (1 + 2)";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 8);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ValuesPowerOverDivisionMultiplicationAndModulo()
        {
            var input = "x = 4 * 2 ** 3";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 32);
            
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void ValuesDivisionMultiplicationAndModuloOverAdditionAndSubtraction()
        {
            var input = "x = 8 - 16 // 4";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 4);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ValuesAdditionAndSubtractionOverComparison()
        {
            var input = "x = 3 <= 8 - 7";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
    
    [TestFixture]
    public class ResultType
    {
        [Test]
        public void UnaryOperatorsReturnIntegerIfGivenInteger()
        {
            var input = "x = -1";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, -1);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void UnaryOperatorsReturnFloatIfGivenFloat()
        {
            var input = "x = -1.0";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, -1.0);
            
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void TrueDivisionAlwaysReturnsAFloat()
        {
            var input = "x = 12/3";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, 4.0);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void FloorDivisionAlwaysReturnsAnInteger()
        {
            var input = "x = 13.5//3.2";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 4);
            
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnIntegerIfBothOperandsAreIntegers()
        {
            var input = "x = 12 * 3";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 36);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnFloatIfBothOperandsAreFloats()
        {
            var input = "x = 2.5 * 4.0";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, 10.0);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void OtherOperatorsReturnFloatIfEitherOperandIsFloat()
        {
            var input = "x = 2.5 * 4";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Float, 10.0);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
    
    [TestFixture]
    public class TruthinessAndFalsiness
    {
        [Test]
        public void EmptyListsAreFalsy()
        {
            var input = "x = False\nif []:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NonEmptyListsAreTruthy()
        {
            var input = "x = False\nif [1]:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void EmptyDictionariesAreFalsy()
        {
            var input = "x = False\nif {}:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NonEmptyDictionariesAreTruthy()
        {
            var input = "x = False\nif {'asdf': 1}:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void EmptyStringsAreFalsy()
        {
            var input = "x = False\nif '':\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NonEmptyStringsAreTruthy()
        {
            var input = "x = False\nif 'asdf':\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ZeroIntegerIsFalsy()
        {
            var input = "x = False\nif 0:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NonZeroIntegerIsTruthy()
        {
            var input = "x = False\nif 1:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void ZeroFloatIsFalsy()
        {
            var input = "x = False\nif 0.0:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NonZeroFloatIsTruthy()
        {
            var input = "x = False\nif 1.5:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void FalseIsFalsy()
        {
            var input = "x = False\nif False:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void TrueIsTruthy()
        {
            var input = "x = False\nif True:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NoneIsFalsy()
        {
            var input = "x = False\nif None:\n\tx = True";
            var memory = Runner.Run(input);
            var expected = memory.CreateBsType(Memory.BsTypes.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}