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
                BtlTypes.Create(BtlTypes.Types.Int, 1234),
                BtlTypes.Create(BtlTypes.Types.String, "asdf")
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
                {"asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234)},
                {"qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf")}
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
            var expectedPositionals = new List<Variable>() { BtlTypes.Create(BtlTypes.Types.Int, 1234) };
            var expectedKeywords = new Dictionary<string, Variable>()
            {
                {"qwer", BtlTypes.Create(BtlTypes.Types.String, "asdf")}
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
    
    
}