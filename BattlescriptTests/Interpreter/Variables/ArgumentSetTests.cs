using Battlescript;

namespace BattlescriptTests.InterpreterTests.Variables;

[TestFixture]
public class ArgumentSetTests
{
    public class Constructors
    {
        [Test]
        public void SupportsVariableArguments()
        {
            var expected = new List<Variable>()
            {
                new NumericVariable(1234),
                new StringVariable("asdf")
            };
            var result = new ArgumentSet(expected);
            Assertions.AssertVariableListsEqual(result.Positionals, expected);
        }
        
        [Test]
        public void SupportsPositionalInstructionArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new List<Instruction>()
            {
                new NumericInstruction(1234),
                new StringInstruction("asdf")
            };
            var result = new ArgumentSet(callStack, closure, input);
            var expected = new List<Variable>()
            {
                BsTypes.Create(BsTypes.Types.Int, 1234),
                BsTypes.Create(BsTypes.Types.String, "asdf")
            };
            Assertions.AssertVariableListsEqual(result.Positionals, expected);
        }
        
        [Test]
        public void SupportsKeywordInstructionArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            };
            var result = new ArgumentSet(callStack, closure, input);
            var expected = new Dictionary<string, Variable>()
            {
                {"asdf", BsTypes.Create(BsTypes.Types.Int, 1234)},
                {"qwer", BsTypes.Create(BsTypes.Types.String, "asdf")}
            };
            Assertions.AssertVariableDictionariesEqual(result.Keywords, expected);
        }
        
        [Test]
        public void SupportsMixedInstructionArguments()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            };
            var result = new ArgumentSet(callStack, closure, input);
            var expectedPositionals = new List<Variable>() { BsTypes.Create(BsTypes.Types.Int, 1234) };
            var expectedKeywords = new Dictionary<string, Variable>()
            {
                {"qwer", BsTypes.Create(BsTypes.Types.String, "asdf")}
            };
            Assertions.AssertVariableListsEqual(result.Positionals, expectedPositionals);
            Assertions.AssertVariableDictionariesEqual(result.Keywords, expectedKeywords);
        }

        [Test]
        public void ThrowsErrorIfKeywordArgsBeforePositionalArgs()
        {
            var (callStack, closure) = Runner.Run("");
            var input = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new StringInstruction("asdf")
            };
            Assert.Throws<InterpreterKeywordArgBeforePositionalArgException>(() => new ArgumentSet(callStack, closure, input));
        }
    }
    
    public class GetVariableDictionary
    {
        [Test]
        public void HandlesPositionalArguments()
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
            var result = arguments.GetVariableDictionary(callStack, closure, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 1234) },
                { "qwer", BsTypes.Create(BsTypes.Types.String, "asdf") }
            };
            Assertions.AssertVariableDictionariesEqual(result, expected);
        }
        
        [Test]
        public void HandlesKeywordArguments()
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
            var result = arguments.GetVariableDictionary(callStack, closure, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 1234) },
                { "qwer", BsTypes.Create(BsTypes.Types.String, "asdf") }
            };
            Assertions.AssertVariableDictionariesEqual(result, expected);
        }
        
        [Test]
        public void HandlesMixedArguments()
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
            var result = arguments.GetVariableDictionary(callStack, closure, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 1234) },
                { "qwer", BsTypes.Create(BsTypes.Types.String, "asdf") }
            };
            Assertions.AssertVariableDictionariesEqual(result, expected);
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
            var result = arguments.GetVariableDictionary(callStack, closure, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 1234) },
                { "qwer", BsTypes.Create(BsTypes.Types.String, "asdf") }
            };
            Assertions.AssertVariableDictionariesEqual(result, expected);
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
            var result = arguments.GetVariableDictionary(callStack, closure, parameters);
            var expected = new Dictionary<string, Variable>()
            {
                { "asdf", BsTypes.Create(BsTypes.Types.Int, 1234) },
                { "qwer", BsTypes.Create(BsTypes.Types.Int, 5678) }
            };
            Assertions.AssertVariableDictionariesEqual(result, expected);
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
            Assert.Throws<Exception>(() => arguments.GetVariableDictionary(callStack, closure, parameters));
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
            Assert.Throws<Exception>(() => arguments.GetVariableDictionary(callStack, closure, parameters));
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
            Assert.Throws<InterpreterMissingRequiredArgumentException>(() => arguments.GetVariableDictionary(callStack, closure, parameters));
        }
    }
}