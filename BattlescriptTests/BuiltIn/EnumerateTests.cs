using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class EnumerateTests
{
    [Test]
    public void EnumerateListWithDefaultStart()
    {
        var input = "x = enumerate(['a', 'b', 'c'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "b")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "c")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateListWithCustomStart()
    {
        var input = "x = enumerate(['apple', 'banana', 'cherry'], 1)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "apple")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "banana")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.String, "cherry")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateEmptyList()
    {
        var input = "x = enumerate([])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateSingleElement()
    {
        var input = "x = enumerate(['only'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.String, "only")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateString()
    {
        var input = "x = enumerate('hello')";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.String, "h")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "e")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "l")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.String, "l")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 4),
                BtlTypes.Create(BtlTypes.Types.String, "o")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateWithNegativeStart()
    {
        var input = "x = enumerate(['a', 'b'], -2)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, -2),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, -1),
                BtlTypes.Create(BtlTypes.Types.String, "b")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateWithLargeStart()
    {
        var input = "x = enumerate(['x', 'y'], 100)";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 100),
                BtlTypes.Create(BtlTypes.Types.String, "x")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 101),
                BtlTypes.Create(BtlTypes.Types.String, "y")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateInForLoop()
    {
        var input = """
                    result = []
                    for i, value in enumerate(['a', 'b', 'c']):
                        result.append(i)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 0),
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2)
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateInForLoopWithCustomStart()
    {
        var input = """
                    result = []
                    for i, value in enumerate(['apple', 'banana'], 1):
                        result.append(i)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.Int, 2)
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateInForLoopAccessingValues()
    {
        var input = """
                    result = []
                    for i, value in enumerate(['a', 'b', 'c']):
                        result.append(value)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.String, "a"),
            BtlTypes.Create(BtlTypes.Types.String, "b"),
            BtlTypes.Create(BtlTypes.Types.String, "c")
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateInForLoopBothIndexAndValue()
    {
        var input = """
                    result = []
                    for i, value in enumerate(['x', 'y'], 10):
                        result.append(i + len(value))
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 11),
            BtlTypes.Create(BtlTypes.Types.Int, 12)
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateListOfIntegers()
    {
        var input = "x = enumerate([10, 20, 30])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 10)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 20)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 30)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void EnumerateResultIsAList()
    {
        var input = """
                    x = enumerate(['a', 'b', 'c'])
                    first_tuple = x[0]
                    index = first_tuple[0]
                    value = first_tuple[1]
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "index"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 0)));
        Assert.That(closure.GetVariable(callStack, "value"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "a")));
    }

    [Test]
    public void EnumerateTuple()
    {
        var input = "x = enumerate((1, 2, 3))";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 1)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
}
