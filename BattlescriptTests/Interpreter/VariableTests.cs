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
            var memory = Runner.Run("""
                                    x = [1, 2, 3]
                                    y = x[1]
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 2);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void GetsItemWithNumericKeyFromDictionary()
        {
            var memory = Runner.Run("""
                                    x = {1: "asdf", 2: "qwer"}
                                    y = x[1]
                                    """);
            var expected = new StringVariable("asdf");
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void GetsItemWithStringKeyFromDictionary()
        {
            var memory = Runner.Run("""
                                    x = {1: "asdf", "zxcv": "qwer"}
                                    y = x["zxcv"]
                                    """);
            var expected = new StringVariable("qwer");
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void GetsItemsFromObjectIfOverrideExists()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                        def __getitem__(self, index):
                                            return 6
                                            
                                    x = asdf()
                                    y = x[1]
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 6);
            Assertions.AssertVariable(memory, "y", expected);
        }
    }

    [TestFixture]
    public class SetItem
    {
        [Test]
        public void SetsItemInList()
        {
            var memory = Runner.Run("""
                                    x = [1, 2, 3]
                                    x[1] = 4
                                    y = x[1]
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 4);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SetsItemWithNumericKeyInDictionary()
        {
            var memory = Runner.Run("""
                                    x = {1: "asdf", 2: "qwer"}
                                    x[1] = "zxcv"
                                    y = x[1]
                                    """);
            var expected = new StringVariable("zxcv");
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SetsItemWithStringKeyInDictionary()
        {
            var memory = Runner.Run("""
                                    x = {1: "asdf", "zxcv": "qwer"}
                                    x["zxcv"] = 2
                                    y = x["zxcv"]
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 2);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void SetsItemFromObjectIfOverrideExists()
        {
            var memory = Runner.Run("""
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
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 9);
            Assertions.AssertVariable(memory, "y", expected);
        }
    }

    [TestFixture]
    public class GetMember
    {
        [Test]
        public void GetsMemberFromObject()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    x = asdf()
                                    y = x.i
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 5);
            Assertions.AssertVariable(memory, "y", expected);
        }

        [Test]
        public void GetsMemberFromClass()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    y = asdf.i
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 5);
            Assertions.AssertVariable(memory, "y", expected);
        }
    }

    [TestFixture]
    public class SetMember
    {
        [Test]
        public void SetsMemberInObject()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        i = 5
                                        
                                    x = asdf()
                                    x.i = 9
                                    y = x.i
                                    """);
            var expected = memory.CreateBsType(Memory.BsTypes.Int, 9);
            Assertions.AssertVariable(memory, "y", expected);
        }
    }
}