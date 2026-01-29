using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class DeleteInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesVariable()
        {
            var input = "del x";
            var expected = new DeleteInstruction(new VariableInstruction("x"));
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.EqualTo(expected));
        }

        [Test]
        public void ParsesIndexAccess()
        {
            var input = "del x[0]";
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.InstanceOf<DeleteInstruction>());
        }

        [Test]
        public void ParsesMemberAccess()
        {
            var input = "del x.y";
            var result = Runner.Parse(input);
            Assert.That(result[0], Is.InstanceOf<DeleteInstruction>());
        }
    }

    [TestFixture]
    public class DeleteVariable
    {
        [Test]
        public void DeletesVariableFromScope()
        {
            var input = """
                        x = 5
                        del x
                        y = x
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("NameError"));
        }

        [Test]
        public void ThrowsNameErrorIfVariableDoesNotExist()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("del x"));
            Assert.That(ex.Type, Is.EqualTo("NameError"));
        }
    }

    [TestFixture]
    public class DeleteListItem
    {
        [Test]
        public void DeletesItemFromList()
        {
            var input = """
                        x = [1, 2, 3]
                        del x[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            });
            Assert.That(x, Is.EqualTo(expected));
        }

        [Test]
        public void DeletesFirstItemFromList()
        {
            var input = """
                        x = [1, 2, 3]
                        del x[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            });
            Assert.That(x, Is.EqualTo(expected));
        }

        [Test]
        public void DeletesLastItemFromList()
        {
            var input = """
                        x = [1, 2, 3]
                        del x[2]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2)
            });
            Assert.That(x, Is.EqualTo(expected));
        }

        [Test]
        public void DeletesNestedListItem()
        {
            var input = """
                        x = [[1, 2], [3, 4]]
                        del x[0][1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
                {
                    BtlTypes.Create(BtlTypes.Types.Int, 1)
                }),
                BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
                {
                    BtlTypes.Create(BtlTypes.Types.Int, 3),
                    BtlTypes.Create(BtlTypes.Types.Int, 4)
                })
            });
            Assert.That(x, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class DeleteDictItem
    {
        [Test]
        public void DeletesItemFromDictByStringKey()
        {
            var input = """
                        x = {'a': 1, 'b': 2}
                        del x['a']
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(
                stringValues: new Dictionary<string, Variable>
                {
                    { "b", BtlTypes.Create(BtlTypes.Types.Int, 2) }
                }
            ));
            Assert.That(x, Is.EqualTo(expected));
        }

        [Test]
        public void DeletesItemFromDictByIntKey()
        {
            var input = """
                        x = {1: 'a', 2: 'b'}
                        del x[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(
                intValues: new Dictionary<int, Variable>
                {
                    { 2, BtlTypes.Create(BtlTypes.Types.String, "b") }
                }
            ));
            Assert.That(x, Is.EqualTo(expected));
        }

        [Test]
        public void ThrowsErrorIfKeyDoesNotExist()
        {
            var input = """
                        x = {'a': 1}
                        del x['b']
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("NameError"));
        }

        [Test]
        public void DeletesFromNestedDict()
        {
            var input = """
                        x = {'a': {'b': 1, 'c': 2}}
                        del x['a']['b']
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x");
            var expected = BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(
                stringValues: new Dictionary<string, Variable>
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable(
                        stringValues: new Dictionary<string, Variable>
                        {
                            { "c", BtlTypes.Create(BtlTypes.Types.Int, 2) }
                        }
                    ))}
                }
            ));
            Assert.That(x, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class DeleteObjectAttribute
    {
        [Test]
        public void DeletesAttributeFromObject()
        {
            var input = """
                        class Foo:
                            pass
                        x = Foo()
                        x.bar = 5
                        del x.bar
                        y = hasattr(x, 'bar')
                        """;
            var (callStack, closure) = Runner.Run(input);
            var y = closure.GetVariable(callStack, "y");
            Assert.That(y, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }

        [Test]
        public void ThrowsAttributeErrorIfAttributeDoesNotExist()
        {
            var input = """
                        class Foo:
                            pass
                        x = Foo()
                        del x.bar
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("AttributeError"));
        }

        [Test]
        public void DeletesNestedAttribute()
        {
            var input = """
                        class Foo:
                            pass
                        x = Foo()
                        x.inner = Foo()
                        x.inner.value = 10
                        del x.inner.value
                        y = hasattr(x.inner, 'value')
                        """;
            var (callStack, closure) = Runner.Run(input);
            var y = closure.GetVariable(callStack, "y");
            Assert.That(y, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }
    }

    [TestFixture]
    public class MixedAccess
    {
        [Test]
        public void DeletesListItemThroughAttribute()
        {
            var input = """
                        class Foo:
                            pass
                        x = Foo()
                        x.items = [1, 2, 3]
                        del x.items[1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var x = closure.GetVariable(callStack, "x") as ObjectVariable;
            var items = x.Values["items"];
            var expected = BtlTypes.Create(BtlTypes.Types.List, new List<Variable>
            {
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 3)
            });
            Assert.That(items, Is.EqualTo(expected));
        }

        [Test]
        public void DeletesAttributeThroughListItem()
        {
            var input = """
                        class Foo:
                            pass
                        a = Foo()
                        a.value = 10
                        x = [a]
                        del x[0].value
                        y = hasattr(x[0], 'value')
                        """;
            var (callStack, closure) = Runner.Run(input);
            var y = closure.GetVariable(callStack, "y");
            Assert.That(y, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }
    }
}
