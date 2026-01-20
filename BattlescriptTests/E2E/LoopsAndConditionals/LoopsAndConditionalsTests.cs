using Battlescript;

namespace BattlescriptTests.E2ETests.LoopsAndConditionals;

public class LoopsAndConditionalsTests
{
    [TestFixture]
    public class IfElifElse
    {
        [Test]
        public void TrueIfStatement()
        {
            var input = """
                        x = 5
                        if x == 5:
                            x = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void FalseIfStatement()
        {
            var input = """
                        x = 5
                        if x == 6:
                            x = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ElseStatement()
        {
            var input = """
                        x = 5
                        if x == 6:
                            x = 6
                        else:
                            x = 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 7);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        [Test]
        public void ElifStatement()
        {
            var input = """
                        x = 5
                        if x == 6:
                            x = 6
                        elif x < 8:
                            x = 9
                        else:
                            x = 7
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ElifStatementWithoutElse()
        {
            var input = """
                        x = 5
                        if x == 6:
                            x = 6
                        elif x < 8:
                            x = 9
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ConsecutiveIfStatements()
        {
            var input = """
                        x = 5
                        if x < 6:
                            x = 7
                        if x >= 7:
                            x = 9
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }

        [Test]
        public void FalsyValuesAsExpressions()
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
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
        
        [Test]
        public void TruthyValuesAsExpressions()
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
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
    }
    
    [TestFixture]
    public class While
    {
        [Test]
        public void TrueWhileStatement()
        {
            var input = """
                        x = 5
                        while x < 10:
                            x += 1
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 10);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void FalseWhileStatement()
        {
            var input = """
                        x = 5
                        while x == 6:
                            x = 10
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
    
    [TestFixture]
    public class For
    {
        [Test]
        public void ForLoopUsingRange()
        {
            var input = """
                        x = 5
                        for y in range(3):
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 8);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void ForLoopUsingList()
        {
            var input = """
                        x = 5
                        for y in [-1, 3, 2]:
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
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
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        [Test]
        public void WhileSupport()
        {
            var input = """
                        x = 5
                        y = 0
                        while y < 4:
                            y = y + 1
                            if y == 2:
                                continue
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 13);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
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
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    
        [Test]
        public void WhileSupport()
        {
            var input = """
                        x = 5
                        y = 0
                        while y < 4:
                            y = y + 1
                            if y == 2:
                                break
                            x = x + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
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
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}