using Battlescript;

namespace BattlescriptTests.Interpreter;

[TestFixture]
public class IteratorTests
{
    [TestFixture]
    public class ListIterator
    {
        [Test]
        public void ForLoopIteratesOverList()
        {
            var input = """
                        items = [1, 2, 3]
                        result = 0
                        for x in items:
                            result = result + x
                        """;
            var (callStack, closure) = Runner.Run(input);
            var result = closure.GetVariable(callStack, "result");
            Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 6)));
        }

        [Test]
        public void ManualIteratorProtocol()
        {
            var input = """
                        items = [10, 20, 30]
                        it = items.__iter__()
                        a = it.__next__()
                        b = it.__next__()
                        c = it.__next__()
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 20)));
            Assert.That(closure.GetVariable(callStack, "c"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 30)));
        }

        [Test]
        public void StopIterationRaisedAtEnd()
        {
            var input = """
                        items = [1]
                        it = items.__iter__()
                        it.__next__()
                        it.__next__()
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("StopIteration"));
        }

        [Test]
        public void IteratorReturnsItself()
        {
            // Calling __iter__() on an iterator should return itself
            // We verify this by checking that both references share state
            var input = """
                        items = [1, 2, 3]
                        it = items.__iter__()
                        it2 = it.__iter__()
                        a = it.__next__()
                        b = it2.__next__()
                        """;
            var (callStack, closure) = Runner.Run(input);
            // If it and it2 are the same object, calling __next__() on it advances it2
            // So a=1 (first element) and b=2 (second element, because state is shared)
            Assert.That(closure.GetVariable(callStack, "a"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 1)));
            Assert.That(closure.GetVariable(callStack, "b"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
        }

    }

    [TestFixture]
    public class DictIterator
    {
        [Test]
        public void ForLoopIteratesOverKeys()
        {
            var input = """
                        d = {'a': 1, 'b': 2}
                        keys = []
                        for k in d:
                            keys.append(k)
                        count = len(keys)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var count = closure.GetVariable(callStack, "count");
            Assert.That(count, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
        }

        [Test]
        public void CanAccessValuesWhileIterating()
        {
            var input = """
                        d = {'x': 10, 'y': 20}
                        total = 0
                        for k in d:
                            total = total + d[k]
                        """;
            var (callStack, closure) = Runner.Run(input);
            var total = closure.GetVariable(callStack, "total");
            Assert.That(total, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 30)));
        }

        [Test]
        public void KeysMethodReturnsKeys()
        {
            var input = """
                        d = {'a': 1, 'b': 2}
                        k = d.keys()
                        count = len(k)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var count = closure.GetVariable(callStack, "count");
            Assert.That(count, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 2)));
        }
    }

    [TestFixture]
    public class StringIterator
    {
        [Test]
        public void ForLoopIteratesOverCharacters()
        {
            var input = """
                        s = "abc"
                        chars = []
                        for c in s:
                            chars.append(c)
                        count = len(chars)
                        """;
            var (callStack, closure) = Runner.Run(input);
            var count = closure.GetVariable(callStack, "count");
            Assert.That(count, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 3)));
        }

        [Test]
        public void StringIndexing()
        {
            var input = """
                        s = "hello"
                        first = s[0]
                        last = s[-1]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "first"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "h")));
            Assert.That(closure.GetVariable(callStack, "last"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "o")));
        }

        [Test]
        public void StringSlicing()
        {
            var input = """
                        s = "hello"
                        sub = s[1:4]
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assert.That(closure.GetVariable(callStack, "sub"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.String, "ell")));
        }

        [Test]
        public void StringIndexOutOfRangeThrows()
        {
            var input = """
                        s = "hi"
                        c = s[10]
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Type, Is.EqualTo("IndexError"));
        }
    }

    [TestFixture]
    public class CustomIterator
    {
        [Test]
        public void CustomClassWithIterator()
        {
            var input = """
                        class Counter:
                            def __init__(self, limit):
                                self.limit = limit
                                self.current = 0

                            def __iter__(self):
                                return self

                            def __next__(self):
                                if self.current >= self.limit:
                                    raise StopIteration()
                                val = self.current
                                self.current = self.current + 1
                                return val

                        total = 0
                        for i in Counter(5):
                            total = total + i
                        """;
            var (callStack, closure) = Runner.Run(input);
            var total = closure.GetVariable(callStack, "total");
            // 0 + 1 + 2 + 3 + 4 = 10
            Assert.That(total, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
        }
    }
}
