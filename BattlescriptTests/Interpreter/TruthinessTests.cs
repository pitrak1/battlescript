using Battlescript;

namespace BattlescriptTests.InterpreterTests;
    
[TestFixture]
public class TruthinessTests
{
    [TestFixture]
    public class NonObjectValues
    {
        [Test]
        public void NonZeroNumericVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new NumericVariable(5);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroNumericVariableReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = new NumericVariable(0);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonEmptyStringVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new StringVariable("Hello");
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyStringVariableReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = new StringVariable("");

            var result = Truthiness.IsTruthy(memory, variable);

            Assert.IsFalse(result);
        }

        [Test]
        public void ClassVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new ClassVariable("", null);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void MappingVariableWithIntValuesReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new MappingVariable(new Dictionary<int, Variable>()
            {
                {1, new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void MappingVariableWithStringValuesReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }
        
        [Test]
        public void EmptyDictionaryVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new MappingVariable();
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void FunctionVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new FunctionVariable(null, null, null);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }
    }

    [TestFixture]
    public class ObjectValues
    {
        [Test]
        public void NonBuiltinObjectVariableReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = new ObjectVariable(null, new ClassVariable("", null));
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void NonZeroIntReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Int, 4);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroIntReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Int, 0);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonZeroFloatReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Float, 4.5);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroFloatReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Float, 0.0);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void TrueBoolReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Bool, true);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void FalseBoolReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.Bool, false);
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void NonEmptyListReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.List, new SequenceVariable(new List<Variable?>()
            {
                new NumericVariable(1)
            }));
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyListReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.List, new SequenceVariable(new List<Variable?>()));
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void NonEmptyStringReturnsTrue()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.String, new StringVariable("Hello"));
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyStringReturnsFalse()
        {
            var memory = Runner.Run("");
            var variable = BsTypes.Create(BsTypes.Types.String, new StringVariable(""));
            var result = Truthiness.IsTruthy(memory, variable);
            Assert.IsFalse(result);
        }
    }
}
