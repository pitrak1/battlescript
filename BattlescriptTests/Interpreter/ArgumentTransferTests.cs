using Battlescript;

namespace BattlescriptTests.InterpreterTests;

public class ArgumentTransferTests
{
    public class GetVariableDictionary
    {
        [Test]
        public void PositionalArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new StringInstruction("asdf")
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void KeywordArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void MixedArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void RespectsDefaultValues()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234)
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void PrioritizedGivenValuesOverDefaultValues()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new NumericInstruction(5678)
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "qwer", BtlTypes.Create(BtlTypes.Types.Int, 5678) }
            };
            Assert.That(result, Is.EquivalentTo(expected));
        }

        [Test]
        public void ThrowsErrorIfTooManyArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new NumericInstruction(5678),
                new NumericInstruction(9012)
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            Assert.Throws<InterpreterUnknownPositionalArgumentException>(() => ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
        }

        [Test]
        public void ThrowsErrorIfUnknownKeywordArgument()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("zxcv"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            Assert.Throws<InterpreterUnknownKeywordArgumentException>(() => ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
        }

        [Test]
        public void ThrowsErrorIfNotAllParametersHaveValues()
        {
            var (callStack, closure) = Runner.Run("");
            var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("zxcv"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer"),
                new VariableInstruction("zxcv")
            });
            Assert.Throws<InterpreterMissingRequiredArgumentException>(() => ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
        }
    }
}
