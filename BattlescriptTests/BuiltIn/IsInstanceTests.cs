using Battlescript;

namespace BattlescriptTests.BuiltIn;

[TestFixture]
public class IsInstanceTests
{
    public class BuiltInTypes
    {
        public class Int
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(5, int)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(5, str)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        public class Float
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(3.14, float)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(3.14, int)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        public class Str
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance("hello", str)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance("hello", int)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        public class Bool
        {
            [Test]
            public void TrueIsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(True, bool)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void FalseIsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(False, bool)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance(True, str)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        public class List
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance([1, 2, 3], list)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance([1, 2, 3], dict)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        public class Dict
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance({"a": 1}, dict)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                        x = isinstance({"a": 1}, list)
                                        """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }
    }

    public class CustomClasses
    {
        [Test]
        public void ObjectIsInstanceOfItsClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class Foo:
                                        pass
                                    obj = Foo()
                                    x = isinstance(obj, Foo)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void ObjectIsNotInstanceOfDifferentClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class Foo:
                                        pass
                                    class Bar:
                                        pass
                                    obj = Foo()
                                    x = isinstance(obj, Bar)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }

        [Test]
        public void ObjectIsInstanceOfParentClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class Parent:
                                        pass
                                    class Child(Parent):
                                        pass
                                    obj = Child()
                                    x = isinstance(obj, Parent)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void ObjectIsInstanceOfGrandparentClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class Grandparent:
                                        pass
                                    class Parent(Grandparent):
                                        pass
                                    class Child(Parent):
                                        pass
                                    obj = Child()
                                    x = isinstance(obj, Grandparent)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void ParentIsNotInstanceOfChildClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class Parent:
                                        pass
                                    class Child(Parent):
                                        pass
                                    obj = Parent()
                                    x = isinstance(obj, Child)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }
    }

    public class EdgeCases
    {
        [Test]
        public void VariableHoldingIntIsInstanceOfInt()
        {
            var (callStack, closure) = Runner.Run("""
                                    y = 42
                                    x = isinstance(y, int)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void EmptyListIsInstanceOfList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = isinstance([], list)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void EmptyDictIsInstanceOfDict()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = isinstance({}, dict)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void EmptyStringIsInstanceOfStr()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = isinstance("", str)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void ZeroIsInstanceOfInt()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = isinstance(0, int)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }

        [Test]
        public void ZeroPointZeroIsInstanceOfFloat()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = isinstance(0.0, float)
                                    """);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
        }
    }

    [TestFixture]
    public class BindingTypes
    {
        [TestFixture]
        public class Numeric
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_numeric__(5), __btl_numeric__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }
            
            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_numeric__(5), int)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
            
            [Test]
            public void OtherObjectsAreNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(4, __btl_numeric__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        [TestFixture]
        public class String
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_string__("hello"), __btl_string__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_string__("hello"), str)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }

            [Test]
            public void OtherObjectsAreNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance("hello", __btl_string__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        [TestFixture]
        public class Sequence
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_sequence__(), __btl_sequence__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_sequence__(), list)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }

            [Test]
            public void OtherObjectsAreNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance([1, 2, 3], __btl_sequence__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }

        [TestFixture]
        public class Mapping
        {
            [Test]
            public void IsInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_mapping__(), __btl_mapping__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            }

            [Test]
            public void IsNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance(__btl_mapping__(), dict)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }

            [Test]
            public void OtherObjectsAreNotInstanceOf()
            {
                var (callStack, closure) = Runner.Run("""
                                                      x = isinstance({"a": 1}, __btl_mapping__)
                                                      """);
                Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
            }
        }
    }
}
