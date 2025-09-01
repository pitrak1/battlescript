using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class MemoryTests
{
    [TestFixture]
    public class Initialization
    {
        [Test]
        public void CreatesEmptyScopeOnObjectCreation()
        {
            var memory = new Memory();

            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class AddScope
    {
        [Test]
        public void AddsEmptyScopeIfNoArgumentProvided()
        {
            var memory = new Memory();
            memory.AddScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1], Is.Empty);
        }

        [Test]
        public void AddsExistingScopeIfArgumentProvided()
        {
            var memory = new Memory();
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables));
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1], Is.EquivalentTo(scopeVariables));
        }
    }

    [TestFixture]
    public class RemoveScope()
    {
        [Test]
        public void RemovesAndReturnsLastScopeInList()
        {
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables));
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assert.That(returnedScope, Is.EquivalentTo(scopeVariables));
        }
    }
    
    [TestFixture]
    public class RemoveScopes()
    {
        [Test]
        public void RemovesScopeCountGiven()
        {
            var memory = new Memory();
            memory.AddScope();
            memory.AddScope();
            memory.AddScope();
            memory.RemoveScopes(3);
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class GetVariable()
    {
        [Test]
        public void GetsVariableInLastScope()
        {
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables));
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }
        
        [Test]
        public void GetsVariableNotInLastScope()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "y", new NumericVariable(8) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables1));
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables2));
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }

        [Test]
        public void PrefersVariablesInLaterScopes()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(8) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables1));
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables2));
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(8));
        }
    }

    [TestFixture]
    public class SetVariable
    {
        [Test]
        public void CreatesVariableInLastScopeIfDoesNotExist()
        {
            var memory = new Memory();
            memory.AddScope();
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(5));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
        }

        [Test]
        public void AssignsToVariableIfExists()
        {
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables));
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(8));
        }

        [Test]
        public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(6) }
            };
            var memory = new Memory();
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables1));
            memory.AddScope(new Dictionary<string, Variable>(scopeVariables2));
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
            
            Assertions.AssertVariablesEqual(scopes[2]["x"], new NumericVariable(8));
        }
    }

    [TestFixture]
    public class Stacktrace
    {
        [Test]
        public void RespectsFunctionNames()
        {
            var memory = Runner.Run("");
            var input = """
                        def z():
                            raise SyntaxError("asdf")
                        
                        def x():
                            z()
                        
                        y = x()
                        """;
            
            bool caught = false;
            try
            {
                Runner.RunAsMain(memory, input);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(memory.CurrentStack.Frames, [
                    ("main", 7, "y = x()", "<module>"),
                    ("main", 5, "z()", "x"),
                    ("main", 2, "raise SyntaxError(\"asdf\")", "z"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
        
        [Test]
        public void RespectsClassConstructors()
        {
            var memory = Runner.Run("");
            var input = """
                        class x:
                            def __init__(self):
                                raise SyntaxError("asdf")
                                
                        y = x()
                        """;
            
            bool caught = false;
            try
            {
                Runner.RunAsMain(memory, input);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(memory.CurrentStack.Frames, [
                    ("main", 5, "y = x()", "<module>"),
                    ("main", 3, "raise SyntaxError(\"asdf\")", "__init__"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
        
        [Test]
        public void RespectsClassMethods()
        {
            var memory = Runner.Run("");
            var input = """
                        class x:
                            def operate(self, y):
                                raise SyntaxError("asdf")
                                
                        z = x()
                        y = z.operate(5)
                        """;
            
            bool caught = false;
            try
            {
                Runner.RunAsMain(memory, input);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(memory.CurrentStack.Frames, [
                    ("main", 6, "y = z.operate(5)", "<module>"),
                    ("main", 3, "raise SyntaxError(\"asdf\")", "operate"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
        
        [Test]
        public void RespectsFileName()
        {
            var memory = Runner.Run("");
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs";
            var input = $"from '{filePath}' import x";
            
            bool caught = false;
            try
            {
                Runner.RunAsMain(memory, input);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(memory.CurrentStack.Frames, [
                    ("main", 1, "from '/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs' import x", "<module>"),
                    ("/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs", 5, "y = x()", "<module>"),
                    ("/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs", 3, "raise SyntaxError(\"asdf\")", "__init__"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }

        [Test]
        public void DoesNotIncludeLoopsAndConditionals()
        {
            var memory = Runner.Run("");
            var input = """
                        i = 0
                        while i < 5:
                            if i == 4:
                                raise SyntaxError("asdf")
                            i = i + 1
                        """;
            
            bool caught = false;
            try
            {
                Runner.RunAsMain(memory, input);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(memory.CurrentStack.Frames, [
                    ("main", 4, "raise SyntaxError(\"asdf\")", "<module>"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
    }
}