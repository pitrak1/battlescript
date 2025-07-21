using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ListsTests
{
    [TestFixture]
    public class Indexing
    {
        [Test]
        public void SupportsListIndexing()
        {
            var input = "x = [5, '5']\ny = x[1]";
            var expected = new StringVariable("5");
            var memory = Runner.Run(input);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SupportsListRangeIndexing()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:3]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 3),
                    BsTypes.Create(memory, BsTypes.Types.Int, 2),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullStart()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[:2]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 5),
                    BsTypes.Create(memory, BsTypes.Types.Int, 3),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNullEnd()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 3),
                    BsTypes.Create(memory, BsTypes.Types.Int, 2),
                    new StringVariable("5"),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithStep()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[::2]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 5),
                    BsTypes.Create(memory, BsTypes.Types.Int, 2),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsListRangeIndexingWithNegativeStep()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[::-2]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    new StringVariable("5"),
                    BsTypes.Create(memory, BsTypes.Types.Int, 3),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsAssigningToIndices()
        {
            var input = "x = [5, 3, 2, '5']\nx[1] = 6";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 5),
                    BsTypes.Create(memory, BsTypes.Types.Int, 6),
                    BsTypes.Create(memory, BsTypes.Types.Int, 2),
                    new StringVariable("5"),
                }
            );
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsAssigningToRangeIndices()
        {
            var input = "x = [5, 3, 2, '5']\nx[1:3] = [5, 7]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(memory, BsTypes.Types.Int, 5),
                    BsTypes.Create(memory, BsTypes.Types.Int, 5),
                    BsTypes.Create(memory, BsTypes.Types.Int, 7),
                    new StringVariable("5"),
                }
            );
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
    
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
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void InReturnsFalseIfValueIsNotInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 4 in y
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
        }

        [Test]
        public void NotInReturnsTrueIfValueIsNotInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 4 not in y
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void NotInReturnsFalseIfValueIsInList()
        {
            var input = """
                        y = [1, 2, 3]
                        x = 2 not in y
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, true);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = BsTypes.Create(memory, BsTypes.Types.Bool, false);
            
            Assertions.AssertVariable(memory, "x", expected);
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
            var expected = BsTypes.Create(memory, BsTypes.Types.List, new List<Variable>()
            {
                BsTypes.Create(memory, BsTypes.Types.Int, 1),
                BsTypes.Create(memory, BsTypes.Types.Int, 2),
                BsTypes.Create(memory, BsTypes.Types.Int, 3),
                BsTypes.Create(memory, BsTypes.Types.Int, 4),
                BsTypes.Create(memory, BsTypes.Types.Int, 5),
                BsTypes.Create(memory, BsTypes.Types.Int, 6)
            });
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void MultiplyConcatenation()
        {
            var input = """
                        y = [1, 2, 3]
                        x = y * 3
                        """;
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, BsTypes.Types.List, new List<Variable>()
            {
                BsTypes.Create(memory, BsTypes.Types.Int, 1),
                BsTypes.Create(memory, BsTypes.Types.Int, 2),
                BsTypes.Create(memory, BsTypes.Types.Int, 3),
                BsTypes.Create(memory, BsTypes.Types.Int, 1),
                BsTypes.Create(memory, BsTypes.Types.Int, 2),
                BsTypes.Create(memory, BsTypes.Types.Int, 3),
                BsTypes.Create(memory, BsTypes.Types.Int, 1),
                BsTypes.Create(memory, BsTypes.Types.Int, 2),
                BsTypes.Create(memory, BsTypes.Types.Int, 3),
            });
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}