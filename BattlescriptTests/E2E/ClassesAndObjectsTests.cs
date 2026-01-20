using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public static class ClassesAndObjectsTests
{

    [TestFixture]
    public class BasicClasses
    {
        [Test]
        public void AllowsDefiningClasses()
        {
            var input = "class asdf:\n\ti = 1234";
            var (callStack, closure) = Runner.Run(input);
            var expected = new ClassVariable("asdf",
                new Dictionary<string, Variable>()
                {
                    {"i", BtlTypes.Create(BtlTypes.Types.Int, 1234)}
                }, closure);
            
            Assertions.AssertVariable(callStack, closure, "asdf", expected);
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var (callStack, closure) = Runner.Run(input);
            var values = new Dictionary<string, Variable>()
            {
                { "i", BtlTypes.Create(BtlTypes.Types.Int, 1234) }
            };
            var expected = new ObjectVariable(
                values,
                new ClassVariable("asdf", values, closure));
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void IgnoresMethodsWhenCreatingObjects()
        {
            var input = """
                        class asdf:
                            i = 1234
                            def j():
                                return 5

                        x = asdf()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                { "j", new FunctionVariable("j", closure, new ParameterSet(), [new ReturnInstruction(new NumericInstruction(5))])}
            };
            var objectValues = new Dictionary<string, Variable>()
            {
                { "i", BtlTypes.Create(BtlTypes.Types.Int, 1234) }
            };
            
            var expected = new ObjectVariable(
                objectValues,
                new ClassVariable("asdf", classValues, closure));
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 1234);
            
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
        
        [Test]
        public void ChangingValueMembersDoesNotAlterClass()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
            var (callStack, closure) = Runner.Run(input);
            var expected = new ClassVariable("asdf",
                new Dictionary<string, Variable>()
                {
                    { "i", BtlTypes.Create(BtlTypes.Types.Int, 1234) }
                },
                closure
            );
            
            Assertions.AssertVariable(callStack, closure, "asdf", expected);
        }
        
        [Test]
        public void AllowsAccessingMethods()
        {
            var input = """
                        class asdf:
                            i = 1234
                        
                            def j(self):
                                self.i = 2345
                        x = asdf()
                        x.j()
                        y = x.i

                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 2345);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
    }
    
    [TestFixture]
    public class Constructors
    {
        [Test]
        public void SupportsClassesWithNoConstructors()
        {
            var input = """
                        class asdf:
                            y = 5
                        z = asdf()
                        x = z.y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 5);
            
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsConstructorsWithNoParameters()
        {
            var input = """
                        class asdf:
                            y = 5
                        
                            def __init__(self):
                                self.y = 6
                        z = asdf()
                        x = z.y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 6);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void SupportsConstructorsWithParameters()
        {
            var input = """
                        class asdf:
                            y = 5
                        
                            def __init__(self, a):
                                self.y = a
                        z = asdf(9)
                        x = z.y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        
    }

    [TestFixture]
    public class Inheritance
    {
        [Test]
        public void AllowsSuperclasses()
        {
            var input = "class asdf:\n\ti = 1234\nclass qwer(asdf):\n\tj = 2345";
            var (callStack, closure) = Runner.Run(input);
            var superclass = new ClassVariable(
                "asdf", new Dictionary<string, Variable>()
                {
                    { "i", BtlTypes.Create(BtlTypes.Types.Int, 1234) }
                }, closure);
            var expected = new ClassVariable("qwer",
                new Dictionary<string, Variable>()
                {
                    { "j", BtlTypes.Create(BtlTypes.Types.Int, 2345) }
                }, closure, [superclass]);
            Assertions.AssertVariable(callStack, closure, "qwer", expected);
        }
        
        [Test]
        public void SupportsConstructorsInSuperclasses()
        {
            var input = """
                        class asdf:
                            def __init__(self, a):
                                self.y = a

                        class qwer(asdf):
                            y = 6

                        z = qwer(9)
                        x = z.y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 9);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }

    [TestFixture]
    public class OperatorOverloading
    { 
        [Test]
        public void AllowsBinaryOperationOverloading()
        {
            var input = """
                        class asdf:
                            i = 5
                        
                            def __add__(self, other):
                                return self.i + other.i

                        z = asdf()
                        y = asdf()
                        x = z + y
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, 10);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsUnaryOperationOverloading()
        {
            var input = """
                        class asdf:
                            i = 5
                        
                            def __neg__(self):
                                return -self.i

                        z = asdf()
                        x = -z
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BtlTypes.Create(BtlTypes.Types.Int, -5);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        // Definitely need to take a closer look here.  We may have to do some wonky stuff to make this work right
//           [Test]
//           public void AllowsAssignmentOperationOverloading()
//           {
//               var input = """
//                           class asdf:
//                               i = 5
//                           
//                               def __iadd__(self, other):
//                                   self.i = self.i + other.i
//                                   return self
//
//                           z = asdf()
//                           y = asdf()
//                           z += y
//                           x = z.i
//                           """;
//               var callStack = Runner.Run(input);
//               var expected = BuiltInTypeHelper.Create(callStack, BtlTypes.Types.Int, 10);
//               Assert.That(callStack.Scopes[0], Contains.Key("x"));
//               Assert.That(callStack.Scopes[0]["x"], Is.EqualTo(expected));
//           }
    }
}
