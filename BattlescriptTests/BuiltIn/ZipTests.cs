using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class ZipTests
{
    [Test]
    public void ZipTwoListsSameLength()
    {
        var input = "x = zip([1, 2, 3], ['a', 'b', 'c'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.String, "c")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipTwoListsDifferentLengths()
    {
        var input = "x = zip([1, 2, 3], ['a', 'b'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipThreeLists()
    {
        var input = "x = zip([1, 2], ['a', 'b'], [True, False])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a"),
                BtlTypes.Create(BtlTypes.Types.Bool, true)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b"),
                BtlTypes.Create(BtlTypes.Types.Bool, false)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipThreeListsDifferentLengths()
    {
        var input = "x = zip([1, 2, 3, 4], ['a', 'b'], [10, 20, 30])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a"),
                BtlTypes.Create(BtlTypes.Types.Int, 10)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b"),
                BtlTypes.Create(BtlTypes.Types.Int, 20)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipEmptyLists()
    {
        var input = "x = zip([], [])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipNoArguments()
    {
        var input = "x = zip()";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipSingleList()
    {
        var input = "x = zip([1, 2, 3])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipOneEmptyList()
    {
        var input = "x = zip([1, 2, 3], [])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>());
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipStrings()
    {
        var input = "x = zip('abc', 'xyz')";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.String, "a"),
                BtlTypes.Create(BtlTypes.Types.String, "x")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.String, "b"),
                BtlTypes.Create(BtlTypes.Types.String, "y")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.String, "c"),
                BtlTypes.Create(BtlTypes.Types.String, "z")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipTuples()
    {
        var input = "x = zip((1, 2, 3), (4, 5, 6))";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 4)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 5)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 6)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipMixedIterables()
    {
        var input = "x = zip([1, 2], 'ab', (10, 20))";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a"),
                BtlTypes.Create(BtlTypes.Types.Int, 10)
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b"),
                BtlTypes.Create(BtlTypes.Types.Int, 20)
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipInForLoop()
    {
        var input = """
                    result = []
                    for a, b in zip([1, 2, 3], ['x', 'y', 'z']):
                        result.append(a)
                        result.append(b)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.String, "x"),
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.String, "y"),
            BtlTypes.Create(BtlTypes.Types.Int, 3),
            BtlTypes.Create(BtlTypes.Types.String, "z")
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipInForLoopThreeIterables()
    {
        var input = """
                    result = []
                    for a, b, c in zip([1, 2], ['x', 'y'], [10, 20]):
                        result.append(a + c)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 11),
            BtlTypes.Create(BtlTypes.Types.Int, 22)
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipResultIsIndexable()
    {
        var input = """
                    x = zip([1, 2, 3], ['a', 'b', 'c'])
                    first = x[0]
                    second = x[1]
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expectedFirst = BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 1),
            BtlTypes.Create(BtlTypes.Types.String, "a")
        });
        var expectedSecond = BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Int, 2),
            BtlTypes.Create(BtlTypes.Types.String, "b")
        });
        Assert.That(closure.GetVariable(callStack, "first"), Is.EqualTo(expectedFirst));
        Assert.That(closure.GetVariable(callStack, "second"), Is.EqualTo(expectedSecond));
    }

    [Test]
    public void ZipTuplesAreIndexable()
    {
        var input = """
                    x = zip([1, 2], ['a', 'b'])
                    first_tuple = x[0]
                    num = first_tuple[0]
                    letter = first_tuple[1]
                    """;
        var (callStack, closure) = Runner.Run(input);
        Assert.That(closure.GetVariable(callStack, "num"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        Assert.That(closure.GetVariable(callStack, "letter"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "a")));
    }

    [Test]
    public void ZipCreateDictionary()
    {
        var input = """
                    keys = ['name', 'age', 'city']
                    values = ['Alice', 25, 'NYC']
                    pairs = zip(keys, values)
                    result = []
                    for k, v in pairs:
                        result.append(k)
                        result.append(v)
                    """;
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.String, "name"),
            BtlTypes.Create(BtlTypes.Types.String, "Alice"),
            BtlTypes.Create(BtlTypes.Types.String, "age"),
            BtlTypes.Create(BtlTypes.Types.Int, 25),
            BtlTypes.Create(BtlTypes.Types.String, "city"),
            BtlTypes.Create(BtlTypes.Types.String, "NYC")
        });
        Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipSingleElementLists()
    {
        var input = "x = zip([1], ['a'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }

    [Test]
    public void ZipLongLists()
    {
        var input = "x = zip([1, 2, 3, 4, 5], ['a', 'b', 'c', 'd', 'e'])";
        var (callStack, closure) = Runner.Run(input);
        var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>() {
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.String, "a")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.String, "b")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.String, "c")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 4),
                BtlTypes.Create(BtlTypes.Types.String, "d")
            }),
            BtlTypes.Create(BtlTypes.Types.Tuple, new List<Variable>() {
                BtlTypes.Create(BtlTypes.Types.Int, 5),
                BtlTypes.Create(BtlTypes.Types.String, "e")
            })
        });
        Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
    }
}
