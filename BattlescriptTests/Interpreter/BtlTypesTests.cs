using Battlescript;


namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public class BtlTypesTests
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
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            Assert.That(BtlTypes.Is(BtlTypes.Types.Int, intVar), Is.True);
        }

        [Test]
        public void ReturnsFalseForNonMatchingType()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            Assert.That(BtlTypes.Is(BtlTypes.Types.Float, intVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            Assert.That(BtlTypes.Is(BtlTypes.Types.Int, numericVar), Is.False);
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
            var result = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.Int, result), Is.True);
            Assert.That(BtlTypes.GetIntValue(result), Is.EqualTo(42));
        }

        [Test]
        public void CreatesFloatFromDoubleValue()
        {
            var result = BtlTypes.Create(BtlTypes.Types.Float, 3.14);
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.Float, result), Is.True);
            Assert.That(BtlTypes.GetFloatValue(result), Is.EqualTo(3.14));
        }

        [Test]
        public void CreatesBoolFromTrue()
        {
            var result = BtlTypes.Create(BtlTypes.Types.Bool, true);
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.Bool, result), Is.True);
            Assert.That(BtlTypes.GetBoolValue(result), Is.True);
        }

        [Test]
        public void CreatesBoolFromFalse()
        {
            var result = BtlTypes.Create(BtlTypes.Types.Bool, false);
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.Bool, result), Is.True);
            Assert.That(BtlTypes.GetBoolValue(result), Is.False);
        }

        [Test]
        public void CreatesListFromListOfVariables()
        {
            var listValues = new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            };
            var result = BtlTypes.Create(BtlTypes.Types.List, listValues);
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.List, result), Is.True);
            var listValue = BtlTypes.GetListValue(result);
            Assert.That(listValue.Values.Count, Is.EqualTo(3));
        }

        [Test]
        public void CreatesStringFromStringValue()
        {
            var result = BtlTypes.Create(BtlTypes.Types.String, "hello");
            
            Assert.That(BtlTypes.Is(BtlTypes.Types.String, result), Is.True);
            Assert.That(BtlTypes.GetStringValue(result), Is.EqualTo("hello"));
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
            var result = BtlTypes.CreateException(_callStack, _closure, "ValueError", "Test error message");
            
            Assert.That(result, Is.InstanceOf<ObjectVariable>());
            Assert.That(BtlTypes.GetErrorMessage(result), Is.EqualTo("Test error message"));
        }

        [Test]
        public void ThrowsExceptionForInvalidExceptionType()
        {
            var nonExistentVar = new NumericVariable(42);
            _closure.SetVariable(_callStack, new VariableInstruction("InvalidType"), nonExistentVar);
            
            Assert.Throws<Exception>(() => 
                BtlTypes.CreateException(_callStack, _closure, "InvalidType", "message"));
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
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.That(BtlTypes.GetIntValue(intVar), Is.EqualTo(42));
        }

        [Test]
        public void ThrowsExceptionForNonIntVariable()
        {
            var floatVar = BtlTypes.Create(BtlTypes.Types.Float, 3.14);
            
            Assert.Throws<Exception>(() => BtlTypes.GetIntValue(floatVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetIntValue(numericVar));
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
            var floatVar = BtlTypes.Create(BtlTypes.Types.Float, 3.14);
            
            Assert.That(BtlTypes.GetFloatValue(floatVar), Is.EqualTo(3.14));
        }

        [Test]
        public void ThrowsExceptionForNonFloatVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetFloatValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(3.14);
            
            Assert.Throws<Exception>(() => BtlTypes.GetFloatValue(numericVar));
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
            var boolVar = BtlTypes.Create(BtlTypes.Types.Bool, true);
            
            Assert.That(BtlTypes.GetBoolValue(boolVar), Is.True);
        }

        [Test]
        public void ReturnsFalseForFalseBoolVariable()
        {
            var boolVar = BtlTypes.Create(BtlTypes.Types.Bool, false);
            
            Assert.That(BtlTypes.GetBoolValue(boolVar), Is.False);
        }

        [Test]
        public void ThrowsExceptionForNonBoolVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetBoolValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(1);
            
            Assert.Throws<Exception>(() => BtlTypes.GetBoolValue(numericVar));
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
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2)
            };
            var listVar = BtlTypes.Create(BtlTypes.Types.List, listValues);
            
            var result = BtlTypes.GetListValue(listVar);
            
            Assert.That(result, Is.InstanceOf<SequenceVariable>());
            Assert.That(result.Values.Count, Is.EqualTo(2));
        }

        [Test]
        public void ThrowsExceptionForNonListVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetListValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var seqVar = new SequenceVariable();
            
            Assert.Throws<Exception>(() => BtlTypes.GetListValue(seqVar));
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
                {1, BtlTypes.Create(BtlTypes.Types.Int, 1)},
                {2, BtlTypes.Create(BtlTypes.Types.Int, 2)}
            };
            var dictStringValues = new Dictionary<string, Variable>
            {
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 1)},
                {"qwer", BtlTypes.Create(BtlTypes.Types.Int, 2)}
            };
            var dictValues = new MappingVariable(dictIntValues, dictStringValues);
            var dictVar = BtlTypes.Create(BtlTypes.Types.Dictionary, dictValues);
            
            var result = BtlTypes.GetDictValue(dictVar);
            
            Assert.That(result, Is.InstanceOf<MappingVariable>());
            Assert.That(result.IntValues, Is.EquivalentTo(dictIntValues));
            Assert.That(result.StringValues, Is.EquivalentTo(dictStringValues));
        }

        [Test]
        public void ThrowsExceptionForNonDictVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetDictValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var mappingVar = new MappingVariable();
            
            Assert.Throws<Exception>(() => BtlTypes.GetDictValue(mappingVar));
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
            var stringVar = BtlTypes.Create(BtlTypes.Types.String, "hello world");
            
            Assert.That(BtlTypes.GetStringValue(stringVar), Is.EqualTo("hello world"));
        }

        [Test]
        public void ThrowsExceptionForNonStringVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetStringValue(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var stringVar = new StringVariable("test");
            
            Assert.Throws<Exception>(() => BtlTypes.GetStringValue(stringVar));
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
            var exception = BtlTypes.CreateException(_callStack, _closure, "ValueError", "Test error");
            
            Assert.That(BtlTypes.GetErrorMessage(exception), Is.EqualTo("Test error"));
        }

        [Test]
        public void ThrowsExceptionForNonExceptionVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetErrorMessage(intVar));
        }

        [Test]
        public void ThrowsExceptionForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.Throws<Exception>(() => BtlTypes.GetErrorMessage(numericVar));
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
            var exception = BtlTypes.CreateException(_callStack, _closure, "ValueError", "Test");
            
            Assert.That(BtlTypes.IsException(exception), Is.True);
        }

        [Test]
        public void ReturnsTrueForSubclassException()
        {
            var exception = BtlTypes.CreateException(_callStack, _closure, "ValueError", "Test");
            
            // ValueError is a subclass of Exception
            Assert.That(BtlTypes.IsException(exception), Is.True);
        }

        [Test]
        public void ReturnsFalseForNonExceptionVariable()
        {
            var intVar = BtlTypes.Create(BtlTypes.Types.Int, 42);
            
            Assert.That(BtlTypes.IsException(intVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNonObjectVariable()
        {
            var numericVar = new NumericVariable(42);
            
            Assert.That(BtlTypes.IsException(numericVar), Is.False);
        }

        [Test]
        public void ReturnsFalseForNullVariable()
        {
            Assert.That(BtlTypes.IsException(null!), Is.False);
        }
    }
}
