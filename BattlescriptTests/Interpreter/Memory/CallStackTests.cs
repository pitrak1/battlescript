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
             var callStack = new CallStack();
             Assert.That(callStack.Frames.Count, Is.EqualTo(1));
             Assertions.AssertStackFrame(callStack.Frames[0], new StackFrame("main", "<module>"));
         }
     }
     
     [TestFixture]
     public class AddFrame
     {
         [Test]
         public void UpdatesPreviousScopeWithLineAndExpression()
         {
             var callStack = new CallStack();
             callStack.AddFrame(5, "x = 5", "func", "file.bs");
             
             Assert.That(callStack.Frames.Count, Is.EqualTo(2));
             Assertions.AssertStackFrame(callStack.Frames[0], new StackFrame("main", 5, "x = 5", "<module>"));
         }
         
         [Test]
         public void CreatesNewScopeWithoutLineOrExpression()
         {
             var callStack = new CallStack();
             callStack.AddFrame(5, "x = 5", "func", "file.bs");
             
             Assert.That(callStack.Frames.Count, Is.EqualTo(2));
             Assertions.AssertStackFrame(callStack.Frames[1], new StackFrame("file.bs", "func"));
         }
     
         [Test]
         public void UsesExistingFileIfNotGiven()
         {
             var callStack = new CallStack();
             callStack.AddFrame(5, "x = 5", "func");
             
             Assert.That(callStack.Frames.Count, Is.EqualTo(2));
             Assertions.AssertStackFrame(callStack.Frames[1], new StackFrame("main", "func"));
         }
     }
     
     [TestFixture]
     public class RemoveFrame()
     {
         [Test]
         public void RemovesAndReturnsLastScopeInList()
         {
             var callStack = new CallStack();
             callStack.AddFrame(5, "x = 5", "func");
             var returnedScope = callStack.RemoveFrame();
             
             Assert.That(callStack.Frames.Count, Is.EqualTo(1));
             Assertions.AssertStackFrame(returnedScope, new StackFrame("main", "func"));
         }
     }
     
    [TestFixture]
    public class Stacktrace
    {
        [Test]
        public void RespectsFunctionNames()
        {
            var input = """
                        def z():
                            raise SyntaxError("asdf")
                        
                        def x():
                            z()
                        
                        y = x()
                        """;
            var callStack = new CallStack();
            var closure = new Closure();
            bool caught = false;
            try
            {
                Runner.Run(input, callStack, closure);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(callStack.Frames, [
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
            var input = """
                        class x:
                            def __init__(self):
                                raise SyntaxError("asdf")
                                
                        y = x()
                        """;
            var callStack = new CallStack();
            var closure = new Closure();
            bool caught = false;
            try
            {
                Runner.Run(input, callStack, closure);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(callStack.Frames, [
                    new StackFrame("main", 5, "y = x()", "<module>"),
                    new StackFrame("main", 3, "raise SyntaxError(\"asdf\")", "__init__"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
        
        [Test]
        public void RespectsClassMethods()
        {
            var input = """
                        class x:
                            def operate(self, y):
                                raise SyntaxError("asdf")
                                
                        z = x()
                        y = z.operate(5)
                        """;
            var callStack = new CallStack();
            var closure = new Closure();
            bool caught = false;
            try
            {
                Runner.Run(input, callStack, closure);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(callStack.Frames, [
                    new StackFrame("main", 6, "y = z.operate(5)", "<module>"),
                    new StackFrame("main", 3, "raise SyntaxError(\"asdf\")", "operate"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
        
        [Test]
        public void RespectsFileName()
        {
            var filePath = @"/Users/nickpitrak/Desktop/Battlescript/BattlescriptTests/TestFiles/stacktrace.bs";
            var input = $"from '{filePath}' import x";
            var callStack = new CallStack();
            var closure = new Closure();
            bool caught = false;
            try
            {
                Runner.Run(input, callStack, closure);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(callStack.Frames, [
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
            var input = """
                        i = 0
                        while i < 5:
                            if i == 4:
                                raise SyntaxError("asdf")
                            i = i + 1
                        """;
            var callStack = new CallStack();
            var closure = new Closure();
            bool caught = false;
            try
            {
                Runner.Run(input, callStack, closure);
            }
            catch (InternalRaiseException e)
            {
                caught = true;
                Assertions.AssertStacktrace(callStack.Frames, [
                    new StackFrame("main", 4, "raise SyntaxError(\"asdf\")", "<module>"),
                ]);
            }
            
            Assert.That(caught, Is.True);
        }
    }
}