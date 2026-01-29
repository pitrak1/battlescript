using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class BindingInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void Basic()
        {
            var input = "__btl_numeric__";
            var expected = new BindingInstruction("__btl_numeric__");
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParenthesesWithoutArguments()
        {
            var input = "__btl_string__()";
            var expected = new BindingInstruction("__btl_string__", new List<Instruction>());
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
        
        [Test]
        public void ParenthesesWithArguments()
        {
            var input = "__btl_mapping__(asdf, qwer)";
            var expected = new BindingInstruction(
                "__btl_mapping__",
                [new VariableInstruction("asdf"), new VariableInstruction("qwer")]
            );
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Interpret
    {
        [Test]
        public void BtlNumeric()
        {
            var (callStack, closure) = Runner.Run("x = __btl_numeric__()");
            var expected = new NumericVariable(0);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void BtlSequence()
        {
            var (callStack, closure) = Runner.Run("x = __btl_sequence__()");
            var expected = new SequenceVariable();
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void BtlMapping()
        {
            var (callStack, closure) = Runner.Run("x = __btl_mapping__()");
            var expected = new MappingVariable();
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
        
        [Test]
        public void BtlString()
        {
            var (callStack, closure) = Runner.Run("x = __btl_string__()");
            var expected = new StringVariable("");
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [TestFixture]
        public class BtlNumericWithArguments
        {
            [Test]
            public void NoArguments()
            {
                var (callStack, closure) = Runner.Run("x = __btl_numeric__()");
                var expected = new NumericVariable(0);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }

            [Test]
            public void IntString()
            {
                var (callStack, closure) = Runner.Run("x = __btl_numeric__('4')");
                var expected = new NumericVariable(4);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }
            
            [Test]
            public void FloatString()
            {
                var (callStack, closure) = Runner.Run("x = __btl_numeric__('4.6')");
                var expected = new NumericVariable(4.6);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }

            [Test]
            public void Variable()
            {
                var (callStack, closure) = Runner.Run("y = 5\nx = __btl_numeric__(y)");
                var expected = new NumericVariable(5);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }
            
            [Test]
            public void Truncate()
            {
                var (callStack, closure) = Runner.Run("x = __btl_numeric__(4.6, True)");
                var expected = new NumericVariable(4);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }
        }

        [TestFixture]
        public class BtlStringWithArguments()
        {
            [Test]
            public void NoArguments()
            {
                var (callStack, closure) = Runner.Run("x = __btl_string__()");
                var expected = new StringVariable("");
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }

            [Test]
            public void Int()
            {
                var (callStack, closure) = Runner.Run("x = __btl_string__(4)");
                var expected = new StringVariable("4");
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }
            
            [Test]
            public void Float()
            {
                var (callStack, closure) = Runner.Run("x = __btl_string__(4.6)");
                var expected = new StringVariable("4.6");
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
            }
        }
    }
}