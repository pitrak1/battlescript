using Battlescript;


namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public class BsTypesTests
{
    [TestFixture]
    public class IsTests
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsTrueForMatchingType()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            Assert.That(BsTypes.Is(BsTypes.Types.Int, intVar), Is.True);
        }

        [Test]
        public void ReturnsFalseForNonMatchingType()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            Assert.That(BsTypes.Is(BsTypes.Types.Float, intVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            Assert.That(BsTypes.Is(BsTypes.Types.Int, numericVar), Is.False);
        }
    }

    [TestFixture]
    public class Create
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void CreatesIntFromIntValue()
        {
            var result = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.That(BsTypes.Is(BsTypes.Types.Int, result), Is.True);
            Assert.That(BsTypes.GetIntValue(result), Is.EqualTo(42));
        }

        [Test]
        public void CreatesFloatFromDoubleValue()
        {
            var result = BsTypes.Create(BsTypes.Types.Float, 3.14);
            
            Assert.That(BsTypes.Is(BsTypes.Types.Float, result), Is.True);
            Assert.That(BsTypes.GetFloatValue(result), Is.EqualTo(3.14));
        }

        [Test]
        public void CreatesBoolFromTrue()
        {
            var result = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assert.That(BsTypes.Is(BsTypes.Types.Bool, result), Is.True);
            Assert.That(BsTypes.GetBoolValue(result), Is.True);
        }

        [Test]
        public void CreatesBoolFromFalse()
        {
            var result = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assert.That(BsTypes.Is(BsTypes.Types.Bool, result), Is.True);
            Assert.That(BsTypes.GetBoolValue(result), Is.False);
        }

        [Test]
        public void CreatesListFromListOfVariables()
        {
            var listValues = new List<Variable>
            {
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2),
                BsTypes.Create(BsTypes.Types.Int, 3)
            };
            var result = BsTypes.Create(BsTypes.Types.List, listValues);
            
            Assert.That(BsTypes.Is(BsTypes.Types.List, result), Is.True);
            var listValue = BsTypes.GetListValue(result);
            Assert.That(listValue.Values.Count, Is.EqualTo(3));
        }

        [Test]
        public void CreatesStringFromStringValue()
        {
            var result = BsTypes.Create(BsTypes.Types.String, "hello");
            
            Assert.That(BsTypes.Is(BsTypes.Types.String, result), Is.True);
            Assert.That(BsTypes.GetStringValue(result), Is.EqualTo("hello"));
        }
    }

    [TestFixture]
    public class CreateException
    {
        private CallStack _callStack;
        private Closure _closure;

        [SetUp]
        public void SetUp()
        {
            (_callStack, _closure) = Runner.Run("");
        }

        [Test]
        public void CreatesExceptionWithMessage()
        {
            var result = BsTypes.CreateException(_callStack, _closure, "ValueError", "Test error message");
            
            Assert.That(result, Is.InstanceOf<ObjectVariable>());
            Assert.That(BsTypes.GetErrorMessage(result), Is.EqualTo("Test error message"));
        }

        [Test]
        public void ThrowsExceptionForInvalidExceptionType()
        {
            var nonExistentVar = new NumericVariable(42);
            _closure.SetVariable(_callStack, new VariableInstruction("InvalidType"), nonExistentVar);
            
            Assert.Throws<Exception>(() => 
                BsTypes.CreateException(_callStack, _closure, "InvalidType", "message"));
        }
    }

    [TestFixture]
    public class GetIntValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsIntValueForIntVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.That(BsTypes.GetIntValue(intVar), Is.EqualTo(42));
        }

        [Test]
        public void ThrowsExceptionForNonIntVariable()
        {
            var floatVar = BsTypes.Create(BsTypes.Types.Float, 3.14);
            
            Assert.Throws<Exception>(() => BsTypes.GetIntValue(floatVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.Throws<Exception>(() => BsTypes.GetIntValue(numericVar));
        }
    }

    [TestFixture]
    public class GetFloatValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsFloatValueForFloatVariable()
        {
            var floatVar = BsTypes.Create(BsTypes.Types.Float, 3.14);
            
            Assert.That(BsTypes.GetFloatValue(floatVar), Is.EqualTo(3.14));
        }

        [Test]
        public void ThrowsExceptionForNonFloatVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetFloatValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(3.14);
            
            Assert.Throws<Exception>(() => BsTypes.GetFloatValue(numericVar));
        }
    }

    [TestFixture]
    public class GetBoolValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsTrueForTrueBoolVariable()
        {
            var boolVar = BsTypes.Create(BsTypes.Types.Bool, true);
            
            Assert.That(BsTypes.GetBoolValue(boolVar), Is.True);
        }

        [Test]
        public void ReturnsFalseForFalseBoolVariable()
        {
            var boolVar = BsTypes.Create(BsTypes.Types.Bool, false);
            
            Assert.That(BsTypes.GetBoolValue(boolVar), Is.False);
        }

        [Test]
        public void ThrowsExceptionForNonBoolVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetBoolValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(1);
            
            Assert.Throws<Exception>(() => BsTypes.GetBoolValue(numericVar));
        }
    }

    [TestFixture]
    public class GetListValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsSequenceVariableForListVariable()
        {
            var listValues = new List<Variable>
            {
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2)
            };
            var listVar = BsTypes.Create(BsTypes.Types.List, listValues);
            
            var result = BsTypes.GetListValue(listVar);
            
            Assert.That(result, Is.InstanceOf<SequenceVariable>());
            Assert.That(result.Values.Count, Is.EqualTo(2));
        }

        [Test]
        public void ThrowsExceptionForNonListVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetListValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var seqVar = new SequenceVariable();
            
            Assert.Throws<Exception>(() => BsTypes.GetListValue(seqVar));
        }
    }

    [TestFixture]
    public class GetDictValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsMappingVariableForDictVariable()
        {
            var dictIntValues = new Dictionary<int, Variable>
            {
                {1, BsTypes.Create(BsTypes.Types.Int, 1)},
                {2, BsTypes.Create(BsTypes.Types.Int, 2)}
            };
            var dictStringValues = new Dictionary<string, Variable>
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 1)},
                {"qwer", BsTypes.Create(BsTypes.Types.Int, 2)}
            };
            var dictValues = new MappingVariable(dictIntValues, dictStringValues);
            var dictVar = BsTypes.Create(BsTypes.Types.Dictionary, dictValues);
            
            var result = BsTypes.GetDictValue(dictVar);
            
            Assert.That(result, Is.InstanceOf<MappingVariable>());
            Assert.That(result.IntValues, Is.EquivalentTo(dictIntValues));
            Assert.That(result.StringValues, Is.EquivalentTo(dictStringValues));
        }

        [Test]
        public void ThrowsExceptionForNonDictVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetDictValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var mappingVar = new MappingVariable();
            
            Assert.Throws<Exception>(() => BsTypes.GetDictValue(mappingVar));
        }
    }

    [TestFixture]
    public class GetStringValue
    {
        [SetUp]
        public void SetUp()
        {
            Runner.Run("");
        }

        [Test]
        public void ReturnsStringValueForStringVariable()
        {
            var stringVar = BsTypes.Create(BsTypes.Types.String, "hello world");
            
            Assert.That(BsTypes.GetStringValue(stringVar), Is.EqualTo("hello world"));
        }

        [Test]
        public void ThrowsExceptionForNonStringVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetStringValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var stringVar = new StringVariable("test");
            
            Assert.Throws<Exception>(() => BsTypes.GetStringValue(stringVar));
        }
    }

    [TestFixture]
    public class GetErrorMessage
    {
        private CallStack _callStack;
        private Closure _closure;

        [SetUp]
        public void SetUp()
        {
            (_callStack, _closure) = Runner.Run("");
        }

        [Test]
        public void ReturnsErrorMessageForException()
        {
            var exception = BsTypes.CreateException(_callStack, _closure, "ValueError", "Test error");
            
            Assert.That(BsTypes.GetErrorMessage(exception), Is.EqualTo("Test error"));
        }

        [Test]
        public void ThrowsExceptionForNonExceptionVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BsTypes.GetErrorMessage(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.Throws<Exception>(() => BsTypes.GetErrorMessage(numericVar));
        }
    }

    [TestFixture]
    public class IsException
    {
        private CallStack _callStack;
        private Closure _closure;

        [SetUp]
        public void SetUp()
        {
            (_callStack, _closure) = Runner.Run("");
        }

        [Test]
        public void ReturnsTrueForExceptionVariable()
        {
            var exception = BsTypes.CreateException(_callStack, _closure, "ValueError", "Test");
            
            Assert.That(BsTypes.IsException(exception), Is.True);
        }

        [Test]
        public void ReturnsTrueForSubclassException()
        {
            var exception = BsTypes.CreateException(_callStack, _closure, "ValueError", "Test");
            
            // ValueError is a subclass of Exception
            Assert.That(BsTypes.IsException(exception), Is.True);
        }

        [Test]
        public void ReturnsFalseForNonExceptionVariable()
        {
            var intVar = BsTypes.Create(BsTypes.Types.Int, 42);
            
            Assert.That(BsTypes.IsException(intVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.That(BsTypes.IsException(numericVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNullVariable()
        {
            Assert.That(BsTypes.IsException(null!), Is.False);
        }
    }
}
