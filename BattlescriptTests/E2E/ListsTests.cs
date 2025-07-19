using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ListsTests
{
    [TestFixture]
    public class InNotIn
    {
        [Test]
        public void InReturnsTrueIfValueIsInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 2 in y
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", true);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void InReturnsFalseIfValueIsNotInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 4 in y
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", false);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }

        [Test]
        public void NotInReturnsTrueIfValueIsNotInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 4 not in y
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", true);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void NotInReturnsFalseIfValueIsInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 2 not in y
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", false);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
    }
    
    [TestFixture]
    public class IsIsNot
    {
        [Test]
        public void IsReturnsTrueIfListsAreSameInstance()
        {
            var input = """
                        y = [1, 2, 3]
                        z = y
                        x = y is z
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", true);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void IsReturnsFalseIfListsAreDifferentInstances()
        {
            var input = """
                        y = [1, 2, 3]
                        z = [1, 2, 3]
                        x = y is z
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", false);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }

        [Test]
        public void IsNotReturnTrueIfListsAreDifferentInstances()
        {
            var input = """
                        y = [1, 2, 3]
                        z = [1, 2, 3]
                        x = y is not z
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", true);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void IsNotReturnFalseIfListsAreSameInstance()
        {
            var input = """
                        y = [1, 2, 3]
                        z = y
                        x = y is not z
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "bool", false);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
    }

    [TestFixture]
    public class Concatenation()
    {
        [Test]
        public void BasicConcatenation()
        {
            var input = """
                        y = [1, 2, 3]
                        z = [4, 5, 6]
                        x = y + z
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>()
            {
                new NumericVariable(1),
                new NumericVariable(2),
                new NumericVariable(3),
                new NumericVariable(4),
                new NumericVariable(5),
                new NumericVariable(6)
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void MultiplyConcatenation()
        {
            var input = """
                        y = [1, 2, 3]
                        x = y * 3
                        z = 3 * y
                        """;
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>()
            {
                new NumericVariable(1),
                new NumericVariable(2),
                new NumericVariable(3),
                new NumericVariable(1),
                new NumericVariable(2),
                new NumericVariable(3),
                new NumericVariable(1),
                new NumericVariable(2),
                new NumericVariable(3),
            });
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["z"], expected);
        }
    }
}