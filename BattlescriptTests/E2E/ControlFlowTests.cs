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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFalseIfStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsElseStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelse:\n\tx = 7";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 7);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    
        [Test]
        public void SupportsElifStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9\nelse:\n\tx = 7";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsElifStatementWithoutElse()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6\nelif x < 8:\n\tx = 9";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsConsecutiveIfStatements()
        {
            var input = "x = 5\nif x < 6:\n\tx = 7\nif x >= 7:\n\tx = 9";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class While
    {
        [Test]
        public void HandlesTrueWhileStatement()
        {
            var input = "x = 5\nwhile x < 10:\n\tx += 1";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 10);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesFalseWhileStatement()
        {
            var input = "x = 5\nwhile x == 6:\n\tx = 10";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class Range
    {
        [Test]
        public void HandlesSingleArgument()
        {
            var input = "x = range(5)";
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 0),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
            ]);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesTwoArguments()
        {
            var input = "x = range(2, 5)";
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
            ]);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesThreeArguments()
        {
            var input = "x = range(2, 10, 2)";
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8),
            ]);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesCountNotMatchingStep()
        {
            var input = "x = range(2, 5, 2)";
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
            ]);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesDecreasingRange()
        {
            var input = "x = range(2, -5, -2)";
            var memory = Runner.Run(input);
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 0),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -2),
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -4),
            ]);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ReturnsEmptyListIfGivenInfiniteRange()
        {
            var input = "x = range(2, -5, 2)";
            var expected = new ListVariable([]);
            var memory = Runner.Run(input);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class For
    {
        [Test]
        public void ForLoopUsingRange()
        {
            var input = "x = 5\nfor y in range(3):\n\tx = x + y";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ForLoopUsingList()
        {
            var input = "x = 5\nfor y in [-1, 3, 2]:\n\tx = x + y";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 13);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
    }
}