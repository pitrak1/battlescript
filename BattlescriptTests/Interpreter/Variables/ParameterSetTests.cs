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
        var memory = Runner.Run("");
        var result = parameters.GetVariableDictionary(memory);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", null },
            { "qwer", null }
        };
        Assertions.AssertVariableDictionariesEqual(result, expected);
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
        var memory = Runner.Run("");
        var result = parameters.GetVariableDictionary(memory);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", memory.Create(Memory.BsTypes.Int, 1234) },
            { "qwer", memory.Create(Memory.BsTypes.Int, 5678) }
        };
        Assertions.AssertVariableDictionariesEqual(result, expected);
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
        var memory = Runner.Run("");
        var result = parameters.GetVariableDictionary(memory);
        var expected = new Dictionary<string, Variable?>()
        {
            { "asdf", null },
            { "qwer", memory.Create(Memory.BsTypes.Int, 5678) }
        };
        Assertions.AssertVariableDictionariesEqual(result, expected);
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