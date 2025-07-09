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
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234)}
                });
            
            Assert.That(memory.Scopes[0], Contains.Key("asdf"));
            Assert.That(memory.Scopes[0]["asdf"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var memory = Runner.Run(input);
            var values = new Dictionary<string, Variable>()
            {
                { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234) }
            };
            var expected = new ObjectVariable(
                values,
                new ClassVariable(values));
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
                { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234) },
                { "j", new FunctionVariable([], [new ReturnInstruction(new NumericInstruction(1234))])}
            };
            var objectValues = new Dictionary<string, Variable>()
            {
                { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234) }
            };
            
            var expected = new ObjectVariable(
                objectValues,
                new ClassVariable(classValues));
            
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var memory = Runner.Run(input);
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234);
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ChangingValueMembersDoesNotAlterClass()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
            var memory = Runner.Run(input);
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234) }
                }
            );
            
            Assert.That(memory.Scopes[0], Contains.Key("asdf"));
            Assert.That(memory.Scopes[0]["asdf"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2345);
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
                new Dictionary<string, Variable>()
                {
                    { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1234) }
                });
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "j", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2345) }
                }, [superclass]);
            
            Assert.That(memory.Scopes[0]["qwer"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 10);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
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
            var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -5);
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }

//         [Test]
//         public void AllowsAssignmentOperationOverloading()
//         {
//             var input = """
//                         class asdf:
//                             i = 5
//                         
//                             def __iadd__(self, other):
//                                 self.i += other.i
//                                 return self
//
//                         z = asdf()
//                         y = asdf()
//                         z += y
//                         x = z.i
//                         """;
//             var memory = Runner.Run(input);
//             var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 10);
//             Assert.That(memory.Scopes[0], Contains.Key("x"));
//             Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
//         }
    }
}