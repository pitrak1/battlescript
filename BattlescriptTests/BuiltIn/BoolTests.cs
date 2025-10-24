using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class BoolTests
{
    [Test]
    public void TrueIsNumericOne()
    {
        var memory = Runner.Run("""
                                x = True
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new NumericVariable(1));
    }
    
    [Test]
    public void FalseIsNumericZero()
    {
        var memory = Runner.Run("""
                                x = False
                                y = x.__value
                                """);
        Assertions.AssertVariable(memory, "y", new NumericVariable(0));
    }
    
    public class Conversions
    {
        [Test]
        public void EmptyStringsAreFalse()
        {
            var memory = Runner.Run("""
                                    x = ""
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonEmptyStringsAreTrue()
        {
            var memory = Runner.Run("""
                                    x = "asdf"
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void ZeroIntIsFalse()
        {
            var memory = Runner.Run("""
                                    x = 0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonZeroIntIsTrue()
        {
            var memory = Runner.Run("""
                                    x = 1
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void ZeroFloatIsFalse()
        {
            var memory = Runner.Run("""
                                    x = 0.0
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void NonZeroFloatIsTrue()
        {
            var memory = Runner.Run("""
                                    x = 1.5
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
        
        [Test]
        public void FalseIsFalse()
        {
            var memory = Runner.Run("""
                                    x = False
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(0)));
        }
        
        [Test]
        public void TrueIsTrue()
        {
            var memory = Runner.Run("""
                                    x = True
                                    y = bool(x)
                                    """);
            Assertions.AssertVariable(memory, "y", BsTypes.Create(BsTypes.Types.Bool, new NumericVariable(1)));
        }
    }
}