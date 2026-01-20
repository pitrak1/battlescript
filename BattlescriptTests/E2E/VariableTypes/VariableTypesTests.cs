using Battlescript;

namespace BattlescriptTests.E2ETests;

public class VariableTypesTests
{
    [Test]
    public void SupportsStrings()
    {
        var input = "x = 'asdf'";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf");
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsFloats()
    {
        var input = "x = 5.5";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Float, 5.5);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsIntegers()
    {
        var input = "x = 5";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test]
    public void SupportsBooleans()
    {
        var input = "x = True";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Bool, 1);
        
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsLists()
    {
        var input = "x = []";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsDictionaries()
    {
        var input = "x = {}";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable([]));
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test]
    public void SupportsClasses()
    {
        var input = "class asdf():\n\ty = 3";
        var (callStack, closure) = Runner.Run(input);
        var expected = new ClassVariable("asdf", new Dictionary<string, Variable>
        {
            {"y", BtlTypes.Create(BtlTypes.Types.Int, 3)}
        }, closure);
        Assertions.AssertVariable(callStack, closure, "asdf", expected);
    }
    
    [Test]
    public void SupportsObjects()
    {
        var input = "class asdf():\n\ty = 3\nx = asdf()";
        var (callStack, closure) = Runner.Run(input);
        var classValues = new Dictionary<string, Variable>
        {
            { "y", BtlTypes.Create(BtlTypes.Types.Int, 3) }
        };
        var expected = new ObjectVariable(classValues, new ClassVariable("asdf", classValues, closure));
        
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}