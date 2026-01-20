using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class VariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void GetsItemFromList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 2);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void GetsItemWithNumericKeyFromDictionary()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = {1: "asdf", 2: "qwer"}
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "asdf");
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void GetsItemWithStringKeyFromDictionary()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = {1: "asdf", "zxcv": "qwer"}
                                    y = x["zxcv"]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "qwer");
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void GetsItemsFromObjectIfOverrideExists()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                        def __getitem__(self, index):
                                            return 6
                                            
                                    x = asdf()
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class SetItem
    {
        [Test]
        public void SetsItemInList()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = [1, 2, 3]
                                    x[1] = 4
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 4);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void SetsItemWithNumericKeyInDictionary()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = {1: "asdf", 2: "qwer"}
                                    x[1] = "zxcv"
                                    y = x[1]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.String, "zxcv");
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void SetsItemWithStringKeyInDictionary()
        {
            var (callStack, closure) = Runner.Run("""
                                    x = {1: "asdf", "zxcv": "qwer"}
                                    x["zxcv"] = 2
                                    y = x["zxcv"]
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 2);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void SetsItemFromObjectIfOverrideExists()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                        def __getitem__(self, index):
                                            return self.i
                                            
                                        def __setitem__(self, key, value):
                                            self.i = value
                                            
                                    x = asdf()
                                    x[1] = 9
                                    y = x.i
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class GetMember
    {
        [Test]
        public void GetsMemberFromObject()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    x = asdf()
                                    y = x.i
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }

        [Test]
        public void GetsMemberFromClass()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    y = asdf.i
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class SetMember
    {
        [Test]
        public void SetsMemberInObject()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    x = asdf()
                                    x.i = 9
                                    y = x.i
                                    """);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assert.That(closure.GetVariable(callStack, "y"), Is.EqualTo(expected));
        }
    }
}