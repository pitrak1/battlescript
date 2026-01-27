using Battlescript;

namespace BattlescriptTests.E2ETests.Memory;

public class MemoryTests
{
    [TestFixture]
    public class ValueSemantics
    {
        [Test]
        public void IntAssignmentCopiesByValue()
        {
            var input = """
                        a = 10
                        b = a
                        b = 20
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 20)));
        }

        [Test]
        public void FloatAssignmentCopiesByValue()
        {
            var input = """
                        a = 3.14
                        b = a
                        b = 2.71
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, 3.14)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Float, 2.71)));
        }

        [Test]
        public void BoolAssignmentCopiesByValue()
        {
            var input = """
                        a = True
                        b = a
                        b = False
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, true)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Bool, false)));
        }

        [Test]
        public void StringAssignmentCopiesByValue()
        {
            var input = """
                        a = "hello"
                        b = a
                        b = "world"
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "hello")));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "world")));
        }

        [Test]
        public void IntReassignmentInFunctionDoesNotAffectOuter()
        {
            var input = """
                        a = 5
                        def modify(x):
                            x = 99
                        modify(a)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
        }

        [Test]
        public void StringReassignmentInFunctionDoesNotAffectOuter()
        {
            var input = """
                        a = "original"
                        def modify(x):
                            x = "changed"
                        modify(a)
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "original")));
        }

        [Test]
        public void OperatorAssignmentsRespectValueVariables()
        {
            var input = """
                        x = []
                        a = 0
                        while a < 5:
                            x.append(a)
                            a += 1
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                BtlTypes.Create(BtlTypes.Types.Int, 0),
                BtlTypes.Create(BtlTypes.Types.Int, 1),
                BtlTypes.Create(BtlTypes.Types.Int, 2),
                BtlTypes.Create(BtlTypes.Types.Int, 3),
                BtlTypes.Create(BtlTypes.Types.Int, 4),
            ]));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ReferenceSemantics
    {
        [Test]
        public void ListMutationIsSharedThroughAlias()
        {
            var input = """
                        a = [1, 2, 3]
                        b = a
                        b[0] = 99
                        x = a[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void ListReassignmentBreaksAlias()
        {
            var input = """
                        a = [1, 2, 3]
                        b = a
                        b = [4, 5, 6]
                        x = a[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        }

        [Test]
        public void ListMutationInFunctionAffectsOuter()
        {
            var input = """
                        a = [1, 2, 3]
                        def modify(lst):
                            lst[0] = 99
                        modify(a)
                        x = a[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void ListReassignmentInFunctionDoesNotAffectOuter()
        {
            var input = """
                        a = [1, 2, 3]
                        def modify(lst):
                            lst = [4, 5, 6]
                        modify(a)
                        x = a[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        }

        [Test]
        public void DictMutationIsSharedThroughAlias()
        {
            var input = """
                        a = {"key": 1}
                        b = a
                        b["key"] = 99
                        x = a["key"]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void DictReassignmentBreaksAlias()
        {
            var input = """
                        a = {"key": 1}
                        b = a
                        b = {"key": 99}
                        x = a["key"]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        }

        [Test]
        public void DictMutationInFunctionAffectsOuter()
        {
            var input = """
                        a = {"key": 1}
                        def modify(d):
                            d["key"] = 99
                        modify(a)
                        x = a["key"]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void ObjectMutationIsSharedThroughAlias()
        {
            var input = """
                        class Foo():
                            def __init__(self):
                                self.val = 1
                        a = Foo()
                        b = a
                        b.val = 99
                        x = a.val
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void ObjectReassignmentBreaksAlias()
        {
            var input = """
                        class Foo():
                            def __init__(self):
                                self.val = 1
                        a = Foo()
                        b = a
                        b = Foo()
                        b.val = 99
                        x = a.val
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
        }

        [Test]
        public void ObjectMutationInFunctionAffectsOuter()
        {
            var input = """
                        class Foo():
                            def __init__(self):
                                self.val = 1
                        a = Foo()
                        def modify(obj):
                            obj.val = 99
                        modify(a)
                        x = a.val
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void NestedListElementsShareReferences()
        {
            var input = """
                        inner = [1, 2]
                        a = [inner, [3, 4]]
                        b = a[0]
                        b[0] = 99
                        x = inner[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }

        [Test]
        public void ObjectContainingListSharesListReference()
        {
            var input = """
                        class Foo():
                            def __init__(self):
                                self.items = [1, 2, 3]
                        a = Foo()
                        b = a.items
                        b[0] = 99
                        x = a.items[0]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 99)));
        }
    }
}