using Battlescript;

namespace BattlescriptTests.Errors.SyntaxErrors;

public class InvalidAssignmentsTests
{
    [Test]
        public void AssigningToALiteral()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("3 = x"));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to literal"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
    
        [Test]
        public void AssigningToAFunctionCall()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("print('asdf') = 3"));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAUserDefinedFunctionCall()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            def func():
                                                                                pass
                                                                            
                                                                            func() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAFunctionCallWithIndex()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            def func():
                                                                                pass
                                                                                
                                                                            x = [func]
                                                                            x[0]() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
        
        [Test]
        public void AssigningToAFunctionCallWithMember()
        {
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run("""
                                                                            class MyClass:
                                                                                def func(self):
                                                                                    pass
                                                                                
                                                                            x = MyClass()
                                                                            x.func() = 3
                                                                            """));
            Assert.That(ex.Message, Is.EqualTo("cannot assign to function call"));
            Assert.That(ex.Type, Is.EqualTo(Memory.BsTypes.SyntaxError));
        }
}