using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class BoolTests
{
    [Test]
    public void TrueIsNumericOne()
    {
        var (callStack, closure) = Runner.Run("""
                                x = True
                                y = x.__value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new NumericVariable(1));
    }
    
    [Test]
    public void FalseIsNumericZero()
    {
        var (callStack, closure) = Runner.Run("""
                                x = False
                                y = x.__value
                                """);
        Assertions.AssertVariable(callStack, closure, "y", new NumericVariable(0));
    }
    
    public class Conversions
    {
        [Test]
        public void EmptyStringsAreFalse()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = ""
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonEmptyStringsAreTrue()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = "asdf"
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void ZeroIntIsFalse()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonZeroIntIsTrue()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void ZeroFloatIsFalse()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 0.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonZeroFloatIsTrue()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = 1.5
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIsFalse()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = False
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void TrueIsTrue()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = True
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
    }
}