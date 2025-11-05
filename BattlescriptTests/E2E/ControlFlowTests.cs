using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ControlFlowTests
{
    [TestFixture]
    public class IfElifElse
    {
        [Test]
        public void HandlesTrueIfStatement()
        {
            var input = "x = 5\nif x == 5:\n\tx = 6";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void HandlesFalseIfStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsElseStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelse:\n\tx = 7";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 7);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        [Test]
        public void SupportsElifStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9\nelse:\n\tx = 7";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsElifStatementWithoutElse()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsConsecutiveIfStatements()
        {
            var input = "x = 5\nif x < 6:\n\tx = 7\nif x >= 7:\n\tx = 9";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void SupportsFalsyValuesAsExpressions()
        {
            var input = """
                        x = ""
                        y = 4
                        if x:
                            y = 5
                        else:
                            y = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
        
        [Test]
        public void SupportsTruthyValuesAsExpressions()
        {
            var input = """
                        x = 1234
                        y = 4
                        if x:
                            y = 5
                        else:
                            y = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
    }
    
    // [TestFixture]
    // public class While
    // {
    //     [Test]
    //     public void HandlesTrueWhileStatement()
    //     {
    //         var input = "x = 5\nwhile x < 10:\n\tx += 1";
    //         var (callStack, closure) = Runner.Run(input);
    //         var expected = BsTypes.Create(BsTypes.Types.Int, 10);
    //         Assertions.AssertVariable(callStack, closure, "x", expected);
    //     }
    //     
    //     [Test]
    //     public void HandlesFalseWhileStatement()
    //     {
    //         var input = "x = 5\nwhile x == 6:\n\tx = 10";
    //         var (callStack, closure) = Runner.Run(input);
    //         var expected = BsTypes.Create(BsTypes.Types.Int, 5);
    //         Assertions.AssertVariable(callStack, closure, "x", expected);
    //     }
    // }
    
    [TestFixture]
    public class For
    {
        [Test]
        public void ForLoopUsingRange()
        {
            var input = "x = 5\nfor y in range(3):\n\tx = x + y";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ForLoopUsingList()
        {
            var input = "x = 5\nfor y in [-1, 3, 2]:\n\tx = x + y";
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
    
    [TestFixture]
    public class Continue
    {
        [Test]
        public void ForSupport()
        {
            var input = """
                        x = 5
                        for y in range(4):
                            if y == 2:
                                continue
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        // [Test]
        // public void WhileSupport()
        // {
        //     var input = """
        //                 x = 5
        //                 y = 0
        //                 while y < 4:
        //                     y = y + 1
        //                     if y == 2:
        //                         continue
        //                     x = x + y
        //                 """;
        //     var (callStack, closure) = Runner.Run(input);
        //     var expected = BsTypes.Create(BsTypes.Types.Int, 13);
        //     Assertions.AssertVariable(callStack, closure, "x", expected);
        // }
    }
    
    [TestFixture]
    public class Break
    {
        [Test]
        public void ForSupport()
        {
            var input = """
                        x = 5
                        for y in range(4):
                            if y == 2:
                                break
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        // [Test]
        // public void WhileSupport()
        // {
        //     var input = """
        //                 x = 5
        //                 y = 0
        //                 while y < 4:
        //                     y = y + 1
        //                     if y == 2:
        //                         break
        //                     x = x + y
        //                 """;
        //     var (callStack, closure) = Runner.Run(input);
        //     var expected = BsTypes.Create(BsTypes.Types.Int, 6);
        //     Assertions.AssertVariable(callStack, closure, "x", expected);
        // }
    }
    
    [TestFixture]
    public class PassKeyword
    {
        [Test]
        public void IsSupported()
        {
            var input = """
                        x = 5
                        if x == 5:
                            pass
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}