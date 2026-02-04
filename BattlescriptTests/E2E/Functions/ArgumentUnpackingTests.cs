using Battlescript;

namespace BattlescriptTests.E2ETests.Functions;

[TestFixture]
public class ArgumentUnpackingTests
{
    [TestFixture]
    public class FunctionCallsAsUnpackedArguments
    {
        [Test]
        public void UnpackFunctionReturnValueWithSingleAsterisk()
        {
            var input = """
                        def get_tuple():
                            return (1, 2, 3)

                        def add(a, b, c):
                            return a + b + c

                        result = add(*get_tuple())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackFunctionReturnValueWithDoubleAsterisk()
        {
            var input = """
                        def get_dict():
                            return {'a': 1, 'b': 2, 'c': 3}

                        def add(a, b, c):
                            return a + b + c

                        result = add(**get_dict())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackNestedFunctionCalls()
        {
            var input = """
                        def get_inner():
                            return [1, 2]

                        def get_outer():
                            return get_inner()

                        def multiply(x, y):
                            return x * y

                        result = multiply(*get_outer())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 2);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackMethodReturnValue()
        {
            var input = """
                        class Container:
                            def get_values(self):
                                return (10, 20)

                        def add(x, y):
                            return x + y

                        obj = Container()
                        result = add(*obj.get_values())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 30);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void MixPositionalAndUnpackedFunctionCall()
        {
            var input = """
                        def get_pair():
                            return (2, 3)

                        def add_three(a, b, c):
                            return a + b + c

                        result = add_three(1, *get_pair())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class IndexAccessAsUnpackedArguments
    {
        [Test]
        public void UnpackListElementWithSingleAsterisk()
        {
            var input = """
                        lists = [[1, 2, 3], [4, 5, 6]]

                        def add(a, b, c):
                            return a + b + c

                        result = add(*lists[0])
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackDictElementWithDoubleAsterisk()
        {
            var input = """
                        dicts = [{'a': 1, 'b': 2}, {'c': 3, 'd': 4}]

                        def add(a, b):
                            return a + b

                        result = add(**dicts[0])
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 3);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackNestedIndexAccess()
        {
            var input = """
                        matrix = [[[1, 2]], [[3, 4]]]

                        def multiply(x, y):
                            return x * y

                        result = multiply(*matrix[0][0])
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 2);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackSliceResult()
        {
            var input = """
                        data = [1, 2, 3, 4, 5]

                        def add(a, b, c):
                            return a + b + c

                        result = add(*data[1:4])
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        // NOTE: Dictionary literals with nested dicts are not yet fully supported
        // [Test]
        // public void UnpackDictionaryByKey()
        // {
        //     var input = """
        //                 nested = {
        //                     'params': {'x': 10, 'y': 20}
        //                 }
        //
        //                 def add(x, y):
        //                     return x + y
        //
        //                 result = add(**nested['params'])
        //                 """;
        //     var (callStack, closure) = Runner.Run(input);
        //     var expected = BtlTypes.Create(BtlTypes.Types.Int, 30);
        //
        //     Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        // }
    }

    [TestFixture]
    public class MemberAccessAsUnpackedArguments
    {
        [Test]
        public void UnpackObjectAttributeWithSingleAsterisk()
        {
            var input = """
                        class Config:
                            def __init__(self):
                                self.values = [5, 10, 15]

                        def add(a, b, c):
                            return a + b + c

                        cfg = Config()
                        result = add(*cfg.values)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 30);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackObjectAttributeWithDoubleAsterisk()
        {
            var input = """
                        class Settings:
                            def __init__(self):
                                self.params = {'width': 100, 'height': 200}

                        def area(width, height):
                            return width * height

                        settings = Settings()
                        result = area(**settings.params)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 20000);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackNestedAttributeAccess()
        {
            var input = """
                        class Inner:
                            def __init__(self):
                                self.data = [7, 8]

                        class Outer:
                            def __init__(self):
                                self.inner = Inner()

                        def multiply(x, y):
                            return x * y

                        obj = Outer()
                        result = multiply(*obj.inner.data)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 56);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        [Test]
        public void UnpackAttributeFromMethodCall()
        {
            var input = """
                        class Factory:
                            def create(self):
                                class Product:
                                    def __init__(self):
                                        self.coords = (3, 4)
                                return Product()

                        def distance(x, y):
                            return (x * x + y * y) ** 0.5

                        factory = Factory()
                        result = distance(*factory.create().coords)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Float, 5.0);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }
    }

    // NOTE: Return statement unpacking (e.g., "return *values,") is not yet implemented
    // [TestFixture]
    // public class FunctionReturnsWithUnpacking
    // {
    //     [Test]
    //     public void FunctionReturnsUnpackedList()
    //     {
    //         var input = """
    //                     def get_data():
    //                         values = [10, 20, 30]
    //                         return *values,
    //
    //                     result = get_data()
    //                     """;
    //         var (callStack, closure) = Runner.Run(input);
    //         var expected = BtlTypes.Create(BtlTypes.Types.Tuple,
    //             new List<Variable>
    //             {
    //                 BtlTypes.Create(BtlTypes.Types.Int, 10),
    //                 BtlTypes.Create(BtlTypes.Types.Int, 20),
    //                 BtlTypes.Create(BtlTypes.Types.Int, 30)
    //             }
    //         );
    //
    //         Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    //     }
    // }

    // NOTE: List literal unpacking (e.g., "[*values]") and dict literal merging (e.g., "{**dict1, **dict2}")
    // are not yet implemented
    // [TestFixture]
    // public class AssignmentWithUnpacking
    // {
    //     [Test]
    //     public void AssignUnpackedFunctionCall()
    //     {
    //         var input = """
    //                     def get_values():
    //                         return [1, 2, 3]
    //
    //                     result = [*get_values()]
    //                     """;
    //         var (callStack, closure) = Runner.Run(input);
    //         var expected = BtlTypes.Create(BtlTypes.Types.List,
    //             new List<Variable>
    //             {
    //                 BtlTypes.Create(BtlTypes.Types.Int, 1),
    //                 BtlTypes.Create(BtlTypes.Types.Int, 2),
    //                 BtlTypes.Create(BtlTypes.Types.Int, 3)
    //             }
    //         );
    //
    //         Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
    //     }
    // }

    [TestFixture]
    public class ComplexUnpackingScenarios
    {
        [Test]
        public void ChainedUnpackingOperations()
        {
            var input = """
                        def get_list():
                            return [1, 2]

                        class Store:
                            def __init__(self):
                                self.values = [3, 4]

                        data = [[5, 6]]
                        store = Store()

                        def sum_all(a, b, c, d, e, f):
                            return a + b + c + d + e + f

                        result = sum_all(*get_list(), *store.values, *data[0])
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 21);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        // NOTE: List literal unpacking in list comprehensions is not yet supported
        // [Test]
        // public void UnpackInListComprehension()
        // {
        //     var input = """
        //                 def get_range():
        //                     return [1, 2, 3]
        //
        //                 result = [x * 2 for x in [*get_range()]]
        //                 """;
        //     var (callStack, closure) = Runner.Run(input);
        //     var expected = BtlTypes.Create(BtlTypes.Types.List,
        //         new List<Variable>
        //         {
        //             BtlTypes.Create(BtlTypes.Types.Int, 2),
        //             BtlTypes.Create(BtlTypes.Types.Int, 4),
        //             BtlTypes.Create(BtlTypes.Types.Int, 6)
        //         }
        //     );
        //
        //     Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        // }

        [Test]
        public void UnpackMethodChainResult()
        {
            var input = """
                        class Builder:
                            def __init__(self):
                                self.data = [1, 2]

                            def get_data(self):
                                return self.data

                        class Factory:
                            def create(self):
                                return Builder()

                        def add(a, b):
                            return a + b

                        factory = Factory()
                        result = add(*factory.create().get_data())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 3);

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }

        // NOTE: Dictionary literals with nested dicts are not yet fully supported
        // [Test]
        // public void UnpackNestedDictFromMethod()
        // {
        //     var input = """
        //                 class DataSource:
        //                     def get_config(self):
        //                         return {
        //                             'nested': {'x': 5, 'y': 10}
        //                         }
        //
        //                 def multiply(x, y):
        //                     return x * y
        //
        //                 source = DataSource()
        //                 result = multiply(**source.get_config()['nested'])
        //                 """;
        //     var (callStack, closure) = Runner.Run(input);
        //     var expected = BtlTypes.Create(BtlTypes.Types.Int, 50);
        //
        //     Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        // }

        [Test]
        public void UnpackInNestedFunctionCalls()
        {
            var input = """
                        def inner(*args):
                            return args

                        def outer():
                            return [1, 2, 3]

                        result = inner(*outer())
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Tuple,
                new List<Variable>
                {
                    BtlTypes.Create(BtlTypes.Types.Int, 1),
                    BtlTypes.Create(BtlTypes.Types.Int, 2),
                    BtlTypes.Create(BtlTypes.Types.Int, 3)
                }
            );

            Assert.That(closure.GetVariable(callStack, "result"), Is.EqualTo(expected));
        }
    }
}
