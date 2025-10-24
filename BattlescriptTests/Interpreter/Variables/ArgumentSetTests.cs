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
            var memory = Runner.Run("");
            var input = new List<Instruction>()
            {
                new NumericInstruction(1234),
                new StringInstruction("asdf")
            };
            var result = new ArgumentSet(memory, input);
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
            var memory = Runner.Run("");
            var input = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            };
            var result = new ArgumentSet(memory, input);
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
            var memory = Runner.Run("");
            var input = new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            };
            var result = new ArgumentSet(memory, input);
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
            var memory = Runner.Run("");
            var input = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new StringInstruction("asdf")
            };
            Assert.Throws<InterpreterKeywordArgBeforePositionalArgException>(() => new ArgumentSet(memory, input));
        }
    }
    
    public class GetVariableDictionary
    {
        [Test]
        public void HandlesPositionalArguments()
        {
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new StringInstruction("asdf")
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = arguments.GetVariableDictionary(memory, parameters);
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
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = arguments.GetVariableDictionary(memory, parameters);
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
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new VariableInstruction("qwer")
            });
            var result = arguments.GetVariableDictionary(memory, parameters);
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
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new NumericInstruction(1234)
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var result = arguments.GetVariableDictionary(memory, parameters);
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
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new NumericInstruction(5678)
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            var result = arguments.GetVariableDictionary(memory, parameters);
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
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
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
            Assert.Throws<Exception>(() => arguments.GetVariableDictionary(memory, parameters));
        }
        
        [Test]
        public void ThrowsErrorIfUnknownKeywordArgument()
        {
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
            {
                new NumericInstruction(1234),
                new AssignmentInstruction("=", new VariableInstruction("zxcv"), new StringInstruction("asdf"))
            });
            var parameters = new ParameterSet(new List<Instruction>()
            {
                new VariableInstruction("asdf"),
                new AssignmentInstruction("=", new VariableInstruction("qwer"), new StringInstruction("asdf"))
            });
            Assert.Throws<Exception>(() => arguments.GetVariableDictionary(memory, parameters));
        }
        
        [Test]
        public void ThrowsErrorIfNotAllParametersHaveValues()
        {
            var memory = Runner.Run("");
            var arguments = new ArgumentSet(memory, new List<Instruction>()
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
            Assert.Throws<InterpreterMissingRequiredArgumentException>(() => arguments.GetVariableDictionary(memory, parameters));
        }
    }
}