using Battlescript;

namespace BattlescriptTests.InterpreterTests.Variables;

[TestFixture]
public class ParameterSetTests
{
    [Test]
    public void SupportsParametersWithoutDefaultValues()
    {
        var parameterInstructions = new List<Instruction>()
        {
            new VariableInstruction("asdf"), new VariableInstruction("qwer")
        };
        var parameters = new ParameterSet(parameterInstructions);
        var (callStack, closure) = Runner.Run("");
        var result = parameters.GetVariableDictionary(callStack, closure);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", null },
            { "qwer", null }
        };
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void SupportsParametersWithDefaultValues()
    {
        var parameterInstructions = new List<Instruction>()
        {
            new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)), 
            new AssignmentInstruction("=", new VariableInstruction("qwer"), new NumericInstruction(5678))
        };
        var parameters = new ParameterSet(parameterInstructions);
        var (callStack, closure) = Runner.Run("");
        var result = parameters.GetVariableDictionary(callStack, closure);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
            { "qwer", BtlTypes.Create(BtlTypes.Types.Int, 5678) }
        };
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void SupportsMixedParameters()
    {
        var parameterInstructions = new List<Instruction>()
        {
            new VariableInstruction("asdf"), 
            new AssignmentInstruction("=", new VariableInstruction("qwer"), new NumericInstruction(5678))
        };
        var parameters = new ParameterSet(parameterInstructions);
        var (callStack, closure) = Runner.Run("");
        var result = parameters.GetVariableDictionary(callStack, closure);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", null },
            { "qwer", BtlTypes.Create(BtlTypes.Types.Int, 5678) }
        };
        Assert.That(result, Is.EquivalentTo(expected));
    }
    
    [Test]
    public void ThrowsErrorIfDefaultParametersBeforeNonDefaultParameters()
    {
        var parameterInstructions = new List<Instruction>()
        {
            new AssignmentInstruction("=", new VariableInstruction("asdf"), new NumericInstruction(1234)), 
            new VariableInstruction("qwer")
        };
        Assert.Throws<InterpreterRequiredParamFollowsDefaultParamException>(() => new ParameterSet(parameterInstructions));
    }
}