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
            var memory = Runner.Run(input);
            var expected = new ClassVariable("asdf",
                new Dictionary<string, Variable>()
                {
                    {"i", BsTypes.Create(memory, "int", 1234)}
                });
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["asdf"], expected);
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var memory = Runner.Run(input);
            var values = new Dictionary<string, Variable>()
            {
                { "i", BsTypes.Create(memory, "int", 1234) }
            };
            var expected = new ObjectVariable(
                values,
                new ClassVariable("asdf", values));
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var memory = Runner.Run(input);
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", BsTypes.Create(memory, "int", 1234) },
                { "j", new FunctionVariable([], [new ReturnInstruction(new NumericInstruction(5))])}
            };
            var objectValues = new Dictionary<string, Variable>()
            {
                { "i", BsTypes.Create(memory, "int", 1234) }
            };
            
            var expected = new ObjectVariable(
                objectValues,
                new ClassVariable("asdf", classValues));
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 1234);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], expected);
        }
        
        [Test]
        public void ChangingValueMembersDoesNotAlterClass()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
            var memory = Runner.Run(input);
            var expected = new ClassVariable("asdf",
                new Dictionary<string, Variable>()
                {
                    { "i", BsTypes.Create(memory, "int", 1234) }
                }
            );
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["asdf"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 2345);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 5);
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 6);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
        }
        
        
    }

    [TestFixture]
    public class Inheritance
    {
        [Test]
        public void AllowsSuperclasses()
        {
            var input = "class asdf:\n\ti = 1234\nclass qwer(asdf):\n\tj = 2345";
            var memory = Runner.Run(input);
            var superclass = new ClassVariable(
                "asdf", new Dictionary<string, Variable>()
                {
                    { "i", BsTypes.Create(memory, "int", 1234) }
                });
            var expected = new ClassVariable("qwer",
                new Dictionary<string, Variable>()
                {
                    { "j", BsTypes.Create(memory, "int", 2345) }
                }, [superclass]);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["qwer"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 9);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", 10);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
            var memory = Runner.Run(input);
            var expected = BsTypes.Create(memory, "int", -5);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
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
//               var memory = Runner.Run(input);
//               var expected = BuiltInTypeHelper.Create(memory, "int", 10);
//               Assert.That(memory.Scopes[0], Contains.Key("x"));
//               Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
//           }
    }
}