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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.String, "5");
            
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SupportsListSlicing()
        {
            var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[1:4]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(BsTypes.Types.Int, 1),
                    BsTypes.Create(BsTypes.Types.Int, 2),
                    BsTypes.Create(BsTypes.Types.Int, 3),
                }
            );
            Assertions.AssertVariable(memory, "y", expected);
        }
        
        [Test]
        public void SupportsAssigningToListIndex()
        {
            var input = "x = [5, '5']\nx[1] = 0";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(BsTypes.Types.Int, 5),
                    BsTypes.Create(BsTypes.Types.Int, 0),
                }
            );
            
            Assertions.AssertVariable(memory, "x", expected);
        }
        
        [Test]
        public void SupportsAssigningToListSlice()
        {
            var input = "x = [0, 1, 2, 3, 4, 5]\nx[1:3] = [6, 7]";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.List,
                new List<Variable>
                {
                    BsTypes.Create(BsTypes.Types.Int, 0),
                    BsTypes.Create(BsTypes.Types.Int, 6),
                    BsTypes.Create(BsTypes.Types.Int, 7),
                    BsTypes.Create(BsTypes.Types.Int, 3),
                    BsTypes.Create(BsTypes.Types.Int, 4),
                    BsTypes.Create(BsTypes.Types.Int, 5),
                }
            );
            Assertions.AssertVariable(memory, "x", expected);
        }

        [TestFixture]
        public class StartAndStopIndices
        {
            [Test]
            public void NullStartDefaultsToZero()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[:4]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NullStopDefaultsToLength()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[2:]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NullStartAndStop()
            {
                var input = "x = [0, 1, 2]\ny = x[:]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void StopBeforeStart()
            {
                var input = "x = [0, 1, 2]\ny = x[4:2]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>()
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NegativeStart()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[-4:4]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NegativeStop()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[1:-1]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NegativeStartAndStop()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[-5:-2]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NegativeStartAndStopWithStopBeforeStart()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[-2:-5]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>()
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void StartValuesBelowRangeAreClampedToNegativeCount()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[-8:-1]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void StartValuesAboveRangeAreClampedToCount()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[8:6]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>()
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void StopValuesBelowRangeAreClampedToNegativeCount()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[0:-9]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>()
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void StopValuesAboveRangeAreClampedToCount()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[0:9]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>()
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
        }

        [TestFixture]
        public class StepValues
        {
            [Test]
            public void PositiveStep()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[1:5:2]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void NegativeStep()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[5:1:-2]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void LargePositiveStep()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[1:5:8]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 1),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
            
            [Test]
            public void LargeNegativeStep()
            {
                var input = "x = [0, 1, 2, 3, 4, 5]\ny = x[5:1:-8]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 5),
                    }
                );
                Assertions.AssertVariable(memory, "y", expected);
            }
        }

        [TestFixture]
        public class AssignmentToSlices
        {
            [Test]
            public void AssigningLargerList()
            {
                var input = "x = [0, 1, 2, 3]\nx[1:3] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void AssigningSmallerList()
            {
                var input = "x = [0, 1, 2, 3]\nx[1:3] = [4]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void AssigningEmptyList()
            {
                var input = "x = [0, 1, 2, 3]\nx[1:3] = []";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void InsertWithEmptyRange()
            {
                var input = "x = [0, 1, 2, 3]\nx[2:2] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void NegativeStartValue()
            {
                var input = "x = [0, 1, 2, 3]\nx[-3:3] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void NegativeEndValue()
            {
                var input = "x = [0, 1, 2, 3]\nx[1:-1] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void NegativeStartAndEndValue()
            {
                var input = "x = [0, 1, 2, 3]\nx[-3:-1] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
        }

        [TestFixture]
        public class AssignmentToSlicesWithStepValues
        {
            [Test]
            public void PositiveStepValue()
            {
                var input = "x = [0, 1, 2, 3]\nx[::2] = [4, 5]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
            
            [Test]
            public void NegativeStepValue()
            {
                var input = "x = [0, 1, 2, 3]\nx[::-2] = [4, 5]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
        }

        [TestFixture]
        public class EdgeCases
        {
            [Test]
            public void Append()
            {
                var input = "x = [0, 1, 2, 3]\nx[4:] = [4]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 2),
                        BsTypes.Create(BsTypes.Types.Int, 3),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }

            [Test]
            public void ReplaceAndAppend()
            {
                var input = "x = [0, 1, 2, 3]\nx[2:] = [4, 5, 6, 7]";
                var memory = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.List,
                    new List<Variable>
                    {
                        BsTypes.Create(BsTypes.Types.Int, 0),
                        BsTypes.Create(BsTypes.Types.Int, 1),
                        BsTypes.Create(BsTypes.Types.Int, 4),
                        BsTypes.Create(BsTypes.Types.Int, 5),
                        BsTypes.Create(BsTypes.Types.Int, 6),
                        BsTypes.Create(BsTypes.Types.Int, 7),
                    }
                );
                Assertions.AssertVariable(memory, "x", expected);
            }
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            
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
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            
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
            var expected = BsTypes.Create(BsTypes.Types.List, new List<Variable>()
            {
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2),
                BsTypes.Create(BsTypes.Types.Int, 3),
                BsTypes.Create(BsTypes.Types.Int, 4),
                BsTypes.Create(BsTypes.Types.Int, 5),
                BsTypes.Create(BsTypes.Types.Int, 6)
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
            var expected = BsTypes.Create(BsTypes.Types.List, new List<Variable>()
            {
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2),
                BsTypes.Create(BsTypes.Types.Int, 3),
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2),
                BsTypes.Create(BsTypes.Types.Int, 3),
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Int, 2),
                BsTypes.Create(BsTypes.Types.Int, 3),
            });
            Assertions.AssertVariable(memory, "x", expected);
        }
    }
}