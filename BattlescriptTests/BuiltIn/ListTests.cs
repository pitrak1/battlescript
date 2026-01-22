using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class ListTests
{
    [Test]
    public void ValueIsSequenceVariable()
    {
        var (callStack, closure) = Runner.Run("""
                                x = []
                                y = x.__btl_value
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(new SequenceVariable()));
    }

    [Test]
    public void ConstructorSetsValue()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x.__btl_value
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(new SequenceVariable([
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
        ])));
    }

    [Test]
    public void CanGetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                y = x[1]
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
    }

    [Test]
    public void CanSetItem()
    {
        var (callStack, closure) = Runner.Run("""
                                x = [1, 2, 3]
                                x[1] = 4
                                y = x[1]
                                """);
        Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 4)));
    }

    [TestFixture]
    public class Append
    {
        [Test]
        public void AppendsIntToEmptyList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    x.append(1)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsIntToNonEmptyList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    x.append(4)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsStringToList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.append("hello")
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "hello"),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsListToList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.append([3, 4])
                                    y = x[2]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void ReturnsNone()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    y = x.append(4)
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.None));
        }

        [Test]
        public void ModifiesListInPlace()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    y = x
                                    x.append(4)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void MultipleAppends()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    x.append(1)
                                    x.append(2)
                                    x.append(3)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendInLoop()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    for i in range(3):
                                        x.append(i * 2)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsBoolToList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1]
                                    x.append(True)
                                    x.append(False)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Bool, true),
                BtlTypes.Create(BtlTypes.Types.Bool, false),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsNoneToList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.append(None)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.None,
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void AppendsFloatToList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.append(3.14)
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Float, 3.14),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class Extend
    {
        [Test]
        public void ExtendsEmptyListWithList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    x.extend([1, 2, 3])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendsNonEmptyListWithList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.extend([3, 4, 5])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
                BtlTypes.Create(BtlTypes.Types.Int, 5),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendsWithEmptyList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    x.extend([])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void ReturnsNone()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    y = x.extend([4, 5])
                                    """);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(BtlTypes.None));
        }

        [Test]
        public void ModifiesListInPlace()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    y = x
                                    x.extend([3, 4])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendsWithMixedTypes()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1]
                                    x.extend(["hello", 3.14, True])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "hello"),
                BtlTypes.Create(BtlTypes.Types.Float, 3.14),
                BtlTypes.Create(BtlTypes.Types.Bool, true),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        // Will come back and uncomment this once a string variable is a proper sequence
        [Test]
        public void ExtendsWithString()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2]
                                    x.extend("abc")
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "a"),
                BtlTypes.Create(BtlTypes.Types.String, "b"),
                BtlTypes.Create(BtlTypes.Types.String, "c"),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendsWithRange()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [0]
                                    x.extend(range(1, 4))
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void MultipleExtends()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    x.extend([1, 2])
                                    x.extend([3, 4])
                                    x.extend([5])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
                BtlTypes.Create(BtlTypes.Types.Int, 5),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendsWithNestedList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1]
                                    x.extend([[2, 3], [4, 5]])
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
            });
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void ExtendInLoop()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = []
                                    for i in range(3):
                                        x.extend([i, i * 2])
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            });
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }
}
