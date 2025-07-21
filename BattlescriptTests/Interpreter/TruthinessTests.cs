using Battlescript;

namespace BattlescriptTests.InterpreterTests;
    
[TestFixture]
public class TruthinessTests
{
    [TestFixture]
    public class NonObjectValues
    {
        private Memory _memory;

        [SetUp]
        public void SetUp()
        {
            _memory = Runner.Run(""); 
        }
        
        [Test]
        public void NonZeroNumericVariableReturnsTrue()
        {
            var variable = new NumericVariable(5);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroNumericVariableReturnsFalse()
        {
            var variable = new NumericVariable(0);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonEmptyStringVariableReturnsTrue()
        {
            var variable = new StringVariable("Hello");
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyStringVariableReturnsFalse()
        {
            var variable = new StringVariable("");

            var result = Truthiness.IsTruthy(_memory, variable);

            Assert.IsFalse(result);
        }

        [Test]
        public void ClassVariableReturnsTrue()
        {
            var variable = new ClassVariable("", null);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void DictionaryVariableWithIntValuesReturnsTrue()
        {
            var variable = new DictionaryVariable(new Dictionary<int, Variable>()
            {
                {1, new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void DictionaryVariableWithStringValuesReturnsTrue()
        {
            var variable = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }
        
        [Test]
        public void EmptyDictionaryVariableReturnsTrue()
        {
            var variable = new DictionaryVariable();
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void FunctionVariableReturnsTrue()
        {
            var variable = new FunctionVariable(null, null);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }
    }

    [TestFixture]
    public class ObjectValues
    {
        private Memory _memory;

        [SetUp]
        public void SetUp()
        {
            _memory = Runner.Run(""); 
        }
        
        [Test]
        public void NonBuiltinObjectVariableReturnsTrue()
        {
            var variable = new ObjectVariable(null, new ClassVariable("", null));
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void NonZeroIntReturnsTrue()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Int, 4);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroIntReturnsFalse()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Int, 0);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonZeroFloatReturnsTrue()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Float, 4.5);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroFloatReturnsFalse()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Float, 0.0);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void TrueBoolReturnsTrue()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void FalseBoolReturnsFalse()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void NonEmptyListReturnsTrue()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable(new List<Variable?>()
            {
                new NumericVariable(1)
            }));
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyListReturnsFalse()
        {
            var variable = BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable(new List<Variable?>()));
            var result = Truthiness.IsTruthy(_memory, variable);
            Assert.IsFalse(result);
        }
    }
}
