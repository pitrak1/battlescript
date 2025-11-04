using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class CallStackTests
{
    [TestFixture]
    public class Initialization
    {
        [Test]
        public void CreatesMainScopeOnObjectCreation()
        {
            var memory = new CallStack();
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assertions.AssertStackFrame(memory.Scopes[0], new StackFrame("main", "<module>"));
        }
    }
    
    [TestFixture]
    public class AddScope
    {
        [Test]
        public void UpdatesPreviousScopeWithLineAndExpression()
        {
            var memory = new CallStack();
            memory.AddScope(5, "x = 5", "func", "file.bs");
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assertions.AssertStackFrame(memory.Scopes[0], new StackFrame("main", 5, "x = 5", "<module>"));
        }
        
        [Test]
        public void CreatesNewScopeWithoutLineOrExpression()
        {
            var memory = new CallStack();
            memory.AddScope(5, "x = 5", "func", "file.bs");
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assertions.AssertStackFrame(memory.Scopes[1], new StackFrame("file.bs", "func"));
        }
    
        [Test]
        public void UsesExistingFileIfNotGiven()
        {
            var memory = new CallStack();
            memory.AddScope(5, "x = 5", "func");
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assertions.AssertStackFrame(memory.Scopes[1], new StackFrame("main", "func"));
        }
    }
    
    [TestFixture]
    public class RemoveScope()
    {
        [Test]
        public void RemovesAndReturnsLastScopeInList()
        {
            var memory = new CallStack();
            memory.AddScope(5, "x = 5", "func");
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assertions.AssertStackFrame(returnedScope, new StackFrame("main", "func"));
        }
    }
    
    // [TestFixture]
    // public class GetVariable()
    // {
    //     [Test]
    //     public void GetsVariableInLastScope()
    //     {
    //         var scopeVariables = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(5) }
    //         };
    //         var callStack = new CallStack();
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables));
    //         var returnedVariable = callStack.GetVariable("x");
    //         
    //         Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
    //     }
    //     
    //     [Test]
    //     public void GetsVariableNotInLastScope()
    //     {
    //         var scopeVariables1 = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(5) }
    //         };
    //         var scopeVariables2 = new Dictionary<string, Variable>()
    //         {
    //             { "y", new NumericVariable(8) }
    //         };
    //         var callStack = new CallStack();
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables1));
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables2));
    //         var returnedVariable = callStack.GetVariable("x");
    //         
    //         Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
    //     }
    //
    //     [Test]
    //     public void PrefersVariablesInLaterScopes()
    //     {
    //         var scopeVariables1 = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(5) }
    //         };
    //         var scopeVariables2 = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(8) }
    //         };
    //         var callStack = new CallStack();
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables1));
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables2));
    //         var returnedVariable = callStack.GetVariable("x");
    //         
    //         Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(8));
    //     }
    // }
    //
    // [TestFixture]
    // public class SetVariable
    // {
    //     [Test]
    //     public void CreatesVariableInLastScopeIfDoesNotExist()
    //     {
    //         var callStack = new CallStack();
    //         callStack.AddScope();
    //         callStack.SetVariable(new VariableInstruction("x"), new NumericVariable(5));
    //         var scopes = callStack.Scopes;
    //         
    //         Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
    //     }
    //
    //     [Test]
    //     public void AssignsToVariableIfExists()
    //     {
    //         var scopeVariables = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(5) }
    //         };
    //         var callStack = new CallStack();
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables));
    //         callStack.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
    //         var scopes = callStack.Scopes;
    //         
    //         Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(8));
    //     }
    //
    //     [Test]
    //     public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
    //     {
    //         var scopeVariables1 = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(5) }
    //         };
    //         var scopeVariables2 = new Dictionary<string, Variable>()
    //         {
    //             { "x", new NumericVariable(6) }
    //         };
    //         var callStack = new CallStack();
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables1));
    //         callStack.AddScope(new Dictionary<string, Variable>(scopeVariables2));
    //         callStack.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
    //         var scopes = callStack.Scopes;
    //         
    //         Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
    //         
    //         Assertions.AssertVariablesEqual(scopes[2]["x"], new NumericVariable(8));
    //     }
    // }

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
                Assertions.AssertStacktrace(memory.Scopes, [
                    new StackFrame("main", 7, "y = x()", "<module>"),
                    new StackFrame("main", 5, "z()", "x"),
                    new StackFrame("main", 2, "raise SyntaxError(\"asdf\")", "z"),
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
                Assertions.AssertStacktrace(memory.Scopes, [
                    new StackFrame("main", 5, "y = x()", "<module>"),
                    new StackFrame("main", 3, "raise SyntaxError(\"asdf\")", "__init__"),
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
                Assertions.AssertStacktrace(memory.Scopes, [
                    new StackFrame("main", 6, "y = z.operate(5)", "<module>"),
                    new StackFrame("main", 3, "raise SyntaxError(\"asdf\")", "operate"),
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
                Assertions.AssertStacktrace(memory.Scopes, [
                    new StackFrame("main", 1, "from '/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs' import x", "<module>"),
                    new StackFrame("/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs", 5, "y = x()", "<module>"),
                    new StackFrame("/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs", 3, "raise SyntaxError(\"asdf\")", "__init__"),
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
                Assertions.AssertStacktrace(memory.Scopes, [
                    new StackFrame("main", 4, "raise SyntaxError(\"asdf\")", "<module>"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
    }
}