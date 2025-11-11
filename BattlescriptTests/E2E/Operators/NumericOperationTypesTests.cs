using Battlescript;

namespace BattlescriptTests.E2ETests.Operators;

public class NumericOperationTypesTests
{
    [Test]
    public void TrueDivisionReturnsFloat()
    {
        var input = "x = 4 / 2";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Float, 2);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }

    [Test] 
    public void FloorDivisionReturnsInt()
    {
        var input = "x = 4.9 // 1.4";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, 3);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test] 
    public void TwoFloatsReturnsFloat()
    {
        var input = "x = 0.5 * 4.0";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Float, 2);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test] 
    public void OneFloatReturnsFloat()
    {
        var input = "x = 0.5 * 4";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Float, 2);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
    
    [Test] 
    public void NoFloatsReturnsInt()
    {
        var input = "x = 2 * 4";
        var (callStack, closure) = Runner.Run(input);
        var expected = BsTypes.Create(BsTypes.Types.Int, 8);
        
        Assertions.AssertVariable(callStack, closure, "x", expected);
    }
}