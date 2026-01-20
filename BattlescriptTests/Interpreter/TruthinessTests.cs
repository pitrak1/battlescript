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
            var (callStack, closure) = Runner.Run("");
            var variable = new NumericVariable(5);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroNumericVariableReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new NumericVariable(0);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonEmptyStringVariableReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new StringVariable("Hello");
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyStringVariableReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new StringVariable("");

            var result = Truthiness.IsTruthy(callStack, closure, variable);

            Assert.IsFalse(result);
        }

        [Test]
        public void ClassVariableReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new ClassVariable("", null, closure);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void MappingVariableWithIntValuesReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new MappingVariable(new Dictionary<int, Variable>()
            {
                {1, new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void MappingVariableWithStringValuesReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new MappingVariable(null, new Dictionary<string, Variable>()
            {
                {"asdf", new NumericVariable(10)}
            });
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }
        
        [Test]
        public void EmptyDictionaryVariableReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new MappingVariable();
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void FunctionVariableReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new FunctionVariable(null, null, null);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }
    }

    [TestFixture]
    public class ObjectValues
    {
        [Test]
        public void NonBuiltinObjectVariableReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = new ObjectVariable(null, new ClassVariable("", null, closure));
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void NonZeroIntReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Int, 4);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroIntReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Int, 0);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }

        [Test]
        public void NonZeroFloatReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Float, 4.5);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void ZeroFloatReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Float, 0.0);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void TrueBoolReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Bool, true);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void FalseBoolReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.Bool, false);
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void NonEmptyListReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable(new List<Variable?>()
            {
                new NumericVariable(1)
            }));
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyListReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable(new List<Variable?>()));
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }
        
        [Test]
        public void NonEmptyStringReturnsTrue()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.String, new StringVariable("Hello"));
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsTrue(result);
        }

        [Test]
        public void EmptyStringReturnsFalse()
        {
            var (callStack, closure) = Runner.Run("");
            var variable = BtlTypes.Create(BtlTypes.Types.String, new StringVariable(""));
            var result = Truthiness.IsTruthy(callStack, closure, variable);
            Assert.IsFalse(result);
        }
    }
}
